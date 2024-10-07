using BackEndSerrano.ConexionDB;
using BackEndSerrano.Model.Levantamiento;
using Dapper;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IEnumerable<EstadoLevantamientoModel>> ftEstados(EstadoLevantamientoModel estado)
        {
            try
            {
                dapper.Open();
                var sql = "select" +
                    "*" +
                    "from ftEstados(@eIDEtapa,@eProceso)";
                var result = dapper.Query<EstadoLevantamientoModel>(sql, new 
                {
                    eIDEtapa=estado.IDEtapa,
                    eProceso=estado.Proceso
                });

                return  await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }
        public  async Task<EncabezadoLevantamientoModel> Encabezado( string usuario) {
            try
            {
                dapper.Open();
                var sql = "select top 1 ID" +
                    ",FechaCreacion" +
                    ",FechaCierre" +
                    ",UsuarioResponsable" +
                    ",IPAddress" +
                    ",Area" +
                    ",Pasillo" +
                    ",Observaciones" +
                    ",Estado" +
                    " from [dbo].[ftLevantamiento](@usuario) where IDEstado in (1,2)";

                return await Task.FromResult(dapper.QuerySingleOrDefault<EncabezadoLevantamientoModel>(sql, new 
                {
                    usuario=usuario
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally { dapper.Close();}
        }

        public async Task<IEnumerable<EncabezadoLevantamientoModel>> ListaLevantamientos(int id)
        {
            try
            {
                dapper.Open();
                var sql = "select ID" +
                    ",FechaCreacion" +
                    ",FechaCierre" +
                    ",UsuarioResponsable" +
                    ",IPAddress" +
                    ",Area" +
                    ",Pasillo" +
                    ",Observaciones" +
                    ",Estado" +
                    " from [dbo].[ftLevantamiento](@id)";

                return await Task.FromResult(dapper.Query<EncabezadoLevantamientoModel>(sql, new
                {
                    id = id
                }));
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<IEnumerable<LevantamientoProductoModel>> ftLevantamientoProducto(int id)
        {
            try
            {
                dapper.Open();
                string sql = "select" +
                            "*" +
                            "from [dbo].[ftLevantamientoProducto](@id)";
                return await Task.FromResult(dapper.Query<LevantamientoProductoModel>(sql, new 
                { 
                    id = id 
                }));

            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<string> AgregarLevantamiento(string usuarioResponsable, string ipAddress)
        {
            try
            {
                dapper.Open();
                string sql = "[dbo].[PILevantamiento]";

                var result=dapper.QuerySingle<string>(sql, new 
                {
                    UsuarioResponsable = usuarioResponsable, 
                    IPAddress=ipAddress 
                });

                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<string> PULevantamiento(EncabezadoLevantamientoModel encabezado)
        {
            try
            {              
                dapper.Open();
                string sql = "[dbo].[PULevantamiento]";
                var result = dapper.QuerySingle<string>(sql, new
                { 
                    ID              =   encabezado.ID,
                    Area            =   encabezado.Area,
                    Pasillo         =   encabezado.Pasillo,
                    Observaciones   =   encabezado.Observaciones,
                    IDEstado        =   encabezado.Estado.GetInt32(),
                });
                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<string> PILevantamientoProducto(LevantamientoProductoModel levantamineto)
        {
            try
            {
                dapper.Open();

                    string sql = "[dbo].[PILevantamientoProducto]";
                    var result = dapper.QuerySingle<string>(sql, new
                    {
                        CODProducto     = levantamineto.CODProducto,
                        Descripcion     = levantamineto.Descripcion,
                        IDLevantamiento = levantamineto.IDLevantamiento,
                       
                    });                  
                    
               
                
                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<string> PDLevantamientoProducto(int idLevantamiento, string codProducto)
        {
            try
            {
                dapper.Open();
                string sql = "[dbo].[PDLevantamientoProducto]";
                var result = dapper.QuerySingle<string>(sql,new 
                {
                    IDLevantamiento =idLevantamiento,
                    CODProducto     =codProducto,
                });
                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<IEnumerable<LevantamientoProductoModel>> LevantamientoProductoConDetalle(int id) {
            try
            {

                dapper.Open();
                var levantamientoProducto = new Dictionary<int, LevantamientoProductoModel>();
                string sql = "select" +
                               "*" +
                              "from [dbo].[ftLevantamientoProducto](@id)";
                var result = dapper.Query<LevantamientoProductoModel, LevantamientoDetalleModel, LevantamientoProductoModel>(sql, (levantamiento, detalle) =>
                {

                    if (!levantamientoProducto.TryGetValue(levantamiento.ID, out var lev))
                    {
                        lev = levantamiento;
                        lev.LevantamientoDetalle = new();
                        levantamientoProducto.Add(lev.ID, lev);
                    }
                    if (detalle != null)
                    {
                        lev.LevantamientoDetalle.Add(detalle);
                    }
                    return lev;
                }, new {id=id },splitOn: "IDLevantamientoProducto"

                ).Distinct().ToList();

                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<string> PGGuardarProgreso(List<LevantamientoProductoModel> data)
        {
            try
            {
                dapper.Open();
                var mensaje = "";
                int iterador;
                string sql = "[dbo].[PGGuardarProgreso]";
                foreach (var producto in data)
                {
                    iterador = 0;
                    foreach (var detalle in producto.LevantamientoDetalle)
                    {

                         mensaje = dapper.QuerySingle<string>(sql, new
                        {
                            eIDLevantamiento = producto.IDLevantamiento,
                            eCODProducto = producto.CODProducto,
                            eObservaciones = producto.Observaciones,
                            eIDTiendas = detalle.IDTiendas,
                            eDisponible = detalle.Disponible,
                            eEncontrado = detalle.Encontrado,
                            eIDEstado = detalle.IDEstado,
                            eIterador = iterador,
                        });
                        iterador++; 
                    }
                };
                return await Task.FromResult(mensaje);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }

        }
        #endregion
    }
}
