using BackEndSerrano.Servicio;
using Microsoft.AspNetCore.Mvc;
using static BackEndSerrano.Model.AutenticateModel;

namespace BackEndSerrano.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        AuthServicio _authServicio;
        HashServicio _hashServicio;
        public AuthController(IConfiguration configuration)
        {
            _authServicio = new(configuration);
            _hashServicio = new(configuration);
        }

        [HttpPost]
        public async Task<IActionResult> login(Authenticate autenticador)
        {
            try
            {
                var usuario = _authServicio.Login(autenticador);

                if (usuario == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new
                    {
                        message = "Credenciales incorrectas"
                    });
                }
                var token = await _hashServicio.GenerarToken(usuario);
                var usuarioTiendas =await _hashServicio.ftInfoUsuario(usuario.Usuario.ToString());


                if (token == null) 
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Error al generar token, contactar al depto. IT" });
                }
              
                return StatusCode(StatusCodes.Status200OK,new { token, usuarioTiendas });
              

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }



        }
    }
}
