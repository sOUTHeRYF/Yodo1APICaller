using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yodo1ServiceClient
{
    public enum PostDataFormat
    {

    }
    public enum RequestType
    {
        GET              = 0,
        POST            = 1
    }
    public enum ProtocalType
    {
        HTTP            = 0,
        HTTPS          = 1
    }
    public enum ConfigureArticle
    {
        PROD           = 0,
        TEST             = 1,
        DEV              = 2,
        LOCAL          = 3
    }
    public struct ServiceConfigureContent
    {
        public string Domain;
        public Dictionary<int, APIContent> APIContents;
        public ServiceConfigureContent(string domain, Dictionary<int, APIContent> apis)
        {
            this.Domain = domain;
            if (null != apis)
                this.APIContents = apis;
            else
                this.APIContents = new Dictionary<int, APIContent>();
        }
        public string GetFullUri(int func)
        {
            StringBuilder result = new StringBuilder("");
            if (APIContents.ContainsKey(func))
            {
                APIContent currentContent = APIContents[func];
                if (currentContent.ProtType == ProtocalType.HTTP)
                {
                    result.Append("http://");
                }
                else
                {
                    result.Append("https://");
                }
                result.Append(this.Domain + "/");
                result.Append(currentContent.APIPath);
            }
            return result.ToString();
        }
        public bool GetIfPost(int func)
        {
            return APIContents.ContainsKey(func) ? APIContents[func].ReqType == RequestType.POST : false;
        }
    }
    public struct APIContent
    {
        public string APIPath;
        public RequestType ReqType;
        public ProtocalType ProtType;
        public APIContent(string apipath, RequestType type, ProtocalType ptype = ProtocalType.HTTP)
        {
            this.APIPath = apipath;
            this.ReqType = type;
            this.ProtType = ptype;
        }

    }

    public static class ClassFuncInjecter
    {
        public static  int ToInteger(this OnlineConfig.Services enumer)
        {
            return (int)enumer;
        }
        public static void FromInteger(this OnlineConfig.Services enumer,int num)
        {
            enumer = (OnlineConfig.Services)num;
        }
    }
}
