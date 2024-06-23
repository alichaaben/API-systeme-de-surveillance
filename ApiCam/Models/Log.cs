namespace ApiCam.Models
{
    public class Log
    {
        public int ID { get; set; }
        public int Utilisateur { get; set; }
        public int Camera { get; set; }
        public string Action { get; set; }
        public DateTime DateHeure { get; set; }
        public string Type { get; set; }
        public string Statut { get; set; }
    }
}