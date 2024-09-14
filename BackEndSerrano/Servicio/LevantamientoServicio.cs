using BackEndSerrano.ConexionDB;

namespace BackEndSerrano.Servicio
{
    public class LevantamientoServicio : ConexionDapper
    {
        readonly IConfiguration _configuration;
        #region ctor
        public LevantamientoServicio(IConfiguration configuration) : base(configuration)
        {
            _configuration= configuration;
        }
        #endregion

        #region metodos
        //public async Task<> AgregaLevantamiento()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception )
        //    {

        //        throw;
        //    }
        //}
        #endregion
    }
}
