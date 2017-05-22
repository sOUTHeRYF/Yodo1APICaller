//  ---------------------------------------------------------------------------------
//  Copyright (c) Yodo1,LTD.  All rights reserved.
//  ---------------------------------------------------------------------------------
using System;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ContosoApp.UserControls
{
    public sealed partial class CopyRightControl : UserControl
    {
       // public ViewModels.AuthenticationViewModel ViewModel { get; set; } = new ViewModels.AuthenticationViewModel();

        public CopyRightControl()
        {
            InitializeComponent();
      //      DataContext = ViewModel;
        }

        private async void  HyperlinkButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
              await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }
    }
}
