using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace ClienteVuelos
{
    public class ClienteVuelo
    {
        public delegate void Movimientos();
        public event Movimientos AlHaberMovimientos;
        HttpClient cliente = new HttpClient();
        public ClienteVuelo()
        {
            cliente.BaseAddress=new Uri("http://vuelos.itesrc.net");
        }

        public async void Agregar(DatosVuelos vuelos)
        {
            var json = JsonConvert.SerializeObject(vuelos);
            var result = await cliente.PostAsync("/Tablero", new StringContent(json, Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();
        }
        public async void Editar(DatosVuelos vuelos)
        {
            var json = JsonConvert.SerializeObject(vuelos);
            var result = await cliente.PutAsync("/Tablero", new StringContent(json, Encoding.UTF8, "application/json"));
            result.EnsureSuccessStatusCode();
        }
        public async void Eliminar(DatosVuelos vuelos)
        {
            var json = JsonConvert.SerializeObject(vuelos);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, "/Tablero");
            message.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await cliente.SendAsync(message);
            result.EnsureSuccessStatusCode();
        }
        public IEnumerable<DatosVuelos> Modelo { get; set; }
        public async void Get()
        {
            
            var client = new HttpClient();
            var response = await client.GetAsync("http://vuelos.itesrc.net/Tablero");
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                Modelo = JsonConvert.DeserializeObject<IEnumerable<DatosVuelos>>(jsonString);
                AlHaberMovimientos?.Invoke();
            }
        }
    }
}