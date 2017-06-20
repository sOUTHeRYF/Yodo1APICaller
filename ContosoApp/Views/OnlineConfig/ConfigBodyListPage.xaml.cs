using Yodo1APICaller.ViewModels;
using PropertyChanged;
using System.Linq;

using System.Collections.Generic;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using System;


// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Yodo1APICaller.Views.OnlineConfig
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    [ImplementPropertyChanged]
    public sealed partial class ConfigBodyListPage : Page
    {
        public ConfigBodyListPageViewModel ViewModel { get; set; } = new ConfigBodyListPageViewModel();
        public ConfigBodyListPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            if (ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Controls.CommandBar", "DefaultLabelPosition"))
            {
                Window.Current.SizeChanged += CurrentWindow_SizeChanged;
            }
        }

        private void CurrentWindow_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile" && e.Size.Width >=
                (double)App.Current.Resources["MediumWindowSnapPoint"])
            {
                mainCommandBar.DefaultLabelPosition = CommandBarDefaultLabelPosition.Right;
            }
            else
            {
                mainCommandBar.DefaultLabelPosition = CommandBarDefaultLabelPosition.Bottom;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.GameSelectBox.SetComboxContent(SettingManager.GetSetting(SettingArticle.GAME_LIST));
            this.ChannelSelectBox.SetComboxContent(SettingManager.GetSetting(SettingArticle.CHANNEL_LIST));
            this.VersionSelectBox.SetComboxContent(SettingManager.GetSetting(SettingArticle.VERSION));
        }
        /// <summary>
        /// Navigates to a blank customer details page for the user to fill in.
        /// </summary>
        private void CreateCustomer_Click(object sender, RoutedEventArgs e) =>
            GoToDetailsPage(null);

        private void CustomerSearchBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (CustomerSearchBox != null)
            {
                CustomerSearchBox.AutoSuggestBox.QuerySubmitted += CustomerSearchBox_QuerySubmitted;
                CustomerSearchBox.AutoSuggestBox.TextChanged += CustomerSearchBox_TextChanged;
                CustomerSearchBox.AutoSuggestBox.PlaceholderText = "Search customers...";
            }
        }

        private async void CustomerSearchBox_TextChanged(AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            // We only want to get results when it was a user typing,
            // otherwise we assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                // If no search query is entered, refresh the complete list.
                if (String.IsNullOrEmpty(sender.Text))
                {
                    await Utilities.CallOnUiThreadAsync(async () =>
                        await ViewModel.GetCustomerListAsync());
                    sender.ItemsSource = null;
                }
                else
                {
                    string[] parameters = sender.Text.Split(new char[] { ' ' },
                        StringSplitOptions.RemoveEmptyEntries);
                    sender.ItemsSource = ViewModel.Customers
                        .Where(x => parameters.Any(y =>
                            x.Address.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                            x.FirstName.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                            x.LastName.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                            x.Company.StartsWith(y, StringComparison.OrdinalIgnoreCase)))
                        .OrderByDescending(x => parameters.Count(y =>
                            x.Address.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                            x.FirstName.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                            x.LastName.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                            x.Company.StartsWith(y, StringComparison.OrdinalIgnoreCase)))
                        .Select(x => $"{x.FirstName} {x.LastName}");
                }
            }
        }

        private async void CustomerSearchBox_QuerySubmitted(AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (String.IsNullOrEmpty(args.QueryText))
            {
                await Utilities.CallOnUiThreadAsync(async () =>
                    await ViewModel.GetCustomerListAsync());
            }
            else
            {
                string[] parameters = sender.Text.Split(new char[] { ' ' },
                    StringSplitOptions.RemoveEmptyEntries);

                var matches = ViewModel.Customers.Where(x => parameters
                    .Any(y =>
                        x.Address.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                        x.FirstName.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                        x.LastName.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                        x.Company.StartsWith(y, StringComparison.OrdinalIgnoreCase)))
                    .OrderByDescending(x => parameters.Count(y =>
                        x.Address.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                        x.FirstName.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                        x.LastName.StartsWith(y, StringComparison.OrdinalIgnoreCase) ||
                        x.Company.StartsWith(y, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                await Utilities.CallOnUiThreadAsync(() =>
                {
                    ViewModel.Customers.Clear();
                    foreach (var match in matches)
                    {
                        ViewModel.Customers.Add(match);
                    }
                });
            }
        }

        /// <summary>
        /// Workaround to support earlier versions of Windows.
        /// </summary>
        private void CommandBar_Loaded(object sender, RoutedEventArgs e)
        {
            if (ApiInformation.IsPropertyPresent("Windows.UI.Xaml.Controls.CommandBar", "DefaultLabelPosition"))
            {
                if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    (sender as CommandBar).DefaultLabelPosition = CommandBarDefaultLabelPosition.Bottom;
                }
                else
                {
                    (sender as CommandBar).DefaultLabelPosition = CommandBarDefaultLabelPosition.Right;
                }
            }
            else
            {
                if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    var lastCommand = mainCommandBar.PrimaryCommands.Last();

                    mainCommandBar.PrimaryCommands.Remove(lastCommand);
                    mainCommandBar.SecondaryCommands.Add(lastCommand);
                }
            }
        }

        /// <summary>
        /// Menu flyout click control for selecting a customer and displaying details.
        /// </summary>
        private void ViewDetails_Click(object sender, RoutedEventArgs e) =>
            GoToDetailsPage(ViewModel.SelectedCustomer);

        /// <summary>
        /// Opens the order detail page for the user to create an order for the selected customer.
        /// </summary>
        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem)
            {
                GoToOrderPage((sender as FrameworkElement).DataContext as CustomerViewModel);
            }
            else
            {
                GoToOrderPage(ViewModel.SelectedCustomer);
            }
        }

        /// <summary>
        /// Navigates to the customer detail page for the provided customer.
        /// </summary>
        private void GoToDetailsPage(CustomerViewModel customer) =>
            Frame.Navigate(typeof(CustomerDetailPage), customer,
                new DrillInNavigationTransitionInfo());

        /// <summary>
        /// Navigates to the order detail page for the provided customer.
        /// </summary>
        private void GoToOrderPage(CustomerViewModel customer) =>
            Frame.Navigate(typeof(OrderDetailPage), customer.Model);

    }
}
