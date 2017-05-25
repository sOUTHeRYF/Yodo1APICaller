using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
namespace Yodo1ServiceClient
{
    public  class IServiceResponse
    {
        private string innerResStr = "";
        private HttpStatusCode innerWebStatus = HttpStatusCode.OK;
        public void SetRes(string str)
        {
            innerResStr = str;
        }
        public void SetWebStatus(HttpStatusCode status)
        {
            innerWebStatus = status;
        }
        public string GetRes()
        {
            return innerResStr;
        }
        public HttpStatusCode GetWebStatus()
        {
            return innerWebStatus;
        }
        public  void FailureNoConfigure()
        {
            this.innerResStr = "-1000";
        }
        public void FailureAPINotExist()
        {
            this.innerResStr = "-1001";
        }
        public object GetCustomResult()
        {
            return null;
        }
    }
}
