using Newtonsoft.Json;
using System.Collections.Generic;

namespace TestUsgs
{
    public class USGSModel
    {
        [JsonProperty(PropertyName = "features")]
        public List<Evento> Feature { get; set; }

        public class Evento
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            [JsonProperty(PropertyName = "properties")]
            public Propiedades Propiedades { get; set; }
            [JsonProperty(PropertyName = "geometry")]
            public Geometry Geometry { get; set; }
        }

        public class Propiedades
        {
            [JsonProperty(PropertyName = "mag")]
            public double Magnitud { get; set; }
            [JsonProperty(PropertyName = "place")]
            public string Lugar { get; set; }
            [JsonProperty(PropertyName = "time")]
            public long Hora { get; set; }
            [JsonProperty(PropertyName = "updated")]
            public long Actualizado { get; set; }
            [JsonProperty(PropertyName = "tz")]
            public int tz { get; set; }
            [JsonProperty(PropertyName = "url")]
            public string Url { get; set; }
            [JsonProperty(PropertyName = "detail")]
            public string DetalleLink { get; set; }
            //DYFI = Did you feel it 
            [JsonProperty(PropertyName = "felt", NullValueHandling =NullValueHandling.Ignore)]
            public int DYFINumber { get; set; }

            //Mercalli intensity reported by DYFI
            [JsonProperty(PropertyName = "cdi", NullValueHandling = NullValueHandling.Ignore)]
            public double MercalliDYFI { get; set; }
            //Mercalli intensity reported by shakeMap
            [JsonProperty(PropertyName = "mmi", NullValueHandling = NullValueHandling.Ignore)]
            public double MercalliMagnitud { get; set; }

            [JsonProperty(PropertyName = "alert")]
            public string Alerta { get; set; }
            //“automatic”, “reviewed”, “deleted”
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
            [JsonProperty(PropertyName = "tsunami")]
            public int Tsunami { get; set; }
            [JsonProperty(PropertyName = "sig", NullValueHandling = NullValueHandling.Ignore)]
            public int Importancia { get; set; }
            [JsonProperty(PropertyName = "code")]
            public string Code { get; set; }
            [JsonProperty(PropertyName = "magType")]
            public string magType { get; set; }
            [JsonProperty(PropertyName = "type")]
            public string Tipo { get; set; }
            [JsonProperty(PropertyName = "title")]
            public string Titulo { get; set; }
        }

        public class Geometry
        {
            [JsonProperty(PropertyName = "type")]
            public string Tipo_geo { get; set; }
            [JsonProperty(PropertyName = "coordinates")]
            public List<double> Cordenadas_geo { get; set; }
        }

    }
}
