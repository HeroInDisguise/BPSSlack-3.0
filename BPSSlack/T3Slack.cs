using System;
using System.Threading.Tasks;

namespace BPSSlack
{
    /*Dies ist die Klasse von der ein Objekt erzeugt wird um dann die MEthode "SendMessage" aufzurufen*/
    public class T3Slack
    {
        Users U = new Users();
        //Nötige Schritte um sicherzustellen, dass Token und User/Channel-Liste vorhanden ist
        public async Task TestForTokenAndUsers()
        {
            Message.CheckToken();
            await U.CheckForUsers();
            await U.CheckForChannels();
        }
                
        public async Task SendMessage(string channel, string message = null, string path = null)
        {
            await TestForTokenAndUsers();     

            if (path == null)
                {
                    var Response = await Message.SendMessage(channel, message);
                    string Check = await Response.Content.ReadAsStringAsync();

                    Console.WriteLine(Check);
                    Console.ReadKey();
                }
                else
                {
                    var Response1 = await Message.SendFile(channel, message, path);
                    string Check = await Response1.Content.ReadAsStringAsync();

                    Console.WriteLine(Check);
                    Console.ReadKey();
                }
            }
        }
    }