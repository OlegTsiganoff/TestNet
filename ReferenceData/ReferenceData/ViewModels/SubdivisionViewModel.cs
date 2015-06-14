using System;
using System.ComponentModel;

namespace ReferenceData.ViewModels
{
    class SubdivisionViewModel: INotifyPropertyChanged
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
            SubdivisionViewModel item = obj as SubdivisionViewModel;
            if (item == null)
                return base.Equals(obj);
            else
                return (String.Equals(this.name, item.name));
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
