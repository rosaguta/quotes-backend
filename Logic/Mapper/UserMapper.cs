using DTO;

namespace Logic.Mapper;

public static class UserMapper
{
    public static User ConvertToLogic(this UserDTO userDto)
    {
        return new User()
        {
            Username = userDto.Username,
            Password = userDto.Password,
            Rights = userDto.Rights
        };
    }

    public static UserDTO ConvertToDTO(this User user)
    {
        return new UserDTO()
        {
            Username = user.Username,
            Password = user.Password,
            Rights = user.Rights
        };
    }
}