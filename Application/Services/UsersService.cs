using System.Globalization;
using Models;
using Repositories;

namespace services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        // Создание администратора при необходимости
        public async Task EnsureAdminExists()
        {
            var users = await _usersRepository.Get();

            var adminUser = users.FirstOrDefault(u => u.Admin);

            if (adminUser == null)
            {
                var (user, error) = User.Create(
                    Guid.NewGuid(),
                    "admin",
                    "admin123",
                    "Artem",
                    0,
                    DateTime.ParseExact("20.07.2000", "dd.MM.yyyy", CultureInfo.InvariantCulture).ToUniversalTime(),
                    true,
                    DateTime.Now.ToUniversalTime(),
                    "init",
                    null,
                    "",
                    null,
                    ""
                );

                await _usersRepository.Create(user);
            }
        }

        public async Task<(User? userCurrent, string error)> Authorization(string login, string password)
        {
            var users = await _usersRepository.Get();
            var userCurrent = users.FirstOrDefault(u => u.Login == login);

            if (userCurrent == null)
            {
                return (null, "User not found");
            }

            if (userCurrent!.Password != password)
            {
                return (userCurrent, "Incorrect password");
            }

            if (userCurrent!.RevokedBy != "")
            {
                return (null, "Your profile has been deleted");
            }

            return (userCurrent, "");
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _usersRepository.Get();
        }

        public async Task<List<User>> GetAllActiveUsers()
        {
            var users = await _usersRepository.Get();

            return users
                .Where(u => u.RevokedOn == null)
                .OrderBy(u => u.CreatedOn)
                .ToList();
        }

        public async Task<List<User>> GetTargetUser(string login)
        {
            var users = await _usersRepository.Get();

            return users
                .Where(u => u.Login == login)
                .ToList();
        }

        public async Task<List<User>> GetAgeUser(int age)
        {
            var users = await _usersRepository.Get();

            var today = DateTime.Today;

            return users
                .Where(u => 
                {
                    int userAge = today.Year - u.Birthday!.Value.Year;

                    return userAge >= age;
                })
                .ToList();
        }

        public async Task<(Guid, string)> CreateUser(User user)
        {
            var users = await _usersRepository.Get();
            var userCheck = users.FirstOrDefault(u => u.Login == user.Login);

            if (userCheck != null)
            {
                return (userCheck.Guid, "User already exist");
            }

            return (await _usersRepository.Create(user), "");
        }

        public async Task<Guid> UpdateUser(Guid guid, string login, string password, string name, int gender, DateTime? birthday, bool admin,
            DateTime createdOn, string createdBy, DateTime? modifiedOn, string modifiedBy, DateTime? revokedOn, string revokedBy)
        {
            // var users = await _usersRepository.Get();
            // var userCheck = users.FirstOrDefault(u => u.Login == login);

            // if (userCheck != null)
            // {
            //     return (userCheck.Guid, "User already exist");
            // }

            // return (await _usersRepository.Update(guid, login, password, name, gender, birthday, admin, createdOn, createdBy, modifiedOn, modifiedBy, revokedOn, revokedBy), "");
            return await _usersRepository.Update(guid, login, password, name, gender, birthday, admin, createdOn, createdBy, modifiedOn, modifiedBy, revokedOn, revokedBy);
        }

        public async Task<Guid> DeleteUser(Guid id)
        {
            return await _usersRepository.Delete(id);
        }
    }
}