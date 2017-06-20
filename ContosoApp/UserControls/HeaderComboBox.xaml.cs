using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Yodo1APICaller;
namespace Yodo1APICaller.UserControls
{
    public sealed partial class HeaderComboBox : UserControl
    {
        public HeaderComboBox()
        {
            InitializeComponent();
            Window.Current.SizeChanged += Current_SizeChanged;
            Loaded += (s, a) =>
            {
                AppShell.Current.TogglePaneButtonRectChanged += Current_TogglePaneButtonSizeChanged;
                titleBar.Margin = new Thickness(AppShell.Current.TogglePaneButtonRect.Right, 0, 0, 0);
            };
        }
        public string LeftText { get; set; }
        public Action<string> onComboxSelectionChanged;
        public void SetComboxContent(List<CustomContentPair<string>> contents)
        {
            if (null != contents)
            {
                comboBox.DataContext = contents;
            }
        }
        /// <summary>
        /// Changes the title bar margin thickness.
        /// </summary>
        private void Current_TogglePaneButtonSizeChanged(AppShell sender, Rect e)
        {
            titleBar.Margin = new Thickness(e.Right, 0, 0, 0);
        }
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (e.Size.Width <= 1200)
            {
                this.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.Visibility = Visibility.Visible;
            }
        }
        private void comboBox_DropDownClosed(object sender, object e)
        {
            if (null != onComboxSelectionChanged)
            {
                CustomContentPair<string> item = (CustomContentPair<string>)comboBox.SelectedItem;
                onComboxSelectionChanged.Invoke(item.GetContent());
            }
        }
    }
}
