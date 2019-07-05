using System.ComponentModel.DataAnnotations;

namespace Edelstein.Service.WebAPI.Types
{
    public class RegisterInput
    {
        [Required] [MinLength(4)] public string Username { get; set; }
        [Required] [MinLength(5)] public string Password { get; set; }
    }
}