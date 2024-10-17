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
    public class ProductoController : ControllerBase
    {
        readonly ProductoServicio _productoServicio;

        #region ctor
        public ProductoController(IConfiguration configuration)
        {

            _productoServicio = new ProductoServicio(configuration);
        }

        #endregion

        #region metodos

        [HttpGet("AllProducto")]
        public async Task<IActionResult> GetAllProducto()
        {
            try
            {
                var userName = User.Identity.Name;
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                var resultado= await _productoServicio.GetAllProducto();
                if (resultado.Count() == 0)
                {

                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontro registro" });
                }
                return StatusCode(StatusCodes.Status200OK,resultado);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest,  ex.Message );
            }
        }

        [HttpGet("Producto")]
        public async Task<IActionResult> GetProducto(string busqueda) {
            try
            {
                var resultado = await _productoServicio.GetProducto(busqueda);
                if (resultado.Count()==0) {

                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontro registro" });
                }
                return StatusCode(StatusCodes.Status200OK, resultado);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, new { ex.Message } );
            }
        }

        [HttpGet("ExistenciaProducto")]
        public async Task<IActionResult> GetExistenciaProducto(string busqueda)
        {
            try
            {
                var resultado = await _productoServicio.GetExistencias(busqueda);
                if (resultado.Count() == 0)
                {

                    return StatusCode(StatusCodes.Status404NotFound, new { message = "No se encontro registro" });
                }
                return StatusCode(StatusCodes.Status200OK, resultado);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message );
            }
        }

        #endregion
    }

}
