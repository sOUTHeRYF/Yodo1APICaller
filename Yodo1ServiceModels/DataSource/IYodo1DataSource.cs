using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yodo1ServiceClient;
namespace Yodo1ServiceModels
{
    public class IYodo1DataSource
    {
        //public async Task<IEnumerable<ConfigBody>> WebGetAsync(int article, string game_appkey, string version, string channel);
        public void Init()
        {
            Yodo1ServiceClient.Yodo1ServiceConfigure.InitConfigure();
        }
        public void SetArticle(int article)
        {
            try
            {
                Yodo1ServiceClient.Yodo1ServiceRequest.OnlineConfigService.SetArticle((ConfigureArticle)Enum.ToObject(typeof(ConfigureArticle), article));
            }
            catch (Exception e)
            { }
        }
    }
}
