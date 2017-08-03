//  ---------------------------------------------------------------------------------
//  Copyright (c) Yodo1,LTD.  All rights reserved.
//  ---------------------------------------------------------------------------------

using ContosoModels;
using Yodo1APICaller.Commands;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using System.Collections.Generic;
using Yodo1ServiceModels;
using Yodo1ServiceModels.DataSource;
namespace Yodo1APICaller.ViewModels
{
    [ImplementPropertyChanged]
    /// <summary>
    /// Encapsulates data for the CustomerListPage. The page UI
    /// binds to the properties defined here. 
    /// </summary>
    public class ConfigBodyListPageViewModel : BindableBase
    {
        /// <summary>
        /// Creates a new CustomerListPageViewModel.
        /// </summary>
        public ConfigBodyListPageViewModel()
        {
            Task.Run(GetConfigListAsync);
            SyncCommand = new RelayCommand(OnSync);
        }

        /// <summary>
        /// The collection of customers in the list. 
        /// </summary>
        public ObservableCollection<CustomerViewModel> Customers { get; set; } =
            new ObservableCollection<CustomerViewModel>();
        public ObservableCollection<ConfigBodyViewModel> ConfigBodys { get; set; } =
            new ObservableCollection<ConfigBodyViewModel>();
        private CustomerViewModel _selectedCustomer;
        /// <summary>
        /// Gets or sets the selected customer, or null if no customer is selected. 
        /// </summary>
        public CustomerViewModel SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                SetProperty(ref _selectedCustomer, value);
            }
        }

        private string _errorText = null;
        /// <summary>
        /// Gets or sets the error text.
        /// </summary>
        public string ErrorText
        {
            get { return _errorText; }

            set
            {
                SetProperty(ref _errorText, value);
            }
        }

        private bool _isLoading = false;
        /// <summary>
        /// Gets or sets whether to show the data loading progress wheel. 
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }

            set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        /// <summary>
        /// Gets the complete list of customers from the database.
        /// </summary>
        public async Task GetCustomerListAsync()
        {
            await Utilities.CallOnUiThreadAsync(() => IsLoading = true);

            var db = new ContosoDataSource();
            var customers = await db.Customers.GetAsync();
            if (customers == null)
            {
                return;
            }
            await Utilities.CallOnUiThreadAsync(() =>
            {
                Customers.Clear();
                foreach (var c in customers)
                {
                    Customers.Add(new CustomerViewModel(c) { Validate = true });
                }
                IsLoading = false;
            });
        }
        public async Task GetConfigListAsync()
        {
            await Utilities.CallOnUiThreadAsync(() => IsLoading = true);
            /*
            var db = new Yodo1DataSource();
            db.OnlineConfig.Init();
            db.OnlineConfig.SetArticle(0);
            var configbodys = await db.OnlineConfig.GetAsync();
            */
            List<ConfigBody> configbodys = new List<ConfigBody>();
            configbodys.Add(new ConfigBody("ak","av","at","ad"));
            configbodys.Add(new ConfigBody("bk", "bv", "bt", "bd"));
            if (configbodys == null)
            {
                return;
            }
            await Utilities.CallOnUiThreadAsync(() =>
            {
                ConfigBodys.Clear();
                foreach (var c in configbodys)
                {
                    ConfigBodys.Add(new ConfigBodyViewModel((ConfigBody)c) { Validate = true });
                }
                IsLoading = false;
            });
        }
        public RelayCommand SyncCommand { get; private set; }

        /// <summary>
        /// Queries the database for a current list of customers.
        /// </summary>
        private void OnSync()
        {
            /*todo
            Task.Run(async () =>
            {
                IsLoading = true;
                var db = new Yodo1DataSource();
                foreach (var modifiedCustomer in Customers
                    .Where(x => x.IsModified).Select(x => x.Model))
                {
                    await db.Customers.PostAsync(modifiedCustomer);
                }
                await GetCustomerListAsync();
                IsLoading = false;
            });
            */
            Task.Run(async () =>
            {
                IsLoading = true;
                await GetConfigListAsync();
                IsLoading = false;
            });
        }
    }
}
