using BackEndSerrano.Model.Levantamiento;
using BackEndSerrano.Servicio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEndSerrano.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class LevantamientoController(IConfiguration configuration) : ControllerBase
    {
        readonly LevantamientoServicio _levantamientoServicio = new(configuration);
        readonly HashServicio _hashServicio = new(configuration);

        #region metodos

        [HttpPost("ftEstados")]
        public async Task<IActionResult> EstadosLevantamiento([FromBody] EstadoLevantamientoModel estado)
        {
            try
            {
                var result = await _levantamientoServicio.ftEstados(estado);
                if (result is null) {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "No se encontro registro" });
                }
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex);
            }        
        }

        [HttpPost("StatusLevantamiento")]
        public async Task<IActionResult> StatusLevantamiento() {
            try
            {
                var requestID = User.FindFirstValue("id");
                var userName = User.Identity.Name;
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                var ipRequest = HttpContext.Connection.RemoteIpAddress!.ToString();
                if (ipRequest == "::1")
                {
                    ipRequest = _hashServicio.GetLocalIPAddress();
                }
                var encabezado = await _levantamientoServicio.Encabezado(requestID);
                if (encabezado is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontro registro!" });
                }
                return StatusCode(StatusCodes.Status200OK, encabezado);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }

        }

        [HttpGet("ListaLevantamientos")]
        public async Task<IActionResult> ListaLevantamientos(int id)
        {
            try
            {
                var encabezado = await _levantamientoServicio.ListaLevantamientos(id);
                if (encabezado is null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontro registro!" });
                }
                return StatusCode(StatusCodes.Status200OK, encabezado);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost("CreaLevantamiento")]
        public async Task<IActionResult> CreaLevantamiento()
        {
            try
            {
                var requestID = User.FindFirstValue("id");
                var userName = User.Identity.Name;

                string ipRequest = HttpContext.Connection.RemoteIpAddress!.ToString();
                if (ipRequest == "::1")
                {
                    ipRequest = _hashServicio.GetLocalIPAddress();
                }
                //var des = JsonConvert.DeserializeObject<dynamic>(Json.ToString());
                var result = await _levantamientoServicio.AgregarLevantamiento(requestID, ipRequest);

                return StatusCode(StatusCodes.Status200OK, new { message = result });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("ftLevantamientoProducto")]
        public async Task<IActionResult> GetLevantamientoProducto(int id)
        {
            try
            {
                var result = await _levantamientoServicio.LevantamientoProductoConDetalle(id);

                if (!result.Any())
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontro registro" });
                }
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPut("PULevantamiento")]
        public async Task<IActionResult> PULevantamiento([FromBody] EncabezadoLevantamientoModel encabezado)
        {
            try
            {
                var actualiza = await _levantamientoServicio.PULevantamiento(encabezado);
                return StatusCode(StatusCodes.Status200OK, new { message = actualiza });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost("PILevantamientoProducto")]
        public async Task<IActionResult> PILevantamientoProducto([FromBody] LevantamientoProductoModel progreso)
        {
            try
            {
                var result = await _levantamientoServicio.PILevantamientoProducto(progreso);
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

        [HttpDelete("PDLevantamientoProducto")]
        public async Task<IActionResult> PDLevantamientoProducto([FromBody] dynamic info)
        {
            try
            {
                var idLevantamiento = info.GetProperty("idLevantamiento").GetInt32();
                var codProducto = info.GetProperty("codProducto").GetString();

                var result = await _levantamientoServicio.PDLevantamientoProducto(idLevantamiento, codProducto);
                string encabezado = result.Split("|").GetValue(0).ToString();
                string detalle = result.Split("|").GetValue(1).ToString();
                if (encabezado == "200") {
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
        
        [HttpPost("PGGuardarProgreso")]
        public async Task<IActionResult> GuadarProgreso(List<LevantamientoProductoModel> data) {
            try
            {
                var result = await _levantamientoServicio.PGGuardarProgreso(data);
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
#endregion
    }
}
