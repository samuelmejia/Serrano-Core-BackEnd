using BackEndSerrano.ConexionDB;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
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
        public async Task<string> GenerarToken(UserConnected usuario)
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

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration.GetSection("JWT:ExpirationMinutes").Value ?? "5")),
                signingCredentials: credenciales
            );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return await Task.FromResult(token);
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

                UserConnected usu = dapper.QuerySingleOrDefault<UserConnected>(sql, new { correo = authenticate.Correo }, commandTimeout: 100, commandType: CommandType.Text);
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
                string sql = "[dbo].[PI_HistoricoToken]";
                var mensaje = dapper.QuerySingle<string>(sql, new
                {
                    IdUsuario = Id,
                    Token = Token,
                    RefrescaToken = RefreshToken,
                    FechaCreacion = DateTime.Now,
                    FechaExpiracion = DateTime.Now.AddMinutes(2),
                });

                return await Task.FromResult(mensaje);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally { dapper.Close(); }
        }

        public RefrescaToken DevolverToken(RefrescaToken refresca, int idUsuario)
        {
            try
            {
                dapper.Open();
                string sql = "execute [dbo].[PS_TokenExpirado]@tokenExpirado,@refrescaToken,@idUsuario";
                var refre = dapper.Query<HistorialRefrescaToken>(sql, new { tokenExpirado = refresca.Token, refrescaToken = refresca.RefreshToke, idUsuario = idUsuario });
                if (refre is null)
                {
                    var mensaje = new RefrescaToken
                    {
                        IdUsuario = idUsuario,
                        Token = null,
                        RefreshToke = null,
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
                        RefreshToke = null,
                        msg = "El Token no ha Expirado"
                    };
                    return mensaje;
                }

                sql = "select " +
                       "* " +
                      "from dbo.VW_UsuarioConectado where Id=@id";
                var usuarioConectado = dapper.QuerySingleOrDefault<UserConnected>(sql, new { id = idUsuario }, commandTimeout: 100, commandType: CommandType.Text);

                var token = GenerarToken(usuarioConectado);
                var refreshToken = CreateRandomToken();

                var RefreshToken = new RefrescaToken
                {
                    IdUsuario = idUsuario,
                    Token = token.Result,
                    RefreshToke = refreshToken,
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
        #endregion
    }
}
