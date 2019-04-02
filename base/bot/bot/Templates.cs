using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    class Templates
    {
        public static List<MessageTemplate.SimpleTemplate> templates = new List<MessageTemplate.SimpleTemplate>();
        public static void initial()
        {
            templates.Add(
                new MessageTemplate.SimpleTemplate(
                    new string[] {
                        "hello", "hi", "здаров", "хай", "<4>ч(о|ё|е) как",
                        "привет", "<3>хой",
                        "<0>вечер в хату", "<1>Лол", "<2>Ку"
                    },
                    new string[] {
                        "Ну привет", "Привет", "Хай", "Драсьте",
                        "<0>Часик в радость.", "<1>Кек", "<2><%50>Ку-ку", "<4>Неплохо."
                    }
                )
            );

            templates.Add(
                new MessageTemplate.SimpleTemplate(
                    new string[] {
                            @"<1>^(!|/)(\s*)(random|rnd|rand|ранд|рандом|рнд)(\s*)"
                    },
                    new string[] {
                            "<1><random>"
                    }
                )
            );
        }

        public class keysStructure
        {
            public string key { get; set; }
            public int startPos { get; set; }
            public int endPos { get; set; }
            public int lenght { get; set; }
            public keysStructure(string k, int sp, int ep, int len)
            {
                key = k;
                startPos = sp;
                endPos = ep;
                lenght = len;
            }
        }

        public static string keySearch(List<keysStructure> list_keys, string text, string value = null)
        {
            foreach (var keys in list_keys)
            {
                if (keys.key == "random")
                {
                    if (value == null || value == "" || value == " ")
                        value = "100";

                    return new Random().Next(0, System.Convert.ToInt32(value)).ToString();
                }

                if (keys.key[0] == '%')
                {
                    int val = System.Convert.ToInt32(keys.key.Substring(1, keys.key.Length - 1));

                    if (val < new Random().Next(1, 100))
                    {
                        return "true";
                    }

                    return "false";
                }
            }

            return null;
        }
    }
}
