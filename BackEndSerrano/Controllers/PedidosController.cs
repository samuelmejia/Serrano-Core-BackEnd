using BackEndSerrano.Model.Pedido;
using BackEndSerrano.Servicio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEndSerrano.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    //[AllowAnonymous]
    public class PedidosController(IConfiguration configuration) : ControllerBase
    {
        readonly PedidosServicio _pedidosServicio = new(configuration);
        readonly HashServicio _hashServicio = new(configuration);

        [HttpGet]
        public async Task<IActionResult> Pedidos(string descripcion, int idEstado)
        {
            try
            {
                if (descripcion == null)
                {
                    descripcion = "";
                }
                var result = await _pedidosServicio.ListaPedidos(descripcion, idEstado);
                if (!result.Any()) {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontro registro!" });
                }
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("get")]
        public async Task<IActionResult> PedidosConProducto(int idPedido)
        {
            try
            {
                var result = await _pedidosServicio.ListaPedidoProducto(idPedido);
                if (result is null) {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontro registro" });
                }
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost("copiarLevantamiento")]
        public async Task<IActionResult> CopiarLevantamiento([FromBody] dynamic idLevantamiento)
        {
            try
            {
                var requestID = User.FindFirstValue("id");
                var ipRequest = HttpContext.Connection.RemoteIpAddress!.ToString();
                if (ipRequest == "::1")
                {
                    ipRequest = _hashServicio.GetLocalIPAddress();
                }

                var result = await _pedidosServicio.PostPedidoCopiarLevantamiento(idLevantamiento.GetProperty("idLevantamiento").GetInt32(), requestID, ipRequest);
                string encabezado = result.Split("|").GetValue(0).ToString();
                string detalle = result.Split("|").GetValue(1).ToString();
                if (encabezado == "200")
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = detalle });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = detalle });
                }

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostPedido([FromBody] dynamic json)
        {
            try
            {

                int idPedido = json.GetProperty("idPedido").GetInt32();
                string descripcion = json.GetProperty("descripcion").GetString();
                DateTime fechaEntrega = json.GetProperty("fechaEntrega").GetDateTime();
                var requestID = User.FindFirstValue("id");
                var ipRequest = HttpContext.Connection.RemoteIpAddress!.ToString();
                if (ipRequest == "::1")
                {
                    ipRequest = _hashServicio.GetLocalIPAddress();
                }

                var result = await _pedidosServicio.PostPedidoG(idPedido, descripcion, fechaEntrega, requestID, ipRequest);
                string encabezado = result.Split("|").GetValue(0).ToString();
                string detalle = result.Split("|").GetValue(1).ToString();
                if (encabezado == "200")
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = detalle });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = detalle });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> PostPedidoProducto([FromBody] PedidoProductoModel pedidoProducto)
        {
            try
            {
                var result = await _pedidosServicio.PostProductosPedido(pedidoProducto); //2024-10-07T03:44:41.370Z
                string encabezado = result.Split("|").GetValue(0).ToString();
                string detalle = result.Split("|").GetValue(1).ToString();
                if (encabezado == "200")
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = detalle });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = detalle });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> DeletePedidoProducto([FromBody] dynamic json)
        {
            try
            {
                int idPedido = json.GetProperty("idPedido").GetInt32();
                string codProducto = json.GetProperty("codProducto").GetString();
                var result = await _pedidosServicio.DeleteProductoPedido(idPedido, codProducto);
                string encabezado = result.Split("|").GetValue(0).ToString();
                string detalle = result.Split("|").GetValue(1).ToString();
                if (encabezado == "200")
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = detalle });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = detalle });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutPedido([FromBody] PedidoModel pedido) 
        {
            try
            {  
                var result=await _pedidosServicio.UpdatePedido(pedido);
                string encabezado = result.Split("|").GetValue(0).ToString();
                string detalle = result.Split("|").GetValue(1).ToString();
                if (encabezado == "200")
                {
                    return StatusCode(StatusCodes.Status200OK, new { message = detalle });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = detalle });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


    }
}
