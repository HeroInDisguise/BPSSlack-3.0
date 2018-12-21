using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPSSlack;

namespace BPSSlackMain
{
    class MainArea
    {
        static void Main(string[] args)
        {
            T3Slack Slack = new T3Slack();
            try
            {
                Task.WaitAll(Slack.SendMessage("DCW21NBHD", "Gettin' there..."));   // Hier rufe ich meine Methode auf - 1.Parameter ist der Channel,
            }catch (Exception ups)                                                  //zweiter der Text der Nachricht, ein eventueller dritter ein Dateipfad
            {
                Console.WriteLine(ups);
                Console.ReadKey();
            }
        }
    }
}