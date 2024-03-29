﻿using System;
using System.Windows.Input;

namespace GeocachingPlus.Model
{
    public class ButtonCommand : ICommand
    {
        private Action<object> executeAction;

        public event EventHandler CanExecuteChanged;

        public ButtonCommand(Action<object> executeAction)
        {
            this.executeAction = executeAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
    }
}
