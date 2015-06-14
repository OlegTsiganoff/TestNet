using System;
using System.ComponentModel;

namespace ReferenceData.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private int id;
        private string firstName;
        private string secondName;
        private string country;
        private string subdivision;
        private string location;


        public int Id
        {
            get { return id; }
            set 
            {                 
                id = value;
                NotifyPropertyChanged("Id");
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set 
            {
                firstName = value;
                NotifyPropertyChanged("FirstName");
            }
        }

        public string SecondName
        {
            get { return secondName; }
            set
            { 
                secondName = value;
                NotifyPropertyChanged("SecondName");
            }
        }

        public string Country
        {
            get { return country; }
            set 
            { 
                country = value;
                NotifyPropertyChanged("Country");
            }
        }

        public string Subdivision
        {
            get { return subdivision; }
            set 
            { 
                subdivision = value;
                NotifyPropertyChanged("Subdivision");
            }
        }

        public string Location
        {
            get { return location; }
            set 
            { 
                location = value;
                NotifyPropertyChanged("Location");
            }
        }

        public UserViewModel() { }
        public UserViewModel(UserViewModel inUser)
        {
            id = inUser.Id;
            firstName = inUser.FirstName;
            SecondName = inUser.SecondName;
            Country = inUser.Country;
            Subdivision = inUser.Subdivision;
            Location = inUser.Location;
        }


        public event PropertyChangedEventHandler PropertyChanged;  

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get 
            {
                string result = null;
                switch(columnName)
                {
                    case "FirstName":
                        if (firstName == null || firstName.Length == 0)
                            result =  "The First Name mast not be empty";
                        break;
                    case "SecondName":
                        if (secondName == null || secondName.Length == 0)
                            result = "Second Name mast not be empty";
                        break;
                    case "Country":
                        if (country == null || country.Length == 0)
                            result = "Select country from the list";
                        break;
                }
                return result;
            }
        }
    }
}
