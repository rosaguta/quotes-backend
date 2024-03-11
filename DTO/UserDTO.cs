using MongoDB.Bson.Serialization.Attributes;

namespace DTO;

public class UserDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Rights { get; set; }
}