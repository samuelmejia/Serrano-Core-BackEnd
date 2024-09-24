using BackEndSerrano.ConexionDB;
using BackEndSerrano.Model.Levantamiento;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static BackEndSerrano.Model.AutenticateModel;

namespace BackEndSerrano.Servicio
{
    public class HashServicio : ConexionDapper
    {
        readonly IConfiguration _configuration;
        #region ctor
        public HashServicio(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region meodos
        public Task<RefrescaToken> GenerarToken(UserConnected usuario)
        {
            var claims = new[]
            {
                new Claim("id", usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre!),
                new Claim(ClaimTypes.Email, usuario.Correo!),
                new Claim(ClaimTypes.AuthenticationInstant, DateTimeOffset.Now.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value ?? string.Empty));
            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //.HmacSha512Signature);
            var Expira = DateTime.Now.AddMinutes(int.Parse(_configuration.GetSection("JWT:ExpirationMinutes").Value ?? "600"));
           var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: Expira,
                signingCredentials: credenciales
            );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            string refrescaToken=CreateRandomToken();
            
            var expiraSegundo = (int.Parse(_configuration.GetSection("JWT:ExpirationMinutes").Value ?? "600")) * 60;
            return Task.FromResult(new RefrescaToken
            {
                IdUsuario = usuario.Id,
                Token = token,
                RefreshToken = refrescaToken,
                ExpiraTime = expiraSegundo,
             
            });
            
        }

        public async Task<string> HashPassword(string contra)
        {
            var passwordHasher = new PasswordHasher<IdentityUser>();
            string hasherdPassword = passwordHasher.HashPassword(new IdentityUser(), contra);

            return await Task.FromResult(hasherdPassword);
        }

        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public Boolean VerificarPass(Authenticate authenticate)
        {
            try
            {
                dapper.Open();
                string sql = "select" +
                        "*" +
                        "from USUARIO where correo=@correo";

                UserConnected usu = dapper.QuerySingleOrDefault<UserConnected>(sql, new 
                { 
                    correo = authenticate.Correo
                }, commandTimeout: 100, commandType: CommandType.Text);
                if (usu == null)
                {
                    return false;
                }
                var passwordHasher = new PasswordHasher<IdentityUser>();
                var result = passwordHasher.VerifyHashedPassword(new IdentityUser(), usu.Contraseña, authenticate.Password);
                if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return true;

                }

                return false;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<string> GuardarToken(int Id, string Token, string RefreshToken)
        {
            try
            {
                dapper.Open();
                var Expira = DateTime.Now.AddMinutes(int.Parse(_configuration.GetSection("JWT:ExpirationMinutes").Value ?? "600"));
                string sql = "[dbo].[PIHistoricoToken]";
                var mensaje = dapper.QuerySingle<string>(sql, new
                {
                    IdUsuario = Id,
                    Token = Token,
                    RefrescaToken = RefreshToken,
                    FechaCreacion = DateTime.Now,
                    FechaExpiracion = Expira,
                });

                return await Task.FromResult(mensaje);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public RefrescaToken DevolverToken(RefrescaToken refresca, string idUsuario)
        {
            try
            {
                dapper.Open();
                string sql = "execute [dbo].[PSTokenExpirado]@tokenExpirado,@refrescaToken,@idUsuario";
                var refre = dapper.Query<HistorialRefrescaToken>(sql, new 
                { 
                    tokenExpirado = refresca.Token, 
                    refrescaToken = refresca.RefreshToken,
                    idUsuario = idUsuario 
                });

                if (refre is null)
                {
                    var mensaje = new RefrescaToken
                    {
                        IdUsuario = idUsuario,
                        Token = null,
                        RefreshToken = null,
                        msg = "RefreshToken invalido"
                    };
                    return mensaje;
                }
                else if (refre.FirstOrDefault().FechaExpiracion > DateTime.Now)
                {
                    var mensaje = new RefrescaToken
                    {
                        IdUsuario = idUsuario,
                        Token = null,
                        RefreshToken = null,
                        msg = "El Token no ha Expirado"
                    };
                    return mensaje;
                }

                sql = "select " +
                       "* " +
                      "from dbo.VW_UsuarioConectado where Id=@id";
                var usuarioConectado = dapper.QuerySingleOrDefault<UserConnected>(sql, new 
                { 
                    id = idUsuario 
                }, commandTimeout: 100, commandType: CommandType.Text);

                var token = GenerarToken(usuarioConectado);
                var refreshToken = CreateRandomToken();

                var RefreshToken = new RefrescaToken
                {
                    IdUsuario = idUsuario,                 
                    RefreshToken = refreshToken,
                    msg = "El Token fue actualizado"
                };

                return RefreshToken;
            }
            catch (Exception)
            {

                throw;
            }
            finally { dapper.Close(); }

        }

        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ip = host.AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);

            return ip?.ToString() ?? "No se encontró la dirección IP.";
        }

        public  async Task<IEnumerable<InfoUsuarioModel>> ftInfoUsuario(string usuario)
        {
            try
            {
                dapper.Open();
                string sql = "select" +
                              "*" +
                              "from [dbo].[ftInfoUsuario](@eUsuario)";
                var result = dapper.Query<InfoUsuarioModel>(sql, new
                { 
                    eUsuario= usuario
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
