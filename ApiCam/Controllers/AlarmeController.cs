using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using ApiCam.Models;

namespace ApiCam.Controllers
{
    [Route("ApiCam/[controller]")]
    [ApiController]
    [Authorize] // Cette ligne sécurise tout le contrôleur avec JWT
    public class AlarmeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AlarmeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAllAlarmes()
        {
            List<Alarme> alarmes = new List<Alarme>();

            string query = "SELECT * FROM Alarme";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            Alarme alarme = new Alarme();
                            alarme.ID = Convert.ToInt32(myReader["ID"]);
                            alarme.Camera = Convert.ToInt32(myReader["Camera"]);
                            alarme.DateHeure = Convert.ToDateTime(myReader["DateHeure"]);
                            alarme.Type = myReader["Type"].ToString();
                            alarme.Niveau = myReader["Niveau"].ToString();
                            alarme.Statut = myReader["Statut"].ToString();
                            alarme.Description = myReader["Description"].ToString();
                            
                            alarmes.Add(alarme);
                        }
                    }
                }
            }

            return Ok(alarmes);
        }

        [HttpPost]
        public IActionResult CreateAlarme(Alarme alarme)
        {
            string query = @"INSERT INTO Alarme (Camera, DateHeure, Type, Niveau, Statut, Description) 
                             VALUES (@Camera, @DateHeure, @Type, @Niveau, @Statut, @Description)";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Camera", alarme.Camera);
                    myCommand.Parameters.AddWithValue("@DateHeure", alarme.DateHeure);
                    myCommand.Parameters.AddWithValue("@Type", alarme.Type);
                    myCommand.Parameters.AddWithValue("@Niveau", alarme.Niveau);
                    myCommand.Parameters.AddWithValue("@Statut", alarme.Statut);
                    myCommand.Parameters.AddWithValue("@Description", alarme.Description);

                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
            }

            return Ok("Alarme créée avec succès");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAlarme(int id)
        {
            string query = "DELETE FROM Alarme WHERE ID = @Id";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myConn.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok("Alarme supprimée avec succès");
                    }
                    else
                    {
                        return NotFound("Failed to delete Alarme");
                    }
                }
            }
        }
    }
}
