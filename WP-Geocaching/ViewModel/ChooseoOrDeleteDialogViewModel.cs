using System;
using System.Windows.Input;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Model;

namespace WP_Geocaching.ViewModel
{
    public class ChooseoOrDeleteDialogViewModel : BaseViewModel
    {
        private int cacheId;
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
                return deleteCommand;
            }
        }
        public ICommand ChooseCommand
        {
            get
            {
                return chooseCommand;
            }
        }

        public int Subtype
        {
            get
            {
                return subtype;
            }
            set
            {
                subtype = value;
            }
        }
        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
            }
        }
        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
            }
        }
        public bool IsDeleteEnabled
        {
            get { return isDeleteEnabled; }
            set
            {
                var changed = isDeleteEnabled != value;
                if (!changed)
                {
                    return;
                }
                isDeleteEnabled = value;
                NotifyPropertyChanged("IsDeleteEnabled");
            }
        }

        public ChooseoOrDeleteDialogViewModel(int cacheId, ListCacheItem item, Action closeDialog)
        {
            this.cacheId = cacheId;
            Subtype = item.Subtype;
            Latitude = item.Latitude;
            Longitude = item.Longitude;
            id = item.Id;
            type = item.Type;
            deleteCommand = new ButtonCommand(DeletefromBd);
            chooseCommand = new ButtonCommand(MakeActive);
            this.closeDialog = closeDialog;
            IsDeleteEnabled = type == (int)Cache.Types.Checkpoint;
        }

        public void DeletefromBd(object p)
        {
            var db = new CacheDataBase();
            db.DeleteCheckpoint(cacheId, id);
            closeDialog();
        }

        public void MakeActive(object p)
        {
            var db = new CacheDataBase();
            if (type != (int)Cache.Types.Checkpoint)
            {
                db.MakeCacheActive(id);
            }
            else
            {
                db.MakeCheckpointActive(cacheId, id);
            }
            closeDialog();
        }
    }
}
