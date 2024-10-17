namespace BackEndSerrano.Model.Producto
{
    public class KardexModel
    {
        public int IDCategoria { get; set; }
        public string Categoria { get; set; }

        public int IDLinea { get; set; }
        public string Linea { get; set; }
        public string UnidadDeMedida { get; set; }
        public decimal Empaque { get; set; }
        public decimal Espesor { get; set; }
        public decimal Entradas { get; set; }
        public decimal Compras { get; set; }
        public decimal Salidas { get; set; }
        public decimal Ventas { get; set; }
        public decimal Saldo { get; set; }


    }

    public class DetalleKardex
    {
        public int Linea { get; set; }
        public string Bodega { get; set; }
        public string DocN { get; set; }
        public dynamic Fecha { get; set; }
        public dynamic Hora { get; set; }
        public string Tipo { get; set; }
        public string TipoKardex { get; set; }
        public string Referencia { get; set; }
        public string RTNCodigo { get; set; }
        public string Cliente { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PiezaCaja { get; set; }
        public decimal Saldo { get; set; }
    }
}
