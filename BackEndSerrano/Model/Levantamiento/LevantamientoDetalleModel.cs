namespace BackEndSerrano.Model.Levantamiento
{
    public class LevantamientoDetalleModel
    {
        public int IDLevantamientoProducto { get; set; }
        public int IDTiendas { get; set; }
        public decimal Disponible { get; set; }
        public decimal Existencia { get; set; }
        public decimal Movimiento { get; set; }
        public decimal Encontrado { get; set; }
        public decimal Solicitado { get; set; }
        public dynamic FechaHora { get; set; }
    }
}
