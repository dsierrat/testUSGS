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
        public static MySqlConnection con;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("The program have started");
                if (Conexion())
                {
                    MessageBox.Show("The program have started successfully", "Program");

                    var aTimer = new System.Timers.Timer(10000);
                    aTimer.Elapsed += OnTimedEvent;
                    aTimer.AutoReset = true;
                    aTimer.Enabled = true;
                    new ManualResetEvent(false).WaitOne();
                }
                else
                {
                    Console.Beep(2000, 1000);
                    MessageBox.Show("ERROR: couldn't connect to the dataBase", "Program");
                }               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var fechaActual = DateTime.Now.ToString("yyyy-MM-dd");
            var usgsString = new WebClient().DownloadString("https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime="+fechaActual+"&minmagnitude=5");
            var model = JsonConvert.DeserializeObject<USGSModel>(usgsString);

            foreach (USGSModel.Evento evento in model.Feature)
            {
                DateTime fechaEvento = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                fechaEvento = fechaEvento.AddMilliseconds(evento.Propiedades.Hora).ToUniversalTime();
                InsertEvent(evento.Propiedades, fechaEvento);
            }
        }

        public static void InsertEvent(USGSModel.Propiedades propiedades, DateTime fecha )
        {
            try {
                if (!SearchExistence(propiedades.Code))
                {
                    Console.WriteLine(propiedades.Titulo + " at " + fecha);                    MySqlCommand comm = con.CreateCommand();
                    comm.CommandText = "INSERT INTO evento(title,code,fecha,magnitud,importancia) VALUES(@title,@code,@fecha,@magnitud,@importancia)";
                    comm.Parameters.AddWithValue("@title", propiedades.Titulo);
                    comm.Parameters.AddWithValue("@code", propiedades.Code);
                    comm.Parameters.AddWithValue("@fecha", fecha);
                    comm.Parameters.AddWithValue("@magnitud", propiedades.Magnitud);
                    comm.Parameters.AddWithValue("@importancia", propiedades.Importancia);
                    comm.ExecuteNonQuery();
                    Console.Beep(2000, 2000);
                    MessageBox.Show("There's a new Earthqueake in " + propiedades.Lugar + " Magnitud: " + propiedades.Magnitud, "Program");
                }
                else
                {
                    Console.WriteLine("The event " + propiedades.Titulo +" Already exists");
                }                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e.Message);
                Console.Beep(2000, 2000);
            }
        }

        private static bool SearchExistence(string code)
        {
            try {
                MySqlCommand comm = con.CreateCommand();
                comm.CommandText = "SELECT code FROM evento WHERE code=@code";
                comm.Parameters.AddWithValue("@code", code);
                var query = comm.ExecuteReader();
                bool exists = query.HasRows;
                query.Close();

                return exists;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e.Message);
                return false;
            }
        }

        public static bool Conexion()
        {
            try
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
                return con.State == System.Data.ConnectionState.Open;
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't connect to BD " + e.Message);
                return false;
            }
        }
    }
}
