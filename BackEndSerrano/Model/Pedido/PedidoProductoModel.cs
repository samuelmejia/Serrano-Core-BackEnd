namespace BackEndSerrano.Model.Pedido
{
    public class PedidoProductoModel
    {
        public int IDPedidoProducto { get; set; }
        public string CodProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public string Marca { get; set; }
        public dynamic FechaHora { get; set; }
        public dynamic FechaEntregaProducto { get; set; }
        public decimal Cantidad { get; set; }
        public string ObservacionesProducto { get; set; }
        public int IDPedido { get; set; }
    }
}
