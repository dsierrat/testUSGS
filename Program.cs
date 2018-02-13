using MySql.Data.MySqlClient;
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
        public static MySqlConnection con;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("The program have started");
                ProbarConexion();
                // MessageBox.Show("The program have started successfully", "Program", MessageBoxButtons.OK);
                aTimer.BeginInit();
                aTimer = new System.Timers.Timer(100000);
                aTimer.Elapsed += OnTimedEvent;
                aTimer.AutoReset = true;
                aTimer.Enabled = true;
                new ManualResetEvent(false).WaitOne();
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var fechaActual = DateTime.Now.ToString("yyyy-MM-dd h:mm tt");
            var usgsString = new WebClient().DownloadString("https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime=2017-02-01&minmagnitude=5");
            var model = JsonConvert.DeserializeObject<USGSModel>(usgsString);


            foreach (USGSModel.Evento evento in model.Feature)
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddMilliseconds(evento.Propiedades.Hora).ToUniversalTime();
                Console.WriteLine(evento.Propiedades.Titulo + " at " + dtDateTime);
                InsertEvent(evento,dtDateTime);
            }

        }

        public static void InsertEvent(USGSModel.Evento evento, DateTime fecha )
        {
            try {
                MySqlCommand comm = con.CreateCommand();
                comm.CommandText = "INSERT INTO evento(title,code,fecha) VALUES(@title,@code,@fecha)";
                comm.Parameters.AddWithValue("@title", evento.Propiedades.Titulo);
                comm.Parameters.AddWithValue("@code", evento.Propiedades.Code);
                comm.Parameters.AddWithValue("@fecha", fecha);
                comm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Evento repetido");
            }
        }


        public static bool ProbarConexion()
        {
            
            String servidor = "localhost";
            String puerto = "3306";
            String usuario = "root";
            String password = "admin";
            String database = "usgs";

            //Cadena de conexion
            var cadenaConexion = String.Format("server={0};port={1};user id={2}; password={3}; database={4}", servidor, puerto, usuario, password, database);

            con = new MySqlConnection(cadenaConexion);
            con.Open();//se abre la conexion
            if (con.State == System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Se conecto con exito a la base de datos");
                return true;
            }
            else
            {
                Console.WriteLine("Error al conectar a la base de datos");
                return false;
            }
        }
    }
}
