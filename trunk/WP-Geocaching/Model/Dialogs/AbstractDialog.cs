using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Microsoft.Xna.Framework.GamerServices;

namespace WP_Geocaching.Model.Dialogs
{
    public abstract class AbstractDialog
    {
        protected Dictionary<string, Action> CommandDistionary;

        protected void ShowDialog(string messageTitle, string message, List<string> buttonKeys)
        {
            Guide.BeginShowMessageBox(messageTitle, message, buttonKeys, 0, MessageBoxIcon.Error,
                asyncResult =>
                    {
                        var result = Guide.EndShowMessageBox(asyncResult);
                        if (!result.HasValue)
                        {
                            return;
                        }

                        Action action;

                        CommandDistionary.TryGetValue(buttonKeys[result.Value], out action);

                        action.Invoke();

                    }, null);     
        }

        protected abstract void FillDictionary();

        protected abstract string GetResultMessage();

        protected abstract List<string> GetResultButtons();
    }
}
