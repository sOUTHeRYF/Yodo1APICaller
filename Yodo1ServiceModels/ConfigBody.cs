using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yodo1ServiceModels
{
    public class ConfigBody : IEquatable<ConfigBody>
    {
        public string Key = "";
        public string Value = "";
        public string Type = "";
        public string Des = "";
        public ConfigBody(){}
        public ConfigBody(string Key, string Value, string Type, string Des)
        {
            this.Key = Key;
            this.Value = Value;
            this.Type = Type;
            this.Des = Des;
        }
        public static ConfigBody ParseFromDic(Dictionary<string, object> dic)
        {
            ConfigBody result = new ConfigBody();
            if (null != dic)
            {
                foreach (KeyValuePair<string, object> pair in dic)
                {
                    switch (pair.Key)
                    {
                        case "data_key":result.Key              = pair.Value.ToString();break;
                        case "data_value":result.Value       = pair.Value.ToString();break;
                        case "data_type":result.Type           = pair.Value.ToString();break;
                        case "data_des":result.Des              = pair.Value.ToString();break;
                    }
                }
            }
            return result;
        }
        public bool Equals(ConfigBody other)
        {
            return
                this.Key == other.Key &&
                this.Value == other.Value &&
                this.Type == other.Type &&
                this.Des == other.Des;
        }
    }

}
