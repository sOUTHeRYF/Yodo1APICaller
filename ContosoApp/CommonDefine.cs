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

}
