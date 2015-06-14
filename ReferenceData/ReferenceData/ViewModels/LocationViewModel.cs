using System;
using System.ComponentModel;

namespace ReferenceData.ViewModels
{
    class LocationViewModel : INotifyPropertyChanged
    {
        private int id;
        private string name;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                NotifyPropertyChanged("Id");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public override string ToString()
        {
            return name;
        }

        public override bool Equals(object obj)
        {
            LocationViewModel item = obj as LocationViewModel;
            if (item == null)
                return base.Equals(obj);
            else
                return String.Equals(this.name, item.name);
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
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
    }
}
