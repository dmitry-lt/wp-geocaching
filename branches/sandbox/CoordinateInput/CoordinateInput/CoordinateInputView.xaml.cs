using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace CoordinateInput
{
    public partial class CoordinateInputView : UserControl
    {
        CheckpointViewModel checkpointViewModel;

        public CoordinateInputView()
        {
            InitializeComponent();
            checkpointViewModel = new CheckpointViewModel();
            DataContext = checkpointViewModel;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            var bindingExpr = textBox.GetBindingExpression(TextBox.TextProperty);
            bindingExpr.UpdateSource();
        }

        private void LoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            checkpointViewModel.Refresh();
        }
    }
}