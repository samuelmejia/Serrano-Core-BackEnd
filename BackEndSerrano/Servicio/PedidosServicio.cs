using BackEndSerrano.ConexionDB;
using BackEndSerrano.Model.Pedido;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace BackEndSerrano.Servicio
{
    public class PedidosServicio(IConfiguration configuration) : ConexionDapper(configuration)
    {

        readonly IConfiguration _configuration = configuration;

       
        #region metodos
        public async Task<IEnumerable<PedidoModel>> ListaPedidos(string descripcion, int idEstado) {
            try
            {
                dapper.Open();
                string sql="select " +
                    "* " +
                    "FROM [dbo].[ftPedidos](@eDescripcion, @eIdEstado)";
                var reslt = dapper.Query<PedidoModel>(sql, new
                {
                    eDescripcion=descripcion,
                    eIdEstado=idEstado

                });

                return await Task.FromResult(reslt);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<IEnumerable<PedidoModel>> ListaPedidoProducto(int idPedido) 
        {
            try
            {
                dapper.Open();
                var pedidos = new Dictionary<int, PedidoModel>();
                string sql = "select " +
                               "* " +
                              "from [dbo].[ftPedidosProducto](@eID)";
                var result = dapper.Query<PedidoModel, PedidoProductoModel, PedidoModel>(sql, (pedido, detalle) =>
                {

                    if (!pedidos.TryGetValue(pedido.ID, out var lev))
                    {
                        lev = pedido;
                        lev.Productos = [];
                        pedidos.Add(lev.ID, lev);
                    }
                    if (detalle != null)
                    {
                        lev.Productos.Add(detalle);
                    }
                    return lev;
                }, new { eID = idPedido }, splitOn: "IDPedidoProducto"

                ).Distinct().ToList();

                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<string> PostPedidoCopiarLevantamiento(int idLevantamiento, string usuarioResponsable, string ipAddress)
        {
            try
            {
                dapper.Open();
                string sql = "[dbo].[PIPedidoCopiaLevantamiento]";
                var result = dapper.QuerySingle<string>(sql, new
                {
                    eIDLevantamiento = idLevantamiento,
                    eUsuarioResponsable= usuarioResponsable,
                    eIPAddress = ipAddress
                });
                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            } finally{dapper.Close(); }
        }

        public async Task<string> PostPedidoG(int idPedido, string descripcion, DateTime fechaEntrega, string usuarioResponsable, string ipAddress)
        {
            try
            {
                dapper.Open();
                string sql = "[dbo].[PGGuardarPedido]";
                var result = dapper.QuerySingle<string>(sql, new 
                {
                    eIDPedido           =idPedido,
                    eDescripcion        = descripcion,
                    eFechaEntrega       = fechaEntrega,
                    eUsuarioResponsable = usuarioResponsable,
                    eIpAddress          = ipAddress
                });
                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }finally { dapper.Close(); }
        }

        public async Task<string> PostProductosPedido(PedidoProductoModel pedidoProducto)
        {
            try
            {
              
                dapper.Open();
                string sql = "[dbo].[PGGuardarPedidoProducto]";
                var result = dapper.QuerySingle<string>(sql, new
                {
                    eCodProducto            =pedidoProducto.CodProducto,
                    eDescripcionProducto    =pedidoProducto.DescripcionProducto,
                    eMarca                  =pedidoProducto.Marca,
                    eFechaEntrega           =Convert.ToDateTime(pedidoProducto.FechaEntregaProducto.GetString()),
                    eCantidad               =pedidoProducto.Cantidad,
                    eObservaciones          =pedidoProducto.ObservacionesProducto,
                    eIDPedido               =pedidoProducto.IDPedido
                });
                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<string> DeleteProductoPedido(int idPedido, string codigoProducto)
        {
            try
            {
                dapper.Open();
                string sql = "[dbo].[PDPedidoProducto]";
                var result = dapper.QuerySingle<string>(sql, new
                {
                    eIDPedido=idPedido,
                    eCodProducto=codigoProducto
                });

                return await Task.FromResult(result);
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public async Task<string> UpdatePedido(PedidoModel pedido)
        {
            try
            {
                dapper.Open();
                string sql = "[dbo].[PUPedido]";
                var result = dapper.QuerySingle<string>(sql, new 
                {
                    eID             =pedido.ID,
                    eObservaciones  = pedido.Observaciones,
                    eFechaEntrega   =Convert.ToDateTime(pedido.FechaEntrega.GetString()),
                    eIDEstado       =pedido.Estado.GetInt32(),
                });

                return await Task.FromResult(result);
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
