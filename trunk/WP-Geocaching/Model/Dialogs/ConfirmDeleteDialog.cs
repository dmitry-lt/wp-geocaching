using System;
using System.Collections.Generic;
using System.Linq;
using WP_Geocaching.Model.DataBase;
using WP_Geocaching.Resources.Localization;

namespace WP_Geocaching.Model.Dialogs
{
    public class ConfirmDeleteDialog : AbstractDialog
    {
        private Action closeAction;
        private Cache cache;

        public ConfirmDeleteDialog(Cache cache, Action closeAction)
        {
            this.cache = cache;
            this.closeAction = closeAction;
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
            CommandDistionary = new Dictionary<string, ButtonCommand>
                                    {
                                        {AppResources.Delete, new ButtonCommand(Delete)},
                                        {AppResources.Cancel, new ButtonCommand(Cancel)}
                                    };
        }

        private void Delete(object p)
        {
            var db = new CacheDataBase();
            db.DeleteCache(cache.Id);
            closeAction();
        }

        private void Cancel(object p)
        {
        }
    }
}
