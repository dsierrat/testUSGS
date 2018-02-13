using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using TestUsgs;

namespace ServiceVerifier
{
    public class Program
    {
        private static System.Timers.Timer aTimer;
        public static bool firstTimeFails = false;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("The program have started");
                // MessageBox.Show("The program have started successfully", "Program", MessageBoxButtons.OK);
                aTimer = new System.Timers.Timer(10000);
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
                new ManualResetEvent(false).WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var usgsString = new WebClient().DownloadString("https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime=2018-02-11&endtime=2018-02-12&minmagnitude=5");
            var model = JsonConvert.DeserializeObject<USGSModel>(usgsString);
            

            foreach (USGSModel.Evento evento in model.Feature)
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddMilliseconds(evento.Propiedades.Hora).ToUniversalTime();
                Console.WriteLine(evento.Propiedades.Titulo + " at " + dtDateTime);
            }

        }
    }
}
