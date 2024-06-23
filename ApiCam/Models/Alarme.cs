using System;

namespace ApiCam.Models
{
    public class Alarme
    {
        public int ID { get; set; }
        public int Camera { get; set; }
        public DateTime DateHeure { get; set; }
        public string Type { get; set; }
        public string Niveau { get; set; }
        public string Statut { get; set; }
        public string Description { get; set; }
    }
}