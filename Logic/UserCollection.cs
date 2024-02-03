using Interface;
using Logic.Mapper;

namespace Logic;

public class UserCollection
{
    public List<User> Users { get; set; }
    private IUserDAL _userDalInterface;

    public UserCollection()
    {
        Users = new List<User>();
        _userDalInterface = Factory.DalFactory.GetUserDal();
    }

    public User GetUser(string username)
    {
        return _userDalInterface.GetUser(username).ConvertToLogic();
    }
}