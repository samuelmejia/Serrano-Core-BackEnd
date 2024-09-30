namespace BackEndSerrano.Model.Levantamiento
{
    public class LevantamientoDetalleModel
    {
        public int IDLevantamientoProducto { get; set; }
        public string IDTiendas { get; set; }
        public decimal Disponible { get; set; }
        public decimal Encontrado { get; set; }
        public int IDEstado { get; set; }
        public dynamic FechaHoraDetalle { get; set; }
    }
}
