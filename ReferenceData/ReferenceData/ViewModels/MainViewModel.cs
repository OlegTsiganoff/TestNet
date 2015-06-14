using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ReferenceData.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<UserViewModel> users;
        private ObservableCollection<CountryViewModel> countries;
        private ObservableCollection<SubdivisionViewModel> subdivisions;
        private ObservableCollection<LocationViewModel> locations;
        public ObservableCollection<UserViewModel> Users
        {
            get { return users; }
            set
            {
                users = value;
                //NotifyPropertyChanged("Users");
            }
        }
        public ObservableCollection<CountryViewModel> Countries 
        {
            get { return countries; }
            set
            {
                countries = value;
                //NotifyPropertyChanged("Countries");
            }
        }

        public ObservableCollection<SubdivisionViewModel> Subdivisions
        {
            get { return subdivisions; }
            set
            {
                subdivisions = value;
                //NotifyPropertyChanged("Subdivisions");
            }
        }
        public ObservableCollection<LocationViewModel> Locations
        {
            get { return locations; }
            set
            {
                locations = value;
                //NotifyPropertyChanged("Locations");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Users = new ObservableCollection<UserViewModel>();
            Countries = new ObservableCollection<CountryViewModel>();
            Subdivisions = new ObservableCollection<SubdivisionViewModel>();
            Locations = new ObservableCollection<LocationViewModel>();
        }

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
