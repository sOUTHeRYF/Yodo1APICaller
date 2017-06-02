using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yodo1ServiceModels.DataSource.OnlineConfig
{
    internal class DataTransfer
    {
        public static ConfigBody DicToConfigBody(Dictionary<string, object> source)
        {
            ConfigBody result = new ConfigBody();
            if (null != source)
            {
                result.Key = (string)source.GetOrDefault("data_key", "error");
                result.Type = (string)source.GetOrDefault("data_type", "error");
                result.Value = (string)source.GetOrDefault("data_value", "Bool");
                result.Des = (string)source.GetOrDefault("data_des", "error");
            }
            return result;
        }
    }
}
