using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PoolServer.Models
{
    public class RegisterRequest
    {
        [Required] 
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required] 
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required] 
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [Required] 
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}