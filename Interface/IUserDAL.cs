using DTO;

namespace Interface;

public interface IUserDAL
{
    UserDTO? GetUser(string username);
}