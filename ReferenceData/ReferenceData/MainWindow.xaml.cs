using System.Windows;
using ReferenceData.DAL.Services;
using ReferenceData.DAL.Model;
using System.Data.Objects;
using System.Linq;
using System.Collections.Generic;
using ReferenceData.ViewModels;
using System.Windows.Data;
using System.Windows.Controls;
using System;
using System.Windows.Threading;
using System.Windows.Media;

namespace ReferenceData
{
    public partial class MainWindow : Window
    {
        MainViewModel mainViewModel;
        System.Timers.Timer timer;
        
        public MainWindow()
        {
            InitializeComponent();
            MyInit();
        }

        void MyInit()
        {
            dataGrid.SelectionChanged += dataGrid_SelectionChanged;            
            mainViewModel = new MainViewModel();

            InitTimer();

            buttonNew.Click += buttonNew_Click;
            buttonSave.Click += buttonSave_Click;
            buttonCancel.Click += buttonCancel_Click;

            Binding bindButtonSaveIsEnabled = new Binding("SelectedItem");
            bindButtonSaveIsEnabled.Source = dataGrid;
            bindButtonSaveIsEnabled.Converter = new DataGridSelectedItemToEnabledConverter();
            buttonSave.SetBinding(Button.IsEnabledProperty, bindButtonSaveIsEnabled);

            comboBoxCountry.SelectionChanged += comboBoxCountry_SelectionChanged;            
        }

        private void InitTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 3000;
            timer.Elapsed += timer_Elapsed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
            FillComboBoxCountry();
            FillComboBoxSubdivision();
            FillComboBoxLocation();
        }

        void comboBoxCountry_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            CountryViewModel item = e.AddedItems[0] as CountryViewModel;
            if(item.Name == null)
            {
                borderofComboCountry.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                borderofComboCountry.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
        }


        void FillDataGrid()
        {
            var users = new UsersService().GetItemsWithProperties();

            var query = from user in users
                        select new UserViewModel()
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            SecondName = user.SecondName,
                            Country = user.Country.Description,
                            Subdivision = user.Subdivision == null ? null : user.Subdivision.Description,   // field can be empty
                            Location = user.Location == null ? null : user.Location.Description             // field can be empty
                        };

            mainViewModel.Users = new System.Collections.ObjectModel.ObservableCollection<UserViewModel>(query.ToList());

            dataGrid.DataContext = mainViewModel.Users;
        }

        void FillComboBoxCountry()
        {
            var countries = new CountriesService().GetItems();
            var query = from country in countries
                         select new CountryViewModel() { Id = country.Id, Name = country.Description };
            mainViewModel.Countries = new System.Collections.ObjectModel.ObservableCollection<CountryViewModel>(query.ToList());

            // adding empty field to coolection for ability to select empty item in Country combobox
            CountryViewModel emptycountry = new CountryViewModel();
            mainViewModel.Countries.Insert(0, emptycountry);

            comboBoxCountry.ItemsSource = mainViewModel.Countries;
        }

        void FillComboBoxSubdivision()
        {
            var subdivisions = new SubdivisionService().GetItems();
            var query = from subdivision in subdivisions
                        select new SubdivisionViewModel() { Id = subdivision.Id, Name = subdivision.Description };

            // removing duplicates from collection            
            mainViewModel.Subdivisions = new System.Collections.ObjectModel.ObservableCollection<SubdivisionViewModel>(query.Distinct().ToList());            
            comboBoxSubdivision.ItemsSource = mainViewModel.Subdivisions;
        }

        void FillComboBoxLocation()
        {
            var locations = new LocationsService().GetItems();
            var query = from location in locations
                        select new LocationViewModel() { Id = location.Id, Name = location.Description };

            // removing duplicates from collection 
            mainViewModel.Locations = new System.Collections.ObjectModel.ObservableCollection<LocationViewModel>(query.ToList().Distinct());
            comboBoxLocation.ItemsSource = mainViewModel.Locations;
        }
        
        void dataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            UserViewModel userTmp = (e.AddedItems[0] as UserViewModel);

            if (userTmp == null)
            {
                ClearControls();
                return;
            }

            UserViewModel user = new UserViewModel(userTmp); // new item for saving data in grid. We can change data, but don't commit by "save" or "new"

            txtBoxFirstName.DataContext = user;
            txtBoxSecondName.DataContext = user;

            // lookong for appropriate item in combobox items by name
            if(user.Country == null) // some times shit happens
            {
                comboBoxCountry.SelectedItem = null; 
            }
            else
                foreach (var item in comboBoxCountry.Items)
                {
                    CountryViewModel country = item as CountryViewModel;
                    if (country == null || country.Name == null)
                        continue;
                    if (country.Name.Equals(user.Country))
                    {
                        // and select 
                        comboBoxCountry.SelectedItem = item;
                        break;
                    }
                }

            if (user.Subdivision == null)
            {
                comboBoxSubdivision.SelectedItem = null;
            }
            else
            {
                foreach (var item in comboBoxSubdivision.Items)
                {
                    SubdivisionViewModel subdivision = item as SubdivisionViewModel;
                    if (subdivision == null)
                        continue;
                    if (subdivision.Name.Equals(user.Subdivision))
                    {
                        comboBoxSubdivision.SelectedItem = item;
                        break;
                    }
                }
            }


            if (user.Location == null)
            {
                comboBoxLocation.SelectedItem = null;
            }
            else
            {
                foreach (LocationViewModel item in comboBoxLocation.Items)
                {
                    LocationViewModel location = item as LocationViewModel;
                    if (location == null)
                        continue;
                    if (location.Name.Equals(user.Location))
                    {
                        comboBoxLocation.SelectedItem = item;
                        break;
                    }
                }
            }
        }


        void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            // prepare error message 
            string messageError = ValidateControls();            

            // when we have an error show message box
            if(!messageError.Equals(string.Empty))
            {
                MessageBox.Show(messageError, "Error!");
                return;
            }

            // receive selected items from controls
            CountryViewModel country = comboBoxCountry.SelectedItem as CountryViewModel;
            SubdivisionViewModel subdivision = comboBoxSubdivision.SelectedItem as SubdivisionViewModel;
            LocationViewModel location = comboBoxLocation.SelectedItem as LocationViewModel;
                        
            // create new user
            User newUser = new User();
            // and don't fill User.Id field for adding new item to database
            
            newUser.FirstName = txtBoxFirstName.Text;
            newUser.SecondName = txtBoxSecondName.Text;
            if(country != null)
                newUser.CountryId = country.Id;
            if(subdivision != null)
                newUser.SubDivisionId = subdivision.Id;
            if(location != null)
                newUser.LocationId = location.Id;
            new UsersService().AddOrUpdate(newUser);
            // refresh data in datagrid
            FillDataGrid();

            // show message to inform user that new item successfully created
            txtBlockSuccess.Text = "new data created...";
            txtBlockSuccess.Visibility = System.Windows.Visibility.Visible;

            // timer will hide message after a few seconds
            if(timer == null)
                InitTimer();
            timer.Start();
            
        }      

        void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            // prepare error message             
            string messageError = ValidateControls();

            // when we have an error show message box
            if (!messageError.Equals(string.Empty))
            {
                MessageBox.Show(messageError, "Error!");
                return;
            }

            // receive selected items from controls
            UserViewModel user = dataGrid.SelectedItem as UserViewModel;
            CountryViewModel country = comboBoxCountry.SelectedItem as CountryViewModel;
            SubdivisionViewModel subdivision = comboBoxSubdivision.SelectedItem as SubdivisionViewModel;
            LocationViewModel location = comboBoxLocation.SelectedItem as LocationViewModel;

            if (user == null || country == null || subdivision == null || location == null)
                return;

            // if we didn't change data do nothing
            if (user.FirstName.Equals(txtBoxFirstName.Text) &&
                user.SecondName.Equals(txtBoxSecondName.Text) &&
                user.Country.Equals(country.Name) &&
                user.Subdivision.Equals(subdivision.Name) &&
                user.Location.Equals(location.Name))
                return;
            else
            {
                // create new user item
                User newUser = new User();
                // fill the fields
                newUser.Id = user.Id;  // also fill Id field for update data in database
                newUser.FirstName = txtBoxFirstName.Text;
                newUser.SecondName = txtBoxSecondName.Text;
                newUser.CountryId = country.Id;
                newUser.SubDivisionId = subdivision.Id;
                newUser.LocationId = location.Id;
                // and add to database
                new UsersService().AddOrUpdate(newUser);
                // refresh data in datagrid
                FillDataGrid();

                // show message to inform user that item successfully saved                
                txtBlockSuccess.Text = "data saved...";
                txtBlockSuccess.Visibility = System.Windows.Visibility.Visible;

                // timer will hide message after a few seconds
                if (timer == null)
                    InitTimer();
                timer.Start();
            }            
        }

        void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            // clear all controls
            ClearControls();
        }

        private void ClearControls()
        {
            dataGrid.SelectedItem = null;
            txtBoxFirstName.DataContext = null;
            txtBoxFirstName.Text = string.Empty;
            txtBoxSecondName.DataContext = null;
            txtBoxSecondName.Text = string.Empty;
            comboBoxCountry.SelectedItem = null;
            borderofComboCountry.BorderBrush = new SolidColorBrush(Colors.Transparent);
            comboBoxSubdivision.SelectedItem = null;
            comboBoxLocation.SelectedItem = null;
        }

        private string ValidateControls()
        {
            string messageError = string.Empty;
            if (txtBoxFirstName.Text == string.Empty)
                messageError += "First name mastn't be empty.";
            if (txtBoxSecondName.Text == string.Empty)
            {
                if (messageError.Length > 0)
                    messageError += Environment.NewLine;
                messageError += "Second name mastn't be empty.";
            }

            if (comboBoxCountry.SelectedItem == null)
            {
                if (messageError.Length > 0)
                    messageError += Environment.NewLine;
                messageError += "Select country from the list.";
            }
            else
            {
                if ((comboBoxCountry.SelectedItem as CountryViewModel) != null && (comboBoxCountry.SelectedItem as CountryViewModel).Name == null)
                {
                    if (messageError.Length > 0)
                        messageError += Environment.NewLine;
                    messageError += "Select country from the list.";
                }
            }
            return messageError;
        }        

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            // this code runing from another thread
            // and we can change controls from UI thread
            Dispatcher.Invoke((Action)delegate 
            {
                if (txtBlockSuccess != null)
                    txtBlockSuccess.Visibility = System.Windows.Visibility.Hidden;
            });
        }

       
    }
}
