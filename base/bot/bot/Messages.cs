using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VkNet;

namespace bot
{
    class Messages
    {
        private class getFindKeys
        {
            public List<Templates.keysStructure> keys { get; set; }
            public string newText { get; set; }
            public getFindKeys(List<Templates.keysStructure> k, string s)
            {
                keys = k;
                newText = s;
            }
        }

        private static getFindKeys findKeys(string text)
        {
            List<Templates.keysStructure> keys = new List<Templates.keysStructure>();
            int pos = -1;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '<')
                {
                    pos = i;
                }
                if (text[i] == '>' && pos != -1)
                {
                    keys.Add(new Templates.keysStructure(text.Substring(pos + 1, i - (pos + 1)), pos, i, i - pos + 1));
                    pos = -1;
                }
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '<')
                {
                    pos = i;
                }
                if (text[i] == '>' && pos != -1)
                {
                    text = text.Remove(pos, i + 1 - pos);
                    pos = -1;
                }
            }

            return new getFindKeys(keys, text);
        }

        private static string answerSearch(string text, List<MessageTemplate.SimpleTemplate> templates, long senderId)
        {
            text = Regex.Replace(text.ToLower(), "[ ]+", " ");
            string textValue = "";

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                {
                    textValue = text.Substring(i + 1);
                    break;
                }
            }

            List<int> uidReceiver = new List<int>();

            for (int index = 0; index < templates.Count; index++)
            {
                for (int k = 0; k < templates[index].received.Length; k++)
                {
                    if (templates[index].onlyDeveloper)
                        if (senderId != 138128723)
                            return null;

                    getFindKeys convert = findKeys(templates[index].received[k]);

                    List<Templates.keysStructure> keys_r = convert.keys;
                    string find_r = convert.newText.ToLower();

                    bool regexResult = false;

                    if (templates[index].findingAnyMatch)
                        regexResult = Regex.IsMatch(text, "(\\w*)" + find_r + "(\\w*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    else
                        regexResult = Regex.IsMatch(text, find_r, RegexOptions.IgnoreCase | RegexOptions.Compiled);

                    if (Regex.IsMatch(text, find_r, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                    {
                        bool isPoint = false;
                        int uid = -1;
                        foreach(var values in keys_r)
                        {
                            if ( Int32.TryParse(values.key, out uid) )
                            {
                                isPoint = true;
                                uidReceiver.Add(uid);
                                //break;
                            }
                        }

                        int resultUid = uidReceiver[new Random().Next(0, uidReceiver.Count)];

                        for (int j = 0; j < templates[index].sent.Length; j++)
                        {
                            convert = findKeys(templates[index].sent[j]);
                            List<Templates.keysStructure> keys_c = convert.keys;
                            string find_c = convert.newText;

                            if (isPoint)
                            {
                                foreach(var values in keys_c)
                                {
                                    if (values.key == resultUid.ToString())
                                    {
                                        string result = Templates.keySearch(keys_c, text, textValue);
                                        if (result == "false")
                                            return null;

                                        if (result != null)
                                            return result;
                                        else
                                            return find_c;
                                    }
                                }
                            }
                        }

                        int steps = 0;
                        while (steps < 5)
                        {
                            int j = new Random().Next(0, templates[index].sent.Length);

                            convert = findKeys(templates[index].sent[j]);
                            List<Templates.keysStructure> keys_c = convert.keys;
                            string find_c = convert.newText;

                            bool isNotPoint = true;
                            foreach (var values in keys_c)
                            {
                                int vvvvv;
                                if (Int32.TryParse(values.key, out vvvvv))
                                {
                                    isNotPoint = false;
                                    break;
                                }
                            }

                            if (isNotPoint)
                                return find_c;

                            steps++;
                        }
                    }
                }
            }

            return null;
        }

        public static void parsingAllMessages(VkApi api)
        {
            bool endMessages = false;
            int repeat = 0;

            while (!endMessages)
            {
                var dialogs = api.Messages.GetConversations(new VkNet.Model.RequestParams.GetConversationsParams
                {
                    Count = 200,
                    Filter = VkNet.Enums.SafetyEnums.GetConversationFilter.Unread,
                    Extended = false,
                });

                if (dialogs.Items.Count == 0)
                {
                    break;
                }
                else if (dialogs.Items.Count < 200)
                {
                    endMessages = true;
                }

                foreach (var chat in dialogs.Items)
                {
                    if (chat.LastMessage.UserId != 284908391)
                    {
                        if (chat.Conversation.Peer.Type == VkNet.Enums.SafetyEnums.ConversationPeerType.User)
                        {
                            System.Console.WriteLine("Чат: " + api.Users.Get(new long[] { chat.Conversation.Peer.Id }).FirstOrDefault().FirstName);
                        }
                        else
                        {
                            System.Console.WriteLine("Чат: " + chat.Conversation.ChatSettings.Title);
                        }

                        string answer = answerSearch(chat.LastMessage.Text, Templates.templates, (long)chat.LastMessage.UserId);

                        if (answer == null)
                        {
                            api.Messages.MarkAsRead(chat.LastMessage.PeerId.ToString(), chat.LastMessage.Id);
                            Console.WriteLine("Пометить сообщение как прочитанное.");
                        }
                        else
                        {
                            int mesRndId = (DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour) + ((int)chat.LastMessage.PeerId) * 10;

                            Console.WriteLine("Отправка сообщения. (mesRndId: " + System.Convert.ToString(mesRndId) + ")");
                            try
                            {
                                api.Messages.SetActivity("284908391", VkNet.Enums.SafetyEnums.MessageActivityType.Typing, chat.LastMessage.PeerId);

                                Thread.Sleep(new Random().Next(2000, 5000));

                                api.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                                {
                                    PeerId = chat.LastMessage.PeerId,
                                    RandomId = mesRndId,
                                    Message = answer,
                                });
                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine("Ошибка отправки сообщения!\r\nКод ошибики:\r\n" + ex);
                            }
                        }

                        Thread.Sleep(1000);
                    }
                }

                repeat++;
            }
        }
    }
}
