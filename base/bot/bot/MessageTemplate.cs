using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    class MessageTemplate
    {
        public class SimpleTemplate
        {
            public string[] received { get; set; }
            public string[] sent { get; set; }
            public SimpleTemplate(string[] rec, string[] sen)
            {
                received = rec;
                sent = sen;
            }
        }
    }
}
