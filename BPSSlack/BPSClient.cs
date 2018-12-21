using System.Net.Http;
using System.Threading.Tasks;

namespace BPSSlack
{
    /*Hier wird ein HttpClient erstellt über den alle Nachrichten gesendet werden*/
    class BPSClient
    {
        private readonly HttpClient Client = new HttpClient();
        private FormUrlEncodedContent content;
        private MultipartFormDataContent content1;
        private string Uri;
        //Konstruktor um Text zu versenden
        public BPSClient(FormUrlEncodedContent _content, string _Uri)
        {
            content = _content;
            Uri = _Uri;
        }
        //Konstruktor um Dateien zu versenden
        public BPSClient(MultipartFormDataContent _content, string _Uri)
        {
            content1 = _content;
            Uri = _Uri;
        }
        //Methode die tatsächlich kommuniziert! - hier wird zwischen reiner Textnachricht oder Datei unterschieden
        public async Task<HttpResponseMessage> SendMessage()
        {
            if (content != null)
            {
                var response = await Client.PostAsync(Uri, content);
                return response;
            }
            else
            {
                var response1 = await Client.PostAsync(Uri, content1);

                return response1;
            }            
        }
    }
}