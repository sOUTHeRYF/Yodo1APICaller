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
        string Domain;
        Dictionary<int, APIContent> APIContents;
        public ServiceConfigureContent(string domain, Dictionary<int, APIContent> apis)
        {
            this.Domain = domain;
            if (null != apis)
                this.APIContents = apis;
            else
                this.APIContents = new Dictionary<int, APIContent>();
        }
    }
    public struct APIContent
    {
        string APIPath;
        RequestType ReqType;
        ProtocalType ProtType;
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
