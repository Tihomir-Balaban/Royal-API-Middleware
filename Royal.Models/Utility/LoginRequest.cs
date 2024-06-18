using System.ComponentModel.DataAnnotations;

namespace FilmForge.Models.Utility;

public sealed class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}