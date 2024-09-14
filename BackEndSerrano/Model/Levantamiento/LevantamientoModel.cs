using System.Dynamic;

namespace BackEndSerrano.Model.Levantamiento
{
    public class LevantamientoModel
    {
        public int ID { get; set; }
        public dynamic FechaCreacion { get; set; }
        public dynamic FechaCierre { get; set; }
        public string UsuarioResponsable { get; set; }
        public string IPAddress { get; set; }   
        public string Area { get; set; }
        public string Pasillo  { get; set; }
        public string Observaciones { get; set; }
        public int IDEstado { get; set; }

    }
}
