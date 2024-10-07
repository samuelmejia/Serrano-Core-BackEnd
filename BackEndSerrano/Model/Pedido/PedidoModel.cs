namespace BackEndSerrano.Model.Pedido
{
    public class PedidoModel
    {
        public int ID { get; set; }
        public dynamic FechaCreacion { get; set; }
        public dynamic FechaCierre { get; set; }
        public string Descripcion { get; set; }
        public string UsuarioResponsable { get; set; }
        public string IPAddress { get; set; }
        public dynamic FechaEntrega { get; set; }
        public string Observaciones { get; set; }
        public dynamic Estado { get; set; }
        public List<PedidoProductoModel> Productos { get; set; }

    }
}
