using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yodo1ServiceModels
{
    public class ConstDefine
    {
        public static readonly string PARAM_NAME_GAMEAPPKEY     = "game_appkey";
        public static readonly string PARAM_NAME_CHANNEL            = "channel";
        public static readonly string PARAM_NAME_VERSION              = "version";
        public static readonly string PARAM_NAME_SIGN                     = "sign";

        public static readonly string ONLINECONFIG_SIGN_CONST     = "yodo1";
    }
    public static class ExtDefine
    {
        public static Object GetOrDefault(this Dictionary<string, Object> source, string key,object defaultValue)
        {
            if (source.ContainsKey(key))
                return source[key];
            else
                return defaultValue;
        }
    }
}
 