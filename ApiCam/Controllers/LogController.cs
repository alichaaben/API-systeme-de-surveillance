using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using ApiCam.Models;
using Microsoft.AspNetCore.Authorization;

namespace ApiCam.Controllers
{
    [Route("ApiCam/[controller]")]
    [ApiController]
    [Authorize] // Sécurise tout le contrôleur avec JWT
    public class LogController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LogController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAllLogs()
        {
            List<Log> logs = new List<Log>();
            string query = "SELECT * FROM Log";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            Log log = new Log
                            {
                                ID = Convert.ToInt32(myReader["ID"]),
                                Utilisateur = Convert.ToInt32(myReader["Utilisateur"]),
                                Camera = Convert.ToInt32(myReader["Camera"]),
                                Action = myReader["Action"].ToString(),
                                DateHeure = Convert.ToDateTime(myReader["DateHeure"]),
                                Type = myReader["Type"].ToString(),
                                Statut = myReader["Statut"].ToString()
                            };
                            logs.Add(log);
                        }
                    }
                }
            }
            return Ok(logs);
        }

        [HttpGet("{id}")]
        public IActionResult GetLogById(int id)
        {
            Log log = null;
            string query = "SELECT * FROM Log WHERE ID = @ID";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@ID", id);
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.Read())
                        {
                            log = new Log
                            {
                                ID = Convert.ToInt32(myReader["ID"]),
                                Utilisateur = Convert.ToInt32(myReader["Utilisateur"]),
                                Camera = Convert.ToInt32(myReader["Camera"]),
                                Action = myReader["Action"].ToString(),
                                DateHeure = Convert.ToDateTime(myReader["DateHeure"]),
                                Type = myReader["Type"].ToString(),
                                Statut = myReader["Statut"].ToString()
                            };
                        }
                    }
                }
            }

            if (log == null)
            {
                return NotFound();
            }

            return Ok(log);
        }

        [HttpPost]
        public IActionResult CreateLog(Log log)
        {
            string query = "INSERT INTO Log (Utilisateur, Camera, Action, DateHeure, Type, Statut) VALUES (@Utilisateur, @Camera, @Action, @DateHeure, @Type, @Statut)";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Utilisateur", log.Utilisateur);
                    myCommand.Parameters.AddWithValue("@Camera", log.Camera);
                    myCommand.Parameters.AddWithValue("@Action", log.Action);
                    myCommand.Parameters.AddWithValue("@DateHeure", log.DateHeure);
                    myCommand.Parameters.AddWithValue("@Type", log.Type);
                    myCommand.Parameters.AddWithValue("@Statut", log.Statut);

                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
            }

            return CreatedAtAction(nameof(GetLogById), new { id = log.ID }, log);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLog(int id, Log log)
        {
            string query = "UPDATE Log SET Utilisateur = @Utilisateur, Camera = @Camera, Action = @Action, DateHeure = @DateHeure, Type = @Type, Statut = @Statut WHERE ID = @ID";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@ID", id);
                    myCommand.Parameters.AddWithValue("@Utilisateur", log.Utilisateur);
                    myCommand.Parameters.AddWithValue("@Camera", log.Camera);
                    myCommand.Parameters.AddWithValue("@Action", log.Action);
                    myCommand.Parameters.AddWithValue("@DateHeure", log.DateHeure);
                    myCommand.Parameters.AddWithValue("@Type", log.Type);
                    myCommand.Parameters.AddWithValue("@Statut", log.Statut);

                    myConn.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLog(int id)
        {
            string query = "DELETE FROM Log WHERE ID = @ID";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@ID", id);

                    myConn.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound("Failed to delete Log");
                    }
                }
            }

            return NoContent();
        }

        [HttpGet("ByUtilisateur/{utilisateurId}")]
        public IActionResult GetLogByUtilisateur(int utilisateurId)
        {
            List<Log> logs = new List<Log>();
            string query = "SELECT * FROM Log WHERE Utilisateur = @Utilisateur";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Utilisateur", utilisateurId);
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            Log log = new Log
                            {
                                ID = Convert.ToInt32(myReader["ID"]),
                                Utilisateur = Convert.ToInt32(myReader["Utilisateur"]),
                                Camera = Convert.ToInt32(myReader["Camera"]),
                                Action = myReader["Action"].ToString(),
                                DateHeure = Convert.ToDateTime(myReader["DateHeure"]),
                                Type = myReader["Type"].ToString(),
                                Statut = myReader["Statut"].ToString()
                            };
                            logs.Add(log);
                        }
                    }
                }
            }

            return Ok(logs);
        }

        [HttpGet("ByCamera/{cameraId}")]
        public IActionResult GetLogByCamera(int cameraId)
        {
            List<Log> logs = new List<Log>();
            string query = "SELECT * FROM Log WHERE Camera = @Camera";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Camera", cameraId);
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            Log log = new Log
                            {
                                ID = Convert.ToInt32(myReader["ID"]),
                                Utilisateur = Convert.ToInt32(myReader["Utilisateur"]),
                                Camera = Convert.ToInt32(myReader["Camera"]),
                                Action = myReader["Action"].ToString(),
                                DateHeure = Convert.ToDateTime(myReader["DateHeure"]),
                                Type = myReader["Type"].ToString(),
                                Statut = myReader["Statut"].ToString()
                            };
                            logs.Add(log);
                        }
                    }
                }
            }

            return Ok(logs);
        }

        [HttpGet("ByDate/{date}")]
        public IActionResult GetLogByDate(DateTime date)
        {
            List<Log> logs = new List<Log>();
            string query = "SELECT * FROM Log WHERE DATE(DateHeure) = @Date";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            Log log = new Log
                            {
                                ID = Convert.ToInt32(myReader["ID"]),
                                Utilisateur = Convert.ToInt32(myReader["Utilisateur"]),
                                Camera = Convert.ToInt32(myReader["Camera"]),
                                Action = myReader["Action"].ToString(),
                                DateHeure = Convert.ToDateTime(myReader["DateHeure"]),
                                Type = myReader["Type"].ToString(),
                                Statut = myReader["Statut"].ToString()
                            };
                            logs.Add(log);
                        }
                    }
                }
            }

            return Ok(logs);
        }
    }
}
