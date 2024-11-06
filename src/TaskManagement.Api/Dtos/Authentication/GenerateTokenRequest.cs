namespace TaskManagement.Api.Dtos.Authentication;

public class GenerateTokenRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}