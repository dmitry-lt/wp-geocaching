using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.Model.Dialogs
{
    public class ConfirmDeleteDialog : AbstractDialog
    {
        private Action closeAction;
        private Cache cache;
        private Dispatcher dispatcher;


        public ConfirmDeleteDialog(Cache cache, Action closeAction, Dispatcher dispatcher)
        {
            this.cache = cache;
            this.closeAction = closeAction;
            this.dispatcher = dispatcher;
        }

        public void Show()
        {
            if (CommandDistionary == null)
            {
                FillDictionary();
            }

            ShowDialog(cache.Name, GetResultMessage(), GetResultButtons());
        }

        protected override List<string> GetResultButtons()
        {
            return CommandDistionary.Keys.ToList();
        }

        protected override string GetResultMessage()
        {
            return AppResources.DeleteConfirmation;
        }

        protected override void FillDictionary()
        {
            CommandDistionary = new Dictionary<string, Action>
                                    {
                                        {AppResources.Delete, Delete},
                                        {AppResources.Cancel, Cancel}
                                    };
        }

        private void Delete()
        {
            dispatcher.BeginInvoke(() =>
                                        {
                                            var db = new CacheDataBase();
                                            db.DeleteCache(cache);
                                            closeAction();
                                        });
        }

        private void Cancel()
        {
        }
    }
}
