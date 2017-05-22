using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Yodo1ServiceClient
{
    class Yodo1RequestJsonContent
    {
        private StringContent innerContent;
        public Yodo1RequestJsonContent(Object content,Encoding encoding, string mediaType = "text/plain")
        {
            innerContent = new StringContent(JSONHelper.Serialize(content), encoding, mediaType);
        }
        public HttpContent getContent()
        {
            return null != innerContent ? innerContent : new StringContent("");
        }
    }
}
