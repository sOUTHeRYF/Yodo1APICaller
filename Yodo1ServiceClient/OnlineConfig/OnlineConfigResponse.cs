using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yodo1ServiceClient.OnlineConfig
{
    public class OnlineConfigResponse : IServiceResponse
    {
        new public object GetCustomResult()
        {
            if (!string.IsNullOrWhiteSpace(this.GetRes()))
            {
                Dictionary<string, object> result = (Dictionary<string, object>)JSONHelper.Deserialize(this.GetRes());
                return result;
            }
            return 0;
        }
    }
}
