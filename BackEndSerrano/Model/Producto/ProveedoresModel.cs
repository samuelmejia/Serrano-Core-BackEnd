namespace BackEndSerrano.Model.Producto
{
    public class ProveedoresModel
    {
        public string IDProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string RTN { get; set; }
        public string Activo { get; set; }
    }

    public class ProveedorMarcaModel
    { 
        public string IDProveedor { get; set; }
        public int IDMarca { get; set; }
        public string NombreMarca { get; set; }

    }
}
