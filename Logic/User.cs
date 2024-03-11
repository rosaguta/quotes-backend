using DTO;
using Interface;

namespace Logic;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Rights { get; set; }
    private IUserDAL _userDalInterface;
    public User()
    {
        _userDalInterface = Factory.DalFactory.GetUserDal();
    }

    public bool CheckPassword()
    {
        try
        {
            UserDTO UserInBackend = _userDalInterface.GetUser(this.Username);
            if (Password == UserInBackend.Password)
            {
                return true; 
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}