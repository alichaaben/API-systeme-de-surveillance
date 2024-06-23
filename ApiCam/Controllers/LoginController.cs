using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ApiCam.Models;

namespace ApiCam.Controllers
{
    [Route("ApiCam/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login(Login login)
        {
            string query = "SELECT Id, Nom_utilisateur, Role FROM Utilisateur WHERE Nom_utilisateur = @Username AND Mot_de_passe = @Password";
            Utilisateur utilisateur = null;

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Username", login.Username);
                    myCommand.Parameters.AddWithValue("@Password", login.Password);

                    myConn.Open();
                    using (var reader = myCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            utilisateur = new Utilisateur
                            {
                                Id = reader["Id"].ToString(),
                                Nom_utilisateur = reader["Nom_utilisateur"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }
            }

            if (utilisateur == null)
            {
                return BadRequest("Nom d'utilisateur ou mot de passe incorrect !");
            }

            // Créer un token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, utilisateur.Id),
                    new Claim(ClaimTypes.Name, utilisateur.Nom_utilisateur),
                    new Claim(ClaimTypes.Role, utilisateur.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:DurationInDays"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Retourner le token JWT dans la réponse
            return Ok(new AuthenticateResponse { Id = utilisateur.Id, Email = utilisateur.Email, Token = tokenString });
        }
    }
}
