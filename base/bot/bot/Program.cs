using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using VkNet;
using VkNet.Model;
using System.Runtime.Serialization.Json;
using VkNet.Enums.Filters;

namespace bot
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string token;

                if (args.Length == 0)
                {
                    System.Console.Write("Введите токен: ");
                    token = System.Console.ReadLine();
                }
                else
                    token = args[0];

                var api = new VkApi();
                api.Authorize(new ApiAuthParams { AccessToken = token });

                //var api = new VkApi();
                //api.Authorize(new ApiAuthParams {
                //    ApplicationId = 5668658,
                //    Settings = Settings.All,
                //    Login = "shark_vil@mail.ru",
                //    Password = "izurob95ponybot"
                //});

                Templates.initial();

                while (true)
                {
                    Messages.parsingAllMessages(api);
                    Thread.Sleep(new Random().Next(1000, 2000));
                    Friends.parsingRequests(api);
                    Thread.Sleep(new Random().Next(1000, 2000));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                System.Console.ReadLine();
            }
        }
    }
}
