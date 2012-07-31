using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Microsoft.Xna.Framework.GamerServices;

namespace WP_Geocaching.Model.Dialogs
{
    public abstract class AbstractDialog
    {
        private DispatcherTimer timer;
        private ButtonCommand command;

        protected Dictionary<string, ButtonCommand> CommandDistionary;

        protected void ShowDialog(string messageTitle, string message, List<string> buttonKeys)
        {
            command = null;

            timer = new DispatcherTimer
                        {
                            Interval = TimeSpan.FromMilliseconds(10)
                        };
            timer.Tick += TimerTick;
            timer.Start();

            Guide.BeginShowMessageBox(messageTitle, message, buttonKeys, 0, MessageBoxIcon.Error,
                asyncResult =>
                    {
                        var result = Guide.EndShowMessageBox(asyncResult);
                        if (!result.HasValue)
                        {
                            return;
                        }

                        CommandDistionary.TryGetValue(buttonKeys[result.Value], out command);
                        
                    }, null);     
        }

        protected abstract void FillDictionary();

        protected abstract string GetResultMessage();

        protected abstract List<string> GetResultButtons();

        private void TimerTick(object sender, EventArgs e)
        {
            if (command == null)
            {
                return;
            }

            command.Execute(null);
            timer.Stop();
        }
    }
}
