namespace BackEndSerrano.Model
{
    public class AutenticateModel
    {
        public class Authenticate
        {
            public string Usuario { get; set; } = null!;

            public string Password { get; set; } = null!;

        }

        public class UserConnected
        {
            public string Id { get; set; }

            public string Nombre { get; set; }

            public string Usuario { get; set; }

            public string Correo { get; set; } = null!;

            public string Contraseña { get; set; } = null!;

            public int idRol { get; set; }

        }

        public class RefrescaToken
        {
            public string IdUsuario { get; set; }
            public string Nombre { get; set; }
            public string Token { get; set; }
            public string RefreshToken { get; set; }
            public dynamic ExpiraTime { get; set; }
            public string msg { get; set; }
        }

        public class HistorialRefrescaToken
        {
            public int IdHistorialToken { get; set; }
            public int IdUsuario { get; set; }
            public string Token { get; set; }
            public string RefrescaToken { get; set; }
            public DateTime FechaCreacion { get; set; }
            public DateTime FechaExpiracion { get; set; }
            public dynamic EsActivo { get; set; }
        }
    }
}
