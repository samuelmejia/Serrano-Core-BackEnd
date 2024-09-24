using BackEndSerrano.ConexionDB;
using Dapper;
using static BackEndSerrano.Model.AutenticateModel;
using System.Data;

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
                                 "from dbo.VW_UsuarioConectado where correo=@correo";

                    return dapper.QuerySingleOrDefault<UserConnected>(sql, new { correo = authenticate.Correo }, commandTimeout: 100, commandType: CommandType.Text);
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
