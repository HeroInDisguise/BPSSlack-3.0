using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BPSSlack
{
    /*Diese Klasse enthält alle nötigen Parameter um die Userliste zu empfangen und erstellt daraus dann eine Json-Datei die gespeichert und eingelesen wird
     um nicht bei jedem Programmstart die Abfrage wiederholen zu müssen*/
    class Users
    {
        //Dies Klasse wäre zur selbstidentifikation, falls wir dies nutzen wollen
        public class MySelf
        {
            public string id { get; set; }
            public string name { get; set; }
        }
        //---------------------------------------------------

        public bool ok { get; set; }
        public Member[] members { get; set; }
 
        public class Member
        {
            public string id { get; set; }
            public string name { get; set; }
            public string real_name { get; set; }
            public Profile profile { get; set; }
        }

        public class Profile
        {
            public string email { get; set; }
            public string display_name { get; set; }
        }

        public class Channel
        {
            public Channels[] channels { get; set; }
            public class Channels
            {
                public string id { get; set; }
                public string name { get; set; }
            }
        }
        /*Methode zum Abrufen der User - Diese Methode tut zwar was sie soll aber die Daten(User) werden natürlich noch nicht genutzt 
        da ich hierzu noch auf das UserInterface(Einbindung in Team3) abwarte*/
        public async Task CheckForUsers()
        {
            string line;
            try
            {
                if (!File.Exists(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackUserList.json"))
                {
                    using (File.Create(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackUserList.json")) ;
                }
                using (StreamReader sr = new StreamReader(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackUserList.json"))
                {
                    line = sr.ReadToEnd();
                }
                    if (line == null || line == "")
                    {
                        Console.WriteLine("Die Userliste ist nicht vorhanden. Drücken Sie eine Taste um die Userliste abzurufen.\n");
                        Console.ReadKey();                        
                        var UserJ = await Message.GetUser();
                        string User = await UserJ.Content.ReadAsStringAsync();

                        using (StreamWriter Sw = new StreamWriter(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackUserList.json"))
                        {
                            Sw.WriteLine(User);
                        }
                        await CheckForUsers();
                    }
                    else
                    {//Diese ausdrückliche Ausgabe der Parameter ist nur für mich, um die Funktionsweise nachvollziehen und sicherstellen zu können! Wird später natürlich entfernt! :)
                    var X = JsonConvert.DeserializeObject<Users>(line);
                        for (int i = 0; i < X.members.Length; i++)
                        {                        
                            Console.WriteLine(X.members[i].id);                            
                            Console.WriteLine(X.members[i].name);
                            Console.WriteLine(X.members[i].real_name);
                            Console.WriteLine(X.members[i].profile.email);
                            string email = X.members[i].profile.email;      //hier findet eine Identifikation des Users via email statt
                            if(String.Equals(email, Team3User.email))
                            {
                                Message.username = Team3User.name;
                            }
                            Console.WriteLine(X.members[i].profile.display_name);
                            Console.WriteLine("-------------------------------------------");
                        }                    
                        Console.ReadKey();
                    }     
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        /*Methode zum Abrufen der Channel - Diese Methode tut zwar was sie soll aber die Daten(Channels) werden natürlich noch nicht genutzt 
        da ich hierzu noch auf das UserInterface(Einbindung in Team3) abwarte*/
        public async Task CheckForChannels()
        {
            string line;
            try
            {
                if (!File.Exists(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackChannelList.json"))
                {
                    using (File.Create(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackChannelList.json")) ;
                }
                using (StreamReader sr = new StreamReader(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackChannelList.json"))
                {
                    line = sr.ReadToEnd();
                }
                if (line == null || line == "")
                {
                    Console.WriteLine("Die Channelliste ist nicht vorhanden. Drücken Sie eine Taste um die Userliste abzurufen.\n");
                    Console.ReadKey();
                    var ChannelsJ = await Message.GetChannels();
                    string Channels = await ChannelsJ.Content.ReadAsStringAsync();

                    using (StreamWriter Sw = new StreamWriter(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackChannelList.json"))
                    {
                        Sw.WriteLine(Channels);
                    }
                    await CheckForChannels();
                }
                else
                {
                    var X = JsonConvert.DeserializeObject<Channel>(line);
                    for (int i = 0; i < X.channels.Length; i++)
                    {
                        Console.WriteLine(X.channels[i].id);
                        Console.WriteLine(X.channels[i].name);
                        Console.WriteLine("-------------------------------------------");
                    }
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read: \n");
                Console.WriteLine(e.Message);
            }
        }
    }
}
