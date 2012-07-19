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
    public partial class MainPage : PhoneApplicationPage
    {
        CreateCheckpointViewModel createCheckpointViewModel;

        public MainPage()
        {
            InitializeComponent();
            createCheckpointViewModel = new CreateCheckpointViewModel();
            DataContext = createCheckpointViewModel;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            var bindingExpr = textBox.GetBindingExpression(TextBox.TextProperty);
            bindingExpr.UpdateSource();
        }
    }
}