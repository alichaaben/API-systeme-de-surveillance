using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using ApiCam.Models;

namespace ApiCam.Controllers
{
    [Route("ApiCam/[controller]")]
    [ApiController]
    [Authorize] 
    public class UtilisateurController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UtilisateurController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       
        [HttpGet]
        public IActionResult GetAllUtilisateurs()
        {
            List<Utilisateur> utilisateurs = new List<Utilisateur>();
            string query = "SELECT * FROM Utilisateur";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myConn.Open();
                    using (var reader = myCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            utilisateurs.Add(new Utilisateur
                            {
                                Id = reader["ID"].ToString(),
                                Nom_utilisateur = reader["Nom_utilisateur"].ToString(),
                                Prenom = reader["Prenom"].ToString(),
                                Email = reader["Email"].ToString(),
                                Mot_de_passe = reader["Mot_de_passe"].ToString(),
                                Actif = reader["Actif"].ToString(),
                                Locale = reader["Locale"].ToString(),
                                Role = reader["Role"].ToString()
                            });
                        }
                    }
                }
            }

            return Ok(utilisateurs);
        }

        
        [HttpGet("{id}")]
        public IActionResult GetUtilisateurById(string id)
        {
            Utilisateur utilisateur = new Utilisateur();
            string query = "SELECT * FROM Utilisateur WHERE ID = @Id";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.Read())
                        {
                            utilisateur.Id = myReader["ID"].ToString();
                            utilisateur.Nom_utilisateur = myReader["Nom_utilisateur"].ToString();
                            utilisateur.Prenom = myReader["Prenom"].ToString();
                            utilisateur.Email = myReader["Email"].ToString();
                            utilisateur.Mot_de_passe = myReader["Mot_de_passe"].ToString();
                            utilisateur.Actif = myReader["Actif"].ToString();
                            utilisateur.Locale = myReader["Locale"].ToString();
                            utilisateur.Role = myReader["Role"].ToString();
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }

            return Ok(utilisateur);
        }

       
        [HttpPost]
        public IActionResult CreateUtilisateur([FromBody] Utilisateur utilisateur)
        {
            string query = "INSERT INTO Utilisateur (Nom_utilisateur, Prenom, Email, Mot_de_passe, Actif, Locale, Role) VALUES (@Nom_utilisateur, @Prenom, @Email, @Mot_de_passe, @Actif, @Locale, @Role)";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Nom_utilisateur", utilisateur.Nom_utilisateur);
                    myCommand.Parameters.AddWithValue("@Prenom", utilisateur.Prenom);
                    myCommand.Parameters.AddWithValue("@Email", utilisateur.Email);
                    myCommand.Parameters.AddWithValue("@Mot_de_passe", utilisateur.Mot_de_passe);
                    myCommand.Parameters.AddWithValue("@Actif", utilisateur.Actif);
                    myCommand.Parameters.AddWithValue("@Locale", utilisateur.Locale);
                    myCommand.Parameters.AddWithValue("@Role", utilisateur.Role);

                    myConn.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
            }
        }

        
        [HttpPut("{id}")]
        public IActionResult UpdateUtilisateur(string id, [FromBody] Utilisateur utilisateur)
        {
            string query = "UPDATE Utilisateur SET Nom_utilisateur = @Nom_utilisateur, Prenom = @Prenom, Email = @Email, Mot_de_passe = @Mot_de_passe, Actif = @Actif, Locale = @Locale, Role = @Role WHERE ID = @Id";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Nom_utilisateur", utilisateur.Nom_utilisateur);
                    myCommand.Parameters.AddWithValue("@Prenom", utilisateur.Prenom);
                    myCommand.Parameters.AddWithValue("@Email", utilisateur.Email);
                    myCommand.Parameters.AddWithValue("@Mot_de_passe", utilisateur.Mot_de_passe);
                    myCommand.Parameters.AddWithValue("@Actif", utilisateur.Actif);
                    myCommand.Parameters.AddWithValue("@Locale", utilisateur.Locale);
                    myCommand.Parameters.AddWithValue("@Role", utilisateur.Role);
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myConn.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }

        
        [HttpDelete("{id}")]
        public IActionResult DeleteUtilisateur(string id)
        {
            string query = "DELETE FROM Utilisateur WHERE ID = @Id";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myConn.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
        }
    }
}
