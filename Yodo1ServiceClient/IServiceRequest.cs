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
        protected ConfigureArticle currentConfigureArticle = ConfigureArticle.PROD;
        public void SetServiceConfigure(Dictionary<ConfigureArticle,ServiceConfigureContent> content)
        {
            if (null != content)
            {
                this.serviceContent = content;
            }
            else
                this.serviceContent = new Dictionary<ConfigureArticle, ServiceConfigureContent>();
        }
        public void SetArticle(ConfigureArticle article) { this.currentConfigureArticle = article; }
        protected bool IfConfigureReady()
        {
            return serviceContent?.Count > 0;
        }
        protected async Task<T> PostData<T>(String url,HttpContent content) where T:IServiceResponse,new()
        {
            T responseObj = new T();

            HttpResponseMessage response = await innerClient.PostAsync(url, content);
            string resultStr = await response.Content.ReadAsStringAsync();
            responseObj.SetWebStatus(response.StatusCode);
            responseObj.SetRes(resultStr);
            return responseObj;
        }
       abstract  public Task<IServiceResponse> MakeCall(int api,Dictionary<string,string> param);
       
    }
}
