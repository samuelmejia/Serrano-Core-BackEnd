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
                //var usuario = _authServicio.Login(autenticador);

                var usuario = new UserConnected
                {
                    Id=1,
                    Nombre="Kevin",
                    apellido="Mejia",
                    Contraseña=null,
                    Correo="cgonzalez@test.com"
                };

                //if (usuario == null)
                //{
                //    return StatusCode(StatusCodes.Status404NotFound, new
                //    {
                //        mensaje = "No se encontro registro"
                //    });
                //}
                var token = _hashServicio.GenerarToken(usuario);
                var refreshToken = _hashServicio.CreateRandomToken();
                //var confirmar = await _hashServicio.GuardarToken(usuario.Id, token.Result, refreshToken);
               // if (confirmar.ToString() == "OK!")
                //{

                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        token = token.Result,
                        refreshToken,
                        usuario
                    });
               // }

              //  return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "No se logro generar el token" });

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    mensaje = "ERROR:: " + ex.Message
                });
            }



        }
    }
}
