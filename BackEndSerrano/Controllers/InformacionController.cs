using BackEndSerrano.Servicio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEndSerrano.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class InformacionController : ControllerBase
    {
        readonly ProductoServicio _productoServicio;

        public InformacionController(IConfiguration configuration)
        {
            _productoServicio = new ProductoServicio(configuration);
        }

        #region metodos

        [HttpGet("Proveedores")]
        public async Task<IActionResult> GetProveedore()
        {
            try
            {
                var result = await _productoServicio.GetProveedores();
                if (result == null) {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "No se encontro registro!" });
                }

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("ProveedoresMarca")]
        public async Task<IActionResult> GetProveedoresMArca(string IDProveedor)
        {
            try
            {
                var result=await _productoServicio.GetProveedorMarca(IDProveedor);
                if (result == null) {
                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontro registro" });
                }
                return StatusCode(StatusCodes.Status200OK, result);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }


        #endregion


    }
}