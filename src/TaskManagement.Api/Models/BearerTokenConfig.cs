namespace TaskManagement.Api.Models;
#nullable disable
public class BearerTokenConfig
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SigningKey { get; set; }

}