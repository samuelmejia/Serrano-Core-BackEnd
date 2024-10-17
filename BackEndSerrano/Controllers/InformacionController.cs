using BackEndSerrano.Model.Producto;
using BackEndSerrano.Servicio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BackEndSerrano.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    //[AllowAnonymous]
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
                var result = await _productoServicio.GetProveedorMarca(IDProveedor);
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


        [HttpPost("Productos")]
        public async Task<IActionResult> ProductosProveedorMarcaModelo(ProductosProMarModel json)
        {
            try
            {
                if (string.IsNullOrEmpty(json.IDProveedor) && string.IsNullOrEmpty(json.IDMarca) && string.IsNullOrEmpty(json.Modelo))
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Filtros vacios (minimo uno debe contener información)." });
                }
                var result = await _productoServicio.PSCargaInfo(json);

                if (result.Count() > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "No se encontro registro!" });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("EncabezadoKardex")]
        public async Task<IActionResult> EncabezadoKardex(string CodProducto)
        {
            try
            {
                if (string.IsNullOrEmpty(CodProducto))return StatusCode(StatusCodes.Status400BadRequest, new { message = "Se espera el parametro CodProducto!" });

                var result=await _productoServicio.GetKardexEncabezado(CodProducto);

                if (result is null)return StatusCode(StatusCodes.Status400BadRequest, new { message = "No se encontro registro!" });
                
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpGet("DetalleKardex")]
        public async Task<IActionResult> DetalleKardex(string CodProducto)
        {
            try
            {
                if (string.IsNullOrEmpty(CodProducto)) return StatusCode(StatusCodes.Status400BadRequest, new { message = "Se espera el parametro CodProducto!" });

                var result = await _productoServicio.GetKardexDetalle(CodProducto);

                if (result.Count()==0) return StatusCode(StatusCodes.Status400BadRequest, new { message = "No se encontro registro!" });

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