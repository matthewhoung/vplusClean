using Core.Entities;

namespace Application.Interfaces
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
