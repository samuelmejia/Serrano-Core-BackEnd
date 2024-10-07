using BackEndSerrano.ConexionDB;
using Dapper;
using System.Data;
using static BackEndSerrano.Model.AutenticateModel;

namespace BackEndSerrano.Servicio
{
    public class AuthServicio : ConexionDapper
    {
        HashServicio _hashServicio;
        public AuthServicio(IConfiguration configuration) : base(configuration)
        {
            _hashServicio = new HashServicio(configuration);
        }

        public UserConnected Login(Authenticate authenticate)
        {
            try
            {
                Boolean identificado = _hashServicio.VerificarPass(authenticate);

                if (identificado)
                {
                    dapper.Open();
                    string sql = "select " +
                                 "* " +
                                 "from [dbo].[ftInfoUsuario](@eUsuario)";

                    var result =dapper.QueryFirstOrDefault<UserConnected>(sql, new { eUsuario = authenticate.Usuario }, commandTimeout: 100, commandType: CommandType.Text);
                  
                
                return result;
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }
    }
}
