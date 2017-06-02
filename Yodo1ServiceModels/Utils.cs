using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

namespace Yodo1ServiceModels
{
    public class Utils
    {
        public static string MD5(string content)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(content));
            return System.Text.Encoding.UTF8.GetString(result);
        }
    }
}
