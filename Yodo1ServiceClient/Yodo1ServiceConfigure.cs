using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yodo1ServiceClient.OnlineConfig;
namespace Yodo1ServiceClient
{
    public class Yodo1ServiceConfigure
    {
        private static string olconfigDevDomain = "192.168.1.132/config";
        private static string olconfigLocalDomain = "localhost:8081/config";
        /// <summary>
        /// Load Configure;
        /// Todo:load from xml
        /// </summary>
        public static void InitConfigure()
        {
            Dictionary<int, APIContent> olconfigContent = new Dictionary<int, APIContent>();
            olconfigContent.Add(OnlineConfig.Services.ADD.ToInteger(), new APIContent("/add", RequestType.POST));
            olconfigContent.Add(OnlineConfig.Services.DEL.ToInteger(), new APIContent("/del", RequestType.POST));
            olconfigContent.Add(OnlineConfig.Services.MODIFY.ToInteger(), new APIContent("/modify", RequestType.POST));
            olconfigContent.Add(OnlineConfig.Services.CLIENTGET.ToInteger(), new APIContent("config/getData", RequestType.POST));
            olconfigContent.Add(OnlineConfig.Services.WEBGET.ToInteger(), new APIContent("config/get", RequestType.POST));
            ServiceConfigureContent configDev = new ServiceConfigureContent(olconfigDevDomain,olconfigContent);
            ServiceConfigureContent configLocal = new ServiceConfigureContent(olconfigLocalDomain, olconfigContent);
            Yodo1ServiceRequest.OnlineConfigService.SetServiceConfigure(new Dictionary<ConfigureArticle, ServiceConfigureContent>
            {
                { ConfigureArticle.PROD,configDev},//todo
                { ConfigureArticle.DEV,configDev},
                { ConfigureArticle.TEST,configDev},
                { ConfigureArticle.LOCAL,configLocal}
            });
        }
    }

}
