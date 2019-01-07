using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BPSSlack
{
    /*Diese Klasse beinhaltet alle eventuellen Parameter, die an Slack gesendet werden können
     und die jeweiligen zum Senden von Nachrichten und dem Abrufen von Channels und Usern*/
    public static class Message
    {   //die Parameter für die Nachrichten
        public static string token { get; set; }
        public static string channel { get; set; }
        public static string channels { get; set; }
        public static string text { get; set; }
        public static string initial_comment { get; set; }
        public static string file { get; set; }
        public static string count { get; set; }
        public static string unreads = "1";
        public static string error { get; set; }
        public static bool ok { get; set; }
        public static string as_user = "true";
        public static string username { get; set; }
        public static string icon_url { get; set; }
        
        public static void CheckToken()        // Diese Funktion prüft ob ein Token zur authentifizierung vorhanden ist. Bei Erstausführung des Codes muss der Token eingefügt werden.
        {
            if (token == null || token == "")
            {
                try
                {
                    string line;
                    if (!File.Exists(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackToke.txt"))             //In diesem Pfad wird der token in einer txt hinterlegt
                    {                                                                                       //Muss natürlich noch festgelegt werden wo der Token dann endgültig gespeichert wird!
                        using (File.Create(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackToke.txt"));
                    }
                    using (StreamReader sr = new StreamReader(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackToke.txt"))
                    {
                        line = sr.ReadToEnd();
                    }
                        if (line == null || line == "")             //Hier wird geprüft ob ein Token vorhanden ist, falls nicht folgt die Routine die den User zur Eingabe auffordert
                        {                                           //Hier fehlen noch Sicherungsmaßnahmen für den Fall, dass eine andere Zeichenfolge als der Token eingegeben werden!
                            Console.WriteLine("Sie haben noch keinen gültigen Token für die nutzung der SlackAPI eingetragen. \n Diesen finden Sie hier: https://api.slack.com/apps/ADTNVBJKG/oauth? \n");
                            Console.WriteLine("Bitte folgen Sie dem Link und kopieren Sie den Token hier herein.\n");
                            try
                            {
                                string Token = Console.ReadLine();                                
                            if (Token != null)
                                {
                                    using (StreamWriter Sw = new StreamWriter(@"C:\Users\f.held\Desktop\Held-Docs\Slack\SlackToke.txt"))
                                    {
                                        Sw.Write(Token);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.ReadKey();
                            }
                            token = line;
                            CheckToken();   //rekursiver aufruf damit nun der Token gelesen werden kann
                        }
                        else
                        {
                            token = line;                            
                        }                    
                }
                catch (Exception e)
                {                    
                    Console.WriteLine(e.Message);                    
                }
            }
        }
        //Diese Methode kann eine Textnachricht versenden
        public static Task <HttpResponseMessage> SendMessage(string channel, string message)
        {
            string Uri = "https://slack.com/api/chat.postMessage";
            var Json = new Dictionary<string, string>
            {
                {"token", token},
                {"channel", channel},
                {"text", message},
                {"as_user", as_user},
                {"username", username }
            };
            var Content = new FormUrlEncodedContent(Json);
            var Client1 = new BPSClient(Content, Uri);
            var Response = Client1.SendMessage();

            return Response;
        }
        //Diese Methode kann eine Datei mit oder ohne Text senden
        public static Task<HttpResponseMessage> SendFile(string channel, string message, string path)
        {
            string Uri = "https://slack.com/api/files.upload";
            var Content = new MultipartFormDataContent();
            var File = new StreamContent(GetFile.ReadFile(path));            
            Content.Add(new StringContent(token), "token");
            Content.Add(new StringContent(channel), "channels");
            if (message != null)
                Content.Add(new StringContent(message), "initial_comment");
            Content.Add(File, "file", Path.GetFileName(path));
            var Client2 = new BPSClient(Content, Uri);
            var Response = Client2.SendMessage();
            return Response;
        }
        //Diese Methode dient einfach dem Einlesen der hochzuladenden Datei
        public static class GetFile
        {
            public static FileStream ReadFile(string path)
            {
                FileInfo FI = new FileInfo(path);
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
                return fs;
            }
        }
        /*Diese Methode ruft die Userliste von Slack ab. Hierüber kann dann eine Liste von Kontakten und vor allem die eigene Identifikation stattfinden 
          Die Identifikation hierüber ist nur Notwendig falls wir keine persönlichen Token verteilen wollen!!! */
        public static Task<HttpResponseMessage> GetUser()
        {
            string Uri = "https://slack.com/api/users.list";
            var Json = new Dictionary<string, string>
            {
                {"token", token}
            };
            var Content = new FormUrlEncodedContent(Json);
            var Client1 = new BPSClient(Content, Uri);
            var Response = Client1.SendMessage();

            return Response;
        }
        //Diese Methode ruft die Channels ab 
        public static Task<HttpResponseMessage> GetChannels()
        {
            string Uri = "https://slack.com/api/conversations.list";
            var Json = new Dictionary<string, string>
            {             
                {"token", token}
            };
            var Content = new FormUrlEncodedContent(Json);
            var Client1 = new BPSClient(Content, Uri);
            var Response = Client1.SendMessage();

            return Response;
        }
    }
}