using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkNet;

namespace bot
{
    class Friends
    {
        public static void parsingRequests(VkApi api)
        {
            bool endRequests = false;
            int repeat = 0;

            while (!endRequests)
            {
                var requests = api.Friends.GetRequests(new VkNet.Model.RequestParams.FriendsGetRequestsParams
                {
                    Count = 1000,
                    Out = false,
                });

                if (requests.Items.Count == 0)
                {
                    break;
                }
                else if (requests.Items.Count < 200)
                {
                    endRequests = true;
                }

                var getUsersProfileInfo = api.Users.Get(requests.Items);

                foreach (long userId in requests.Items)
                {
                    foreach (var profileInfo in getUsersProfileInfo)
                    {
                        if (profileInfo.Id == userId)
                        {
                            if (profileInfo.IsDeactivated)
                            {
                                System.Console.WriteLine("Отклоняю заявку в друзья от пользователя - " + profileInfo.FirstName + " " + profileInfo.LastName);
                                api.Friends.Delete(userId);
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                System.Console.WriteLine("Добавляю в друзья пользователя - " + profileInfo.FirstName + " " + profileInfo.LastName);
                                api.Friends.Add(userId, "", false);
                                Thread.Sleep(1000);
                            }
                        }
                    }
                }

                repeat++;
            }
        }
    }
}
