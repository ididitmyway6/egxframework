using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Egx.EgxBusiness
{
    public enum MessageType { Stop, Pass, Fail }
    public class SystemMessage
    {
        public string Message { get; set; }
        public object Attachment { get; set; }
        public MessageType Type { get; set; }
    }
}
