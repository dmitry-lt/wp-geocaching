using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using WP_Geocaching.Model.Api.GeocachingSu;
using WP_Geocaching.Model.Converters;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Resources.Localization;
using WP_Geocaching.ViewModel;

namespace WP_Geocaching.Model.Dialogs
{
    public class ChooseOrDeleteDialog : AbstractDialog
    {
        private const string Message = "{0} {1}\n{2} {3}\n{4} {5}";

        private ListCacheItem item;
        private readonly string cacheId;
        private readonly Action closeAction;
        private Dispatcher dispatcher;

        public ChooseOrDeleteDialog(string cacheId, Action closeAction, Dispatcher dispatcher)
        {
            this.cacheId = cacheId;
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
            ShowDialog(item.Name, GetResultMessage(), GetResultButtons());
        }

        protected override string GetResultMessage()
        {
            var resultMessage = String.Format(Message,
                                              AppResources.Subtype,
                                              (new CacheSubtypeConverter()).Convert(item.Subtype, null, null, null),
                                              AppResources.Latitude,
                                              (new LatitudeConverter()).Convert(item.Latitude, null, null, null),
                                              AppResources.Longitude,
                                              (new LongitudeConverter()).Convert(item.Longitude, null, null, null));
            return resultMessage;
        }

        protected override List<string> GetResultButtons()
        {
            return CommandDistionary.Keys.Where(p => item.Type == (int)GeocachingSuCache.Types.Checkpoint ||
                p != AppResources.Delete).ToList();
        }


        protected override void FillDictionary()
        {
            CommandDistionary = new Dictionary<string, Action>
                                    {
                                        {AppResources.Delete, DeleteFromBd},
                                        {AppResources.Choose, MakeActive}
                                    };
        }

        private void DeleteFromBd()
        {
            dispatcher.BeginInvoke(() =>
                                         {
                                             var db = new CacheDataBase();
                                             db.DeleteCheckpoint(cacheId, item.Id);
                                             closeAction();
                                         });
        }

        private void MakeActive()
        {
            dispatcher.BeginInvoke(() =>
                                        {
                                            var db = new CacheDataBase();
                                            if (item.Type != (int)GeocachingSuCache.Types.Checkpoint)
                                            {
                                                db.MakeCacheActive(item.Id);
                                            }
                                            else
                                            {
                                                db.MakeCheckpointActive(cacheId, item.Id);
                                            }
                                            closeAction();
                                        });
        }
    }
}
