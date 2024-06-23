namespace ApiCam.Models
{
    public class Utilisateur
    {
        public string Id { get; set; }
        public string Nom_utilisateur { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Mot_de_passe { get; set; }
        public string? Actif { get; set; }
        public string Locale { get; set; }
        public string Role { get; set; }
    }
    
    /*
{
  "sub": "502111591099515",
  "name": "Ali chaabane",
  "iat": 50211159
}*/
}