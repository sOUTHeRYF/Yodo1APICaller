using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yodo1ServiceClient.OnlineConfig
{
    class OnlineConfigRequest : IServiceRequest
    {
        override public IServiceResponse MakeCall(int api,Dictionary<string,string> param)
        {
            OnlineConfigResponse rep = new OnlineConfigResponse();
            if (IfConfigureReady())
            {
                Services services = new Services();
                try
                {
                    services = (Services)api;
                    switch (services)
                    {
                        case Services.ADD:break;
                        case Services.DEL:break;
                        case Services.MODIFY:break;
                        case Services.INHERIT:break;
                    }
                }
                catch(Exception e)
                {
                    rep.FailureAPINotExist();
                }
            }
            else
                rep.FailureNoConfigure();
            return rep;
        }
    }
}
