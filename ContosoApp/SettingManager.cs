using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
namespace Yodo1APICaller
{
    public enum SettingArticle
    {
        GAME_LIST           = 0,
        CHANNEL_LIST     = 1,
        VERSION
    }
    public class SettingManager
    {
        private static readonly string SAVE_FILE_NAME = "setting.json";
        private static readonly string KEY_CONTENT = "Content";
        private static readonly string KEY_CUSTOM = "Custom";
        private static readonly SettingManager _instance = new SettingManager();

        private StorageFile storageFile;
        //  private Dictionary<string, List<Dictionary<string,string>>> innerDic = new Dictionary<string, List<Dictionary<string, string>>>();
        private Dictionary<string, object> innerDic = new Dictionary<string,object>();
        public async static void Init()
        {
            _instance.storageFile = await Windows.Storage.ApplicationData.Current.RoamingFolder.CreateFileAsync(SAVE_FILE_NAME,CreationCollisionOption.OpenIfExists);
            string result = await FileIO.ReadTextAsync(_instance.storageFile);
            try
            {
                if (!string.IsNullOrWhiteSpace(result))
                {
                    Dictionary<string, object> tempValue = (Dictionary<string, object>)JSONHelper.Deserialize(result);
                    _instance.innerDic = tempValue;
                } 
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            if (null != _instance.innerDic)
            {
                _instance.innerDic = loadDefaultSettring();
                SaveSetting();
            }
        }
        public static List<CustomContentPair<string>>  GetSetting(SettingArticle article)
        {
            List<CustomContentPair<string>> result = new List<CustomContentPair<string>>();
            try
            {
                if (null != _instance.innerDic && _instance.innerDic.ContainsKey(article.ToString()))
                {
                    List<Dictionary<string,string>> tempList = (List<Dictionary<string, string>>)_instance.innerDic[article.ToString()];
                    if (null != tempList && tempList.Count > 0)
                    {
                        foreach (Dictionary<string, string> tempDic in tempList)
                        {
                            result.Add(new CustomContentPair<string>((string)tempDic["Custom"], (string)tempDic["Content"]));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

          
     
            return result;
        }
        public async static void SaveSetting()
        {
            string result = JSONHelper.Serialize(_instance.innerDic);
            await FileIO.WriteTextAsync(_instance.storageFile, result);
        }

        private static Dictionary<string, object> loadDefaultSettring()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result[SettingArticle.GAME_LIST.ToString()] = new List<Dictionary<string, string>>().
                AddTo(new Dictionary<string, string>().AddTo(KEY_CONTENT, "23lqbnB7LY").AddTo(KEY_CUSTOM, "变形金刚-国内安卓")).
                AddTo(new Dictionary<string, string>().AddTo(KEY_CONTENT, "2qzpQ2MGQd").AddTo(KEY_CUSTOM, "疯狂动物园")).
                AddTo(new Dictionary<string, string>().AddTo(KEY_CONTENT,"test").AddTo(KEY_CUSTOM,"Test"));
            result[SettingArticle.CHANNEL_LIST.ToString()] = new List<Dictionary<string, string>>().
                AddTo(new Dictionary<string, string>().AddTo(KEY_CONTENT, "360").AddTo(KEY_CUSTOM, "奇虎360")).
                AddTo(new Dictionary<string, string>().AddTo(KEY_CONTENT, "TXYYB").AddTo(KEY_CUSTOM, "腾讯应用宝")).
                AddTo(new Dictionary<string, string>().AddTo(KEY_CONTENT, "TXWX").AddTo(KEY_CUSTOM, "腾讯微信"));
            result[SettingArticle.VERSION.ToString()] = new List<Dictionary<string, string>>().
                AddTo(new Dictionary<string, string>().AddTo(KEY_CONTENT, "1.1").AddTo(KEY_CUSTOM, "1.1")).
                AddTo(new Dictionary<string, string>().AddTo(KEY_CONTENT, "2.0").AddTo(KEY_CUSTOM, "2.0")).
                AddTo(new Dictionary<string, string>().AddTo(KEY_CONTENT, "3.0").AddTo(KEY_CUSTOM, "3.0"));
            return result;
        }
    }
    static class JSONHelper
    {
        /// <summary>
        /// Parses the string json into a value
        /// </summary>
        /// <param name="json">A JSON string.</param>
        /// <returns>An List<object>, a Dictionary<string, object>, a double, an integer,a string, null, true, or false</returns>
        public static object Deserialize(string json)
        {
            // save the string for debug information
            if (json == null)
            {
                return null;
            }
            return Parser.Parse(json);
        }
        sealed class Parser : IDisposable
        {
            const string WHITE_SPACE = " \t\n\r";
            const string WORD_BREAK = " \t\n\r{}[],:\"";
            enum TOKEN
            {
                NONE,
                CURLY_OPEN,
                CURLY_CLOSE,
                SQUARED_OPEN,
                SQUARED_CLOSE,
                COLON,
                COMMA,
                STRING,
                NUMBER,
                TRUE,
                FALSE,
                NULL
            };
            System.IO.StringReader json;
            Parser(string jsonString)
            {
                json = new System.IO.StringReader(jsonString);
            }
            public static object Parse(string jsonString)
            {
                using (var instance = new Parser(jsonString))
                {
                    return instance.ParseValue();
                }
            }
            public void Dispose()
            {
                json.Dispose();
                json = null;
            }
            Dictionary<string, object> ParseObject()
            {
                Dictionary<string, object> table = new Dictionary<string, object>();
                // ditch opening brace
                json.Read();
                // {
                while (true)
                {
                    switch (NextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.CURLY_CLOSE:
                            return table;
                        default:
                            // name
                            string name = ParseString();
                            if (name == null)
                            {
                                return null;
                            }
                            // :
                            if (NextToken != TOKEN.COLON)
                            {
                                return null;
                            }
                            // ditch the colon
                            json.Read();
                            // value
                            table[name] = ParseValue();
                            break;
                    }
                }
            }
            List<object> ParseArray()
            {
                List<object> array = new List<object>();
                // ditch opening bracket
                json.Read();
                // [
                var parsing = true;
                while (parsing)
                {
                    TOKEN nextToken = NextToken;
                    switch (nextToken)
                    {
                        case TOKEN.NONE:
                            return null;
                        case TOKEN.COMMA:
                            continue;
                        case TOKEN.SQUARED_CLOSE:
                            parsing = false;
                            break;
                        default:
                            object value = ParseByToken(nextToken);
                            array.Add(value);
                            break;
                    }
                }
                return array;
            }
            object ParseValue()
            {
                TOKEN nextToken = NextToken;
                return ParseByToken(nextToken);
            }
            object ParseByToken(TOKEN token)
            {
                switch (token)
                {
                    case TOKEN.STRING:
                        return ParseString();
                    case TOKEN.NUMBER:
                        return ParseNumber();
                    case TOKEN.CURLY_OPEN:
                        return ParseObject();
                    case TOKEN.SQUARED_OPEN:
                        return ParseArray();
                    case TOKEN.TRUE:
                        return true;
                    case TOKEN.FALSE:
                        return false;
                    case TOKEN.NULL:
                        return null;
                    default:
                        return null;
                }
            }
            string ParseString()
            {
                StringBuilder s = new StringBuilder();
                char c;
                // ditch opening quote
                json.Read();
                bool parsing = true;
                while (parsing)
                {
                    if (json.Peek() == -1)
                    {
                        parsing = false;
                        break;
                    }
                    c = NextChar;
                    switch (c)
                    {
                        case '"':
                            parsing = false;
                            break;
                        case '\\':
                            if (json.Peek() == -1)
                            {
                                parsing = false;
                                break;
                            }
                            c = NextChar;
                            switch (c)
                            {
                                case '"':
                                case '\\':
                                case '/':
                                    s.Append(c);
                                    break;
                                case 'b':
                                    s.Append('\b');
                                    break;
                                case 'f':
                                    s.Append('\f');
                                    break;
                                case 'n':
                                    s.Append('\n');
                                    break;
                                case 'r':
                                    s.Append('\r');
                                    break;
                                case 't':
                                    s.Append('\t');
                                    break;
                                case 'u':
                                    var hex = new StringBuilder();
                                    for (int i = 0; i < 4; i++)
                                    {
                                        hex.Append(NextChar);
                                    }
                                    s.Append((char)Convert.ToInt32(hex.ToString(), 16));
                                    break;
                            }
                            break;
                        default:
                            s.Append(c);
                            break;
                    }
                }
                return s.ToString();
            }
            object ParseNumber()
            {
                string number = NextWord;
                if (number.IndexOf('.') == -1)
                {
                    long parsedInt;
                    Int64.TryParse(number, out parsedInt);
                    return parsedInt;
                }
                double parsedDouble;
                Double.TryParse(number, out parsedDouble);
                return parsedDouble;
            }
            void EatWhitespace()
            {
                while (WHITE_SPACE.IndexOf(PeekChar) != -1)
                {
                    json.Read();
                    if (json.Peek() == -1)
                    {
                        break;
                    }
                }
            }
            char PeekChar
            {
                get
                {
                    return Convert.ToChar(json.Peek());
                }
            }
            char NextChar
            {
                get
                {
                    return Convert.ToChar(json.Read());
                }
            }
            string NextWord
            {
                get
                {
                    StringBuilder word = new StringBuilder();
                    while (WORD_BREAK.IndexOf(PeekChar) == -1)
                    {
                        word.Append(NextChar);
                        if (json.Peek() == -1)
                        {
                            break;
                        }
                    }
                    return word.ToString();
                }
            }
            TOKEN NextToken
            {
                get
                {
                    EatWhitespace();
                    if (json.Peek() == -1)
                    {
                        return TOKEN.NONE;
                    }
                    char c = PeekChar;
                    switch (c)
                    {
                        case '{':
                            return TOKEN.CURLY_OPEN;
                        case '}':
                            json.Read();
                            return TOKEN.CURLY_CLOSE;
                        case '[':
                            return TOKEN.SQUARED_OPEN;
                        case ']':
                            json.Read();
                            return TOKEN.SQUARED_CLOSE;
                        case ',':
                            json.Read();
                            return TOKEN.COMMA;
                        case '"':
                            return TOKEN.STRING;
                        case ':':
                            return TOKEN.COLON;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                        case '-':
                            return TOKEN.NUMBER;
                    }
                    string word = NextWord;
                    switch (word)
                    {
                        case "false":
                            return TOKEN.FALSE;
                        case "true":
                            return TOKEN.TRUE;
                        case "null":
                            return TOKEN.NULL;
                    }
                    return TOKEN.NONE;
                }
            }
        }
        /// <summary>
        /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
        /// </summary>
        /// <param name="json">A Dictionary<string, object> / List<object></param>
        /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
        public static string Serialize(object obj)
        {
            return Serializer.Serialize(obj);
        }
        sealed class Serializer
        {
            StringBuilder builder;
            Serializer()
            {
                builder = new StringBuilder();
            }
            public static string Serialize(object obj)
            {
                var instance = new Serializer();
                instance.SerializeValue(obj);
                return instance.builder.ToString();
            }
            void SerializeValue(object value)
            {
                IList<Object> asList;
                List<Dictionary<string, string>> asMixList;
                IDictionary<string, Object> asDict;
                Dictionary<string, string> asStrDict;
                Dictionary<string, List<Dictionary<string, string>>> asMixDict;

                string asStr;
                if (value == null)
                {
                    builder.Append("null");
                }
                else if ((asStr = value as string) != null)
                {
                    SerializeString(asStr);
                }
                else if (value is bool)
                {
                    builder.Append(value.ToString().ToLower());
                }
                else if ((asList = value as IList<Object>) != null)
                {
                    SerializeArray(asList);
                }
                else if ((asMixList = value as List<Dictionary<string, string>>) != null)
                {
                    SerializeArray(asMixList);
                }
                else if ((asDict = value as IDictionary<string, Object>) != null)
                {
                    SerializeObject(asDict);
                }
                else if ((asStrDict = value as Dictionary<string, string>) != null)
                {
                    SerializeObject(asStrDict);
                }
                else if ((asMixDict = value as Dictionary<string, List<Dictionary<string, string>>>) != null)
                {
                    SerializeObject(asMixDict);
                }
                else if (value is char)
                {
                    SerializeString(value.ToString());
                }
                else
                {
                    SerializeOther(value);
                }
            }
            void SerializeObject(IDictionary<string, Object> obj)
            {
                bool first = true;
                builder.Append('{');
                foreach (string e in obj.Keys)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }
                    SerializeString(e);
                    builder.Append(':');
                    SerializeValue(obj[e]);
                    first = false;
                }
                builder.Append('}');
            }
            void SerializeObject(Dictionary<string, string> obj)
            {
                bool first = true;
                builder.Append('{');
                foreach (string e in obj.Keys)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }
                    SerializeString(e);
                    builder.Append(':');
                    SerializeValue(obj[e]);
                    first = false;
                }
                builder.Append('}');
            }
            void SerializeObject(Dictionary<string, List<Dictionary<string, string>>> obj)
            {
                bool first = true;
                builder.Append('{');
                foreach (string e in obj.Keys)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }
                    SerializeString(e);
                    builder.Append(':');
                    SerializeValue(obj[e]);
                    first = false;
                }
                builder.Append('}');
            }
            void SerializeArray(IList<Object> anArray)
            {
                builder.Append('[');
                bool first = true;
                foreach (object obj in anArray)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }
                    SerializeValue(obj);
                    first = false;
                }
                builder.Append(']');
            }
            void SerializeArray(List<Dictionary<string, string>> anArray)
            {
                builder.Append('[');
                bool first = true;
                foreach (object obj in anArray)
                {
                    if (!first)
                    {
                        builder.Append(',');
                    }
                    SerializeValue(obj);
                    first = false;
                }
                builder.Append(']');
            }
            void SerializeString(string str)
            {
                builder.Append('\"');
                char[] charArray = str.ToCharArray();
                foreach (var c in charArray)
                {
                    switch (c)
                    {
                        case '"':
                            builder.Append("\\\"");
                            break;
                        case '\\':
                            builder.Append("\\\\");
                            break;
                        case '\b':
                            builder.Append("\\b");
                            break;
                        case '\f':
                            builder.Append("\\f");
                            break;
                        case '\n':
                            builder.Append("\\n");
                            break;
                        case '\r':
                            builder.Append("\\r");
                            break;
                        case '\t':
                            builder.Append("\\t");
                            break;
                        default:
                            int codepoint = Convert.ToInt32(c);
                            if ((codepoint >= 32) && (codepoint <= 126))
                            {
                                builder.Append(c);
                            }
                            else
                            {
                                builder.Append("\\u" + Convert.ToString(codepoint, 16).PadLeft(4, '0'));
                            }
                            break;
                    }
                }
                builder.Append('\"');
            }
            void SerializeOther(object value)
            {
                if (value is float
                    || value is int
                    || value is uint
                    || value is long
                    || value is double
                    || value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is ulong
                    || value is decimal)
                {
                    builder.Append(value.ToString());
                }
                else
                {
                    SerializeString(value.ToString());
                }
            }
        }
    }
}
