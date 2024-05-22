using Application.Interfaces.Users;
using Core.Entities;
using Dapper;
using System.Data;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public User AddUser(User user)
        {
            _dbConnection.Open();
            using (var transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var sqlcommand = @"
                        INSERT INTO users (user_name, email, phone, password_hash)
                        VALUES (@Username, @Email, @Phone, @PasswordHash)";
                     var userId = _dbConnection.ExecuteScalar<int>(sqlcommand, user, transaction);
                    user.Id = userId;

                    transaction.Commit();
                    return user;
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    _dbConnection.Close();
                }
            }
        }

        public User GetUserByEmail(string email)
        {
            var sqlcommand = $@"
                SELECT
                id AS {nameof(User.Id)},
                user_name AS {nameof(User.Username)},
                email AS {nameof(User.Email)},
                phone AS {nameof(User.Phone)},
                password_hash AS {nameof(User.PasswordHash)}
                FROM users
                WHERE email = @Email";
            var getUserByEmail = _dbConnection.QueryFirstOrDefault<User>(sqlcommand, new { Email = email });
            return getUserByEmail;
        }

        public User GetUserByPhone(string phone)
        {
            var sqlcommand = $@"
                SELECT
                id AS {nameof(User.Id)},
                user_name AS {nameof(User.Username)},
                email AS {nameof(User.Email)},
                phone AS {nameof(User.Phone)},
                password_hash AS {nameof(User.PasswordHash)}
                FROM users
                WHERE phone = @Phone";
            var getUserByPhone = _dbConnection.QueryFirstOrDefault<User>(sqlcommand, new { Phone = phone });
            return getUserByPhone;
        }

        public User GetUserById(int id)
        {
            var sqlcommand = $@"
                SELECT
                id AS {nameof(User.Id)},
                user_name AS {nameof(User.Username)},
                email AS {nameof(User.Email)},
                phone AS {nameof(User.Phone)},
                password_hash AS {nameof(User.PasswordHash)}
                FROM users
                WHERE id = @Id";
            var getUserById = _dbConnection.QueryFirstOrDefault<User>(sqlcommand, new { Id = id });
            return getUserById;
        }

        public User GetUserByUsername(string username)
        {
            var sqlcommand = $@"
                SELECT
                id AS {nameof(User.Id)},
                user_name AS {nameof(User.Username)},
                email AS {nameof(User.Email)},
                phone AS {nameof(User.Phone)},
                password_hash AS {nameof(User.PasswordHash)}
                FROM users
                WHERE user_name = @UserName";
            var getUserByUsername = _dbConnection.QueryFirstOrDefault<User>(sqlcommand, new { UserName = username });
            return getUserByUsername;
        }
    }
}
