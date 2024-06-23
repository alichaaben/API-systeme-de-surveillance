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
    public class FluxVideoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FluxVideoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAllFluxVideos()
        {
            List<FluxVideo> fluxVideos = new List<FluxVideo>();

            string query = "SELECT * FROM FluxVideo";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            FluxVideo fluxVideo = new FluxVideo();
                            fluxVideo.ID = Convert.ToInt32(myReader["ID"]);
                            fluxVideo.Camera = Convert.ToInt32(myReader["Camera"]);
                            fluxVideo.URL = myReader["URL"].ToString();
                            fluxVideo.DateHeure = Convert.ToDateTime(myReader["DateHeure"]);

                            fluxVideos.Add(fluxVideo);
                        }
                    }
                }
            }

            return Ok(fluxVideos);
        }

        [HttpGet("{id}")]
        public IActionResult GetFluxVideoById(int id)
        {
            string query = "SELECT * FROM FluxVideo WHERE ID = @Id";
            FluxVideo fluxVideo = new FluxVideo();

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
                            fluxVideo.ID = Convert.ToInt32(myReader["ID"]);
                            fluxVideo.Camera = Convert.ToInt32(myReader["Camera"]);
                            fluxVideo.URL = myReader["URL"].ToString();
                            fluxVideo.DateHeure = Convert.ToDateTime(myReader["DateHeure"]);
                        }
                    }
                }
            }

            if (fluxVideo.ID == 0)
            {
                return NotFound();
            }

            return Ok(fluxVideo);
        }

        [HttpGet("by-url")]
        public IActionResult GetFluxVideoByUrl(string url)
        {
            string query = "SELECT * FROM FluxVideo WHERE URL = @Url";
            FluxVideo fluxVideo = new FluxVideo();

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Url", url);
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.Read())
                        {
                            fluxVideo.ID = Convert.ToInt32(myReader["ID"]);
                            fluxVideo.Camera = Convert.ToInt32(myReader["Camera"]);
                            fluxVideo.URL = myReader["URL"].ToString();
                            fluxVideo.DateHeure = Convert.ToDateTime(myReader["DateHeure"]);
                        }
                    }
                }
            }

            if (fluxVideo.ID == 0)
            {
                return NotFound();
            }

            return Ok(fluxVideo);
        }

        [HttpGet("by-camera")]
        public IActionResult GetFluxVideosByCamera(int cameraId)
        {
            List<FluxVideo> fluxVideos = new List<FluxVideo>();
            string query = "SELECT * FROM FluxVideo WHERE Camera = @CameraId";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@CameraId", cameraId);
                    myConn.Open();
                    using (MySqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        while (myReader.Read())
                        {
                            FluxVideo fluxVideo = new FluxVideo();
                            fluxVideo.ID = Convert.ToInt32(myReader["ID"]);
                            fluxVideo.Camera = Convert.ToInt32(myReader["Camera"]);
                            fluxVideo.URL = myReader["URL"].ToString();
                            fluxVideo.DateHeure = Convert.ToDateTime(myReader["DateHeure"]);
                            fluxVideos.Add(fluxVideo);
                        }
                    }
                }
            }

            return Ok(fluxVideos);
        }

        [HttpPost]
        public IActionResult CreateFluxVideo([FromBody] FluxVideo fluxVideo)
        {
            string query = @"INSERT INTO FluxVideo (Camera, URL, DateHeure) VALUES (@Camera, @URL, @DateHeure)";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Camera", fluxVideo.Camera);
                    myCommand.Parameters.AddWithValue("@URL", fluxVideo.URL);
                    myCommand.Parameters.AddWithValue("@DateHeure", DateTime.Now);

                    myConn.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        return Ok(fluxVideo);
                    }
                    else
                    {
                        return StatusCode(500, "Failed to create FluxVideo");
                    }
                }
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFluxVideo(int id, [FromBody] FluxVideo updatedFluxVideo)
        {
            string query = @"UPDATE FluxVideo SET Camera = @Camera, URL = @URL WHERE ID = @Id";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Camera", updatedFluxVideo.Camera);
                    myCommand.Parameters.AddWithValue("@URL", updatedFluxVideo.URL);
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myConn.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        return Ok(updatedFluxVideo);
                    }
                    else
                    {
                        return StatusCode(500, "Failed to update FluxVideo");
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFluxVideo(int id)
        {
            string query = "DELETE FROM FluxVideo WHERE ID = @Id";

            using (MySqlConnection myConn = new MySqlConnection(_configuration.GetConnectionString("CamAppCox")))
            {
                using (MySqlCommand myCommand = new MySqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);

                    myConn.Open();
                    int rowsAffected = myCommand.ExecuteNonQuery();

                    if (rowsAffected == 1)
                    {
                        return Ok();
                    }
                    else
                    {
                        return StatusCode(500, "Failed to delete FluxVideo");
                    }
                }
            }
        }
    }
}
