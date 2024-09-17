namespace BackEndSerrano.Model
{
    public class AutenticateModel
    {
        public class Authenticate
        {
            public string Correo { get; set; } = null!;

            public string Password { get; set; } = null!;

            public string key { get; set; }

        }

        public class UserConnected
        {
            public int Id { get; set; }

            public string Nombre { get; set; }

            public string apellido { get; set; }

            public string Correo { get; set; } = null!;

            public string Contraseña { get; set; } = null!;

            public int idRol { get; set; }

        }

        public class RefrescaToken
        {
            public int IdUsuario { get; set; }
            public string Token { get; set; }
            public string RefreshToke { get; set; }
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
