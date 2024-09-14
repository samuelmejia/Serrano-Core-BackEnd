namespace BackEndSerrano.Model.Producto
{
    public class ProductosModel
    {
        public string ID { get; set; }
        public string Nombre { get; set; }
        public string Linea { get; set; }
        public string Categoria { get; set; }
        public string Marca { get; set; }
        public decimal  Impuesto {get;set;}
        public decimal StockTotal {get;set;}
        public dynamic FechaUltimaCompra { get; set; }
        public dynamic FechaUltimaVenta { get; set; }
        public string Estado { get; set; }

    }
}
