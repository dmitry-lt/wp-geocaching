using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using GeocachingPlus.Model.Api.GeocachingSu;
using GeocachingPlus.Model.Navigation;
using GeocachingPlus.View.Converters;
using GeocachingPlus.Model.DataBase;
using GeocachingPlus.Resources.Localization;
using GeocachingPlus.ViewModel;

namespace GeocachingPlus.Model.Dialogs
{
    public class ChooseOrDeleteDialog : AbstractDialog
    {
        private const string Message = "{0} {1}\n{2} {3}\n{4} {5}";

        private ListCacheItem item;
        private readonly Cache cache;
        private readonly Action closeAction;
        private Dispatcher dispatcher;

        public ChooseOrDeleteDialog(Cache cache, Action closeAction, Dispatcher dispatcher)
        {
            this.cache = cache;
            this.closeAction = closeAction;
            this.dispatcher = dispatcher;
        }

        public void Show(ListCacheItem selectedItem)
        {
            if (CommandDistionary == null)
            {
                FillDictionary();
            }

            item = selectedItem;
            ShowDialog(item.Cache.Name, GetResultMessage(), GetResultButtons());
        }

        protected override string GetResultMessage()
        {
            var resultMessage = String.Format(Message,
                                              AppResources.Subtype,
                                              (new CacheSubtypeConverter()).Convert(item.Subtype, null, null, null),
                                              AppResources.Latitude,
                                              (new LatitudeConverter()).Convert(item.Cache.Location.Latitude, null, null, null),
                                              AppResources.Longitude,
                                              (new LongitudeConverter()).Convert(item.Cache.Location.Longitude, null, null, null));
            return resultMessage;
        }

        protected override List<string> GetResultButtons()
        {
            // TODO: refactor
            return CommandDistionary.Keys.Where(
                p => (
                (item.Cache is GeocachingSuCache) && ((GeocachingSuCache)item.Cache).Type == GeocachingSuCache.Types.Checkpoint) 
                || p != AppResources.Edit).ToList();
        }


        protected override void FillDictionary()
        {
            CommandDistionary = new Dictionary<string, Action>
                                    {
                                        {AppResources.Choose, MakeActive},
                                        {AppResources.Edit, Edit},
                                    };
        }

        private void Edit()
        {
            dispatcher.BeginInvoke(() => NavigationManager.Instance.NavigateToEditCheckpoint(item.Cache.Id));
        }

        private void MakeActive()
        {
            // TODO: refactor
            dispatcher.BeginInvoke(() =>
            {
                var db = new CacheDataBase();
                if ((item.Cache is GeocachingSuCache) && ((GeocachingSuCache)item.Cache).Type == GeocachingSuCache.Types.Checkpoint)
                {
                    db.MakeCheckpointActive(cache, item.Cache.Id);
                }
                else
                {
                    db.MakeCacheActive(cache);
                }
                closeAction();
            });
        }
    }
}
