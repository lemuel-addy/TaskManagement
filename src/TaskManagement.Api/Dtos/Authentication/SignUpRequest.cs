using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Api.Dtos.Authentication;
#nullable disable
public class SignUpRequest
{
    [Required(ErrorMessage = "{0} is required")]
    public string Username { get; set; }
    
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(10, ErrorMessage = "{0} must be at most 10 characters long")]
    public string Contacts { get; set; }
    
    [Required(ErrorMessage = "{0} is required")]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character")]
    public string Password { get; set; }
}