using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yodo1ServiceClient;
using Yodo1ServiceClient.OnlineConfig;
namespace Yodo1ServiceModels.DataSource.OnlineConfig
{
    class OnlineConfigDataSource : IYodo1DataSource
    {
         public async Task<IEnumerable<ConfigBody>> WebGetAsync(int article,string game_appkey,string version,string channel)
         {
            List<ConfigBody> result = new List<ConfigBody>();
            Dictionary<string, string> requestParams = new Dictionary<string, string>();
            requestParams.Add(ConstDefine.PARAM_NAME_GAMEAPPKEY, game_appkey);
            requestParams.Add(ConstDefine.PARAM_NAME_CHANNEL, channel);
            requestParams.Add(ConstDefine.PARAM_NAME_VERSION, version);
            requestParams.Add(ConstDefine.PARAM_NAME_SIGN, Utils.MD5(game_appkey + version + channel + ConstDefine.ONLINECONFIG_SIGN_CONST));
            OnlineConfigResponse response =  await Yodo1ServiceClient.Yodo1ServiceRequest.OnlineConfigService.MakeCall((int)Services.WEBGET,requestParams) as OnlineConfigResponse;
            if (response.GetWebStatus() == System.Net.HttpStatusCode.OK)
            {
                Dictionary<string, object> resultDic = (Dictionary<string, object>)response.GetCustomResult();
                if (null != resultDic)
                {
                    try
                    {
                        if (resultDic["error_code"].ToString().Equals("0"))
                        {
                            List<Dictionary<string, object>> listResult = (List<Dictionary<string, object>>)resultDic["data"];
                            foreach (Dictionary<string, object> index in listResult)
                            {
                                result.Add(DataTransfer.DicToConfigBody(index));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        //todo     
                    }
                }
            }
            else
            {
                //todo
            }
            return result;
        }
   
    }
}
