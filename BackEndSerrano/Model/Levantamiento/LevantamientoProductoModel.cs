namespace BackEndSerrano.Model.Levantamiento
{
    public class LevantamientoProductoModel
    {
        public int ID { get; set; }
        public int IDLevantamiento { get; set; }
        public string CODProducto { get; set; }
        public string Descripcion { get; set; }
        public dynamic FechaHora { get; set; }
        public string Observaciones { get; set; }
        public int IDEstado { get; set; }
    }
}
