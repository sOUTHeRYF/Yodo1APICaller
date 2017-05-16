using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
namespace Yodo1ServiceClient
{
    public abstract class IServiceRequest
    {
        protected Dictionary<ConfigureArticle, ServiceConfigureContent> serviceContent;
        protected HttpClient innerClient = new HttpClient();
        public void SetServiceConfigure(Dictionary<ConfigureArticle,ServiceConfigureContent> content)
        {
            if (null != content)
            {
                this.serviceContent = content;
            }
            else
                this.serviceContent = new Dictionary<ConfigureArticle, ServiceConfigureContent>();
        }
        public bool IfConfigureReady()
        {
            return serviceContent?.Count > 0;
        }
        public async Task<T> PostData<T>(String url,Dictionary<string,string> param) where T:IServiceResponse,new()
        {
            T responseObj = new T();
            HttpContent content = new HttpContent();

            HttpResponseMessage response = await innerClient.PostAsync(url, null);
            string resultStr = await response.Content.ReadAsStringAsync();
            responseObj.SetWebStatus(response.StatusCode);
            responseObj.SetRes(resultStr);
            return responseObj;
        }
        public void Test<T>(T a) where T: IServiceRequest
        {

        }
       abstract public IServiceResponse MakeCall(int api,Dictionary<string,string> param);
       
    }
}
