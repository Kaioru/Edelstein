using System.ComponentModel.DataAnnotations;

namespace Edelstein.Service.WebAPI.Contracts
{
    public class RegisterContract
    {
        [Required] [MinLength(4)] public string Username { get; set; }
        [Required] [MinLength(5)] public string Password { get; set; }
        [Required] [MinLength(5)] [Compare("Password")] public string ConfirmPassword { get; set; }
    }
}