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
    [Authorize] 
    public class CameraController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CameraController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAllCameras()
        {
            List<Camera> cameras = new List<Camera>();
            string query = "SELECT ID, Nom, AdresseIP, Statut, Localisation FROM Camera";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            cameras.Add(new Camera
                            {
                                ID = myReader.GetInt32("ID"),
                                Nom = myReader.GetString("Nom"),
                                AdresseIP = myReader.GetString("AdresseIP"),
                                Statut = myReader.GetString("Statut"),
                                Localisation = myReader.GetString("Localisation")
                            });
                        }
                    }
                }
            }

            return Ok(cameras);
        }

        [HttpGet("{id}")]
        public IActionResult GetCameraById(int id)
        {
            Camera camera = null;
            string query = "SELECT ID, Nom, AdresseIP, Statut, Localisation FROM Camera WHERE ID = @Id";

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
                            camera = new Camera
                            {
                                ID = myReader.GetInt32("ID"),
                                Nom = myReader.GetString("Nom"),
                                AdresseIP = myReader.GetString("AdresseIP"),
                                Statut = myReader.GetString("Statut"),
                                Localisation = myReader.GetString("Localisation")
                            };
                        }
                    }
                }
            }

            if (camera == null)
            {
                return NotFound();
            }

            return Ok(camera);
        }

        [HttpPost]
        public IActionResult CreateCamera(Camera camera)
        {
            string query = "INSERT INTO Camera (Nom, AdresseIP, Statut, Localisation) VALUES (@Nom, @AdresseIP, @Statut, @Localisation)";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Nom", camera.Nom);
                    myCommand.Parameters.AddWithValue("@AdresseIP", camera.AdresseIP);
                    myCommand.Parameters.AddWithValue("@Statut", camera.Statut);
                    myCommand.Parameters.AddWithValue("@Localisation", camera.Localisation);

                    myConn.Open();
                    myCommand.ExecuteNonQuery();
                }
            }

            return CreatedAtAction(nameof(GetCameraById), new { id = camera.ID }, camera);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCamera(int id, Camera camera)
        {
            string query = "UPDATE Camera SET Nom = @Nom, AdresseIP = @AdresseIP, Statut = @Statut, Localisation = @Localisation WHERE ID = @Id";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myCommand.Parameters.AddWithValue("@Nom", camera.Nom);
                    myCommand.Parameters.AddWithValue("@AdresseIP", camera.AdresseIP);
                    myCommand.Parameters.AddWithValue("@Statut", camera.Statut);
                    myCommand.Parameters.AddWithValue("@Localisation", camera.Localisation);

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
        public IActionResult DeleteCamera(int id)
        {
            string query = "DELETE FROM Camera WHERE ID = @Id";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

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

        [HttpGet("byStatus")]
        public IActionResult GetCamerasByStatus(string status)
        {
            List<Camera> cameras = new List<Camera>();

            string query = "SELECT * FROM Camera WHERE Statut = @Status";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Status", status);

                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            Camera camera = new Camera();
                            camera.ID = Convert.ToInt32(myReader["ID"]);
                            camera.Nom = myReader["Nom"].ToString();
                            camera.AdresseIP = myReader["AdresseIP"].ToString();
                            camera.Statut = myReader["Statut"].ToString();
                            camera.Localisation = myReader["Localisation"].ToString();

                            cameras.Add(camera);
                        }
                    }
                }
            }

            return Ok(cameras);
        }
    }
}
