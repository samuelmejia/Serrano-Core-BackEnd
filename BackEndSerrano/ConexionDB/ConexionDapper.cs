using System.Data.SqlClient;

namespace BackEndSerrano.ConexionDB
{
        public class ConexionDapper
        {
            public string cadenaConexion { get; set; } = string.Empty;

            protected SqlConnection dapper;

            public ConexionDapper(IConfiguration configuration)
            {
                cadenaConexion = configuration!.GetSection("ConnectionStrings:Cadena").Value!.ToString();
                dapper = InstanciaConexion();
            }


            private SqlConnection InstanciaConexion()
            {
                SqlConnection sqlC = new SqlConnection();
                sqlC.ConnectionString = cadenaConexion;

                return sqlC;
            }
        }
    }

