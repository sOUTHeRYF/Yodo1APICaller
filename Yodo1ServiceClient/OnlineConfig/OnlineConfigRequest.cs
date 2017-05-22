using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
namespace Yodo1ServiceClient.OnlineConfig
{
    class OnlineConfigRequest : IServiceRequest
    {
        override async public Task<IServiceResponse> MakeCall(int api,Dictionary<string,string> param)
        {
            OnlineConfigResponse rep = new OnlineConfigResponse();
            if (IfConfigureReady())
            {
                HttpContent webContent = null;
                string requestUrl = null;
                Services services = new Services();
                try
                {
                    services = (Services)api;
                    switch (services)
                    {
                        case Services.ADD: {
                                requestUrl = serviceContent[currentConfigureArticle];
                            } break;
                        case Services.DEL: {

                            } break;
                        case Services.MODIFY: {

                            } break;
                        case Services.INHERIT: {

                            } break;
                    }
                }
                catch(Exception e)
                {
                    rep.FailureAPINotExist();
                    return rep;
                }
                if (null != requestUrl && null != webContent)
                {
                    rep = await PostData<OnlineConfigResponse>(requestUrl, webContent) ;
                }
            }
            else
                rep.FailureNoConfigure();
            return rep;
        }
    }
}
