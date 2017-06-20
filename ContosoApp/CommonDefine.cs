using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yodo1APICaller
{
    public struct CustomContentPair<T>
    {
        private string customStr;
        private T content;
        public CustomContentPair(string customStr, T content)
        {
            this.customStr = customStr;
            this.content = content;
        }
        public override string ToString()
        {
            return customStr;
        }
        public T GetContent()
        {
            return content;
        }
    }
    public static class ExtDefine
    {
        public static Object GetOrDefault(this Dictionary<string, Object> source, string key, object defaultValue)
        {
            if (source.ContainsKey(key))
                return source[key];
            else
                return defaultValue;
        }
        public static Dictionary<string, string> AddTo(this Dictionary<string, string> source, string key, string value)
        {
            source.Add(key, value);
            return source;
        }
        public static List<string> AddTo(this List<string> source, string value)
        {
            source.Add(value);
            return source;
        }
        public static List<Dictionary<string,string>> AddTo(this List<Dictionary<string, string>> source, Dictionary<string, string> value)
        {
            source.Add(value);
            return source;
        }
    }
}
