using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BackEndSerrano.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class LevantamientoController : ControllerBase
    {
        public LevantamientoController()
        {
        }

        [HttpPost("Levantamiento")]
        public async Task<IActionResult> AgregaLevantamiento([FromBody] string CodProducto)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { message = "Exito!" });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        [HttpGet("GetLevantamiento")]
        public async Task<IActionResult> SeleccionaLevantamiento()
        {

            try
            {
                return StatusCode(StatusCodes.Status200OK, new {message="Exito!" });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPost("InsetLevantamiento")]
        public async Task<IActionResult> AgregaLevantamientoProducto([FromBody] object Json)
        {
            try
            {
                var des = JsonConvert.DeserializeObject<dynamic>(Json.ToString());
                
                return StatusCode(StatusCodes.Status200OK, Json);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        [HttpGet("GetLevantamientoDetalle")]
        public async Task<IActionResult> LevantamientoDetalle(int IdLevantamiento)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, new { message="Exito!" });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}
