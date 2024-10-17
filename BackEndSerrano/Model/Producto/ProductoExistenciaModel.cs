namespace BackEndSerrano.Model.Producto
{
    public class ProductoExistenciaModel
    {
        public string ID { get; set; }
        public string tndID { get; set; }
        public string tndNombre { get; set; }
        public decimal Reservado { get; set; }
        public decimal EnTransito { get; set; }
        public decimal Confirmado { get; set; }
        public decimal Existencia { get; set; }
        public decimal Disponible { get; set; }
        public decimal PrdStockMinimo { get; set; }
        public decimal PrdStockMaximo { get;set; }
        public decimal PrdCosto { get; set; }

        public decimal PrdPreUni { get; set; }
    }
}
