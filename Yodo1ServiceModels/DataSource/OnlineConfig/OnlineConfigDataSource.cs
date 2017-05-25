using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yodo1ServiceClient;
using Yodo1ServiceClient.OnlineConfig;
namespace Yodo1ServiceModels.DataSource.OnlineConfig
{
    class OnlineConfigDataSource
    {
         
         public async Task<IEnumerable<ConfigBody>> WebGetAsync(int article,string game_appkey,string version,string channel)
         {
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add(ConstDefine.PARAM_NAME_GAMEAPPKEY, game_appkey);
            OnlineConfigResponse response =  await Yodo1ServiceClient.Yodo1ServiceRequest.OnlineConfigService.MakeCall((int)Services.WEBGET,requestParams) as OnlineConfigResponse;
            if(response.GetWebStatus() == System.Net.HttpStatusCode.OK)
            {
                Dictionary<string, object> resultDic = (Dictionary<string,object>)response.GetCustomResult();
                if (null != resultDic)
                {

                }
            }
         }
         

        public async Task<Customer> GetAsync(Guid id) =>
            await ApiHelper.GetAsync<Customer>($"customer/{id}");

        public async Task<IEnumerable<Customer>> GetAsync(string search) =>
            await ApiHelper.GetAsync<IEnumerable<Customer>>($"customer/search?value={search}");

        public async Task<Customer> PostAsync(Customer customer) =>
            await ApiHelper.PostAsync<Customer, Customer>("customer", customer);

        public async Task DeleteAsync(Guid customerId) =>
            await ApiHelper.DeleteAsync<HttpResponseMessage>("customer", customerId);
    }
}
