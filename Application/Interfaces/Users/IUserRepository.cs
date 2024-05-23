using Core.Users;

namespace Application.Interfaces.Users
{
    public interface IUserRepository
    {
        User AddUser(User user);
        User GetUserById(int id);
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);
        User GetUserByPhone(string phone);
    }
}
