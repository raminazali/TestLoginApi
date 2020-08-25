using System.Text.Json.Serialization;

namespace LoginExample.Models
{
    public class UserLogin
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string token { get; set; }
    }
}