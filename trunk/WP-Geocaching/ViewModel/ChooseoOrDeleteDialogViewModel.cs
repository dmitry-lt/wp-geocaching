using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;
using System.ComponentModel;

namespace WP_Geocaching.ViewModel
{
    public class ChooseoOrDeleteDialogViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int id;
        private int subtype;
        private int type;
        private bool isDeleteEnabled;
        private double latitude;
        private double longitude;
        private ICommand deleteCommand;
        private ICommand chooseCommand;
        private Action closeDialog;

        public ICommand DeleteCommand
        {
            get
            {
                return this.deleteCommand;
            }
        }
        public ICommand ChooseCommand
        {
            get
            {
                return this.chooseCommand;
            }
        }

        public int Subtype
        {
            get
            {
                return this.subtype;
            }
            set
            {
                this.subtype = value;
            }
        }
        public double Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                this.latitude = value;
            }
        }
        public double Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                this.longitude = value;
            }
        }
        public bool IsDeleteEnabled
        {
            get { return isDeleteEnabled; }
            set
            {
                bool changed = isDeleteEnabled != value;
                if (changed)
                {
                    isDeleteEnabled = value;
                    OnPropertyChanged("IsDeleteEnabled");
                }
            }
        }

        public ChooseoOrDeleteDialogViewModel(ListCacheItem item, Action closeDialog)
        {
            Subtype = item.Subtype;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
            id = item.Id;
            type = item.Type;
            deleteCommand = new ButtonCommand(DeletefromBd);
            chooseCommand = new ButtonCommand(MakeActive);
            this.closeDialog = closeDialog;
            if (type != (int)Cache.Types.Checkpoint)
            {
                IsDeleteEnabled = false;
            }
            else
            {
                IsDeleteEnabled = true;
            }
        }

        public void DeletefromBd(object p)
        {
            CacheDataBase db = new CacheDataBase();
            db.DeleteCheckpoint(id);
            closeDialog();
        }

        public void MakeActive(object p)
        {
            CacheDataBase db = new CacheDataBase();
            if (type != (int)Cache.Types.Checkpoint)
            {
                db.MakeCacheActive(id);
            }
            else
            {                
                db.MakeCheckpointActive(id);               
            }
            closeDialog();
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
