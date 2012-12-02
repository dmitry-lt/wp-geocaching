using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Microsoft.Xna.Framework.GamerServices;

namespace GeocachingPlus.Model.Dialogs
{
    public abstract class AbstractDialog
    {
        protected Dictionary<string, Action> CommandDistionary;

        protected void ShowDialog(string messageTitle, string message, List<string> buttonKeys)
        {
            if (String.IsNullOrEmpty(messageTitle))
            {
                messageTitle = " ";
            }
            else if (messageTitle.Length > 255)
            {
                messageTitle = messageTitle.Substring(0, 255);
            }
            if (!Guide.IsVisible)
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
        }

        protected abstract void FillDictionary();

        protected abstract string GetResultMessage();

        protected abstract List<string> GetResultButtons();
    }
}
