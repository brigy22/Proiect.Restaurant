namespace Restaurant.API.Models;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string SessionToken { get; set; } = "";
    public int CustomerId { get; set; }
}
