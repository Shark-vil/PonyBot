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
            public bool findingAnyMatch;
            public bool isCommand;
            public bool onlyDeveloper;

            public SimpleTemplate(string[] rec, string[] sen, bool fam = true, bool icom = false, bool onlyDev = false)
            {
                received = rec;
                sent = sen;
                findingAnyMatch = fam;
                isCommand = icom;
                onlyDeveloper = onlyDev;
            }
        }
    }
}
