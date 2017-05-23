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
                bool ifPost = false;
                Services services = new Services();
                /*
                try
                {
                    services = (Services)api;
                    switch (services)
                    {
                        case Services.ADD: {

                            } break;
                        case Services.DEL: {

                            } break;
                        case Services.MODIFY: {

                            } break;
                        case Services.INHERIT: {

                            } break;
                        case Services.WEBGET: {
                            }break;
                        case Services.CLIENTGET: {
                            }break;
                    }
                }
                catch(Exception e)
                {
                    rep.FailureAPINotExist();
                    return rep;
                }*/
                requestUrl = serviceContent[currentConfigureArticle].GetFullUri(api);
                ifPost = serviceContent[currentConfigureArticle].GetIfPost(api);
                Yodo1RequestJsonContent jsonContent = new Yodo1RequestJsonContent(param, Encoding.UTF8);
                webContent = jsonContent.getContent();
                if (null != requestUrl && null != webContent)
                {
                    if(ifPost)
                        rep = await PostData<OnlineConfigResponse>(requestUrl, webContent) ;
                    else
                        rep = await PostData<OnlineConfigResponse>(requestUrl, webContent);
                }
            }
            else
                rep.FailureNoConfigure();
            return rep;
        }
    }
}
