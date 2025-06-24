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
                    DateTime.ParseExact("10.10.1990", "dd.MM.yyyy", CultureInfo.InvariantCulture).ToUniversalTime(),
                    "",
                    DateTime.ParseExact("10.10.1990", "dd.MM.yyyy", CultureInfo.InvariantCulture).ToUniversalTime(),
                    ""
                );

                await _usersRepository.Create(user);
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _usersRepository.Get();
        }

        public async Task<Guid> CreateUser(User user)
        {
            return await _usersRepository.Create(user);
        }

        public async Task<Guid> UpdateUser(Guid guid, string login, string password, string name, int gender, DateTime? birthday, bool admin,
            DateTime createdOn, string createdBy, DateTime modifiedOn, string modifiedBy, DateTime revokedOn, string revokedBy)
        {
            return await _usersRepository.Update(guid, login, password, name, gender, birthday, admin, createdOn, createdBy, modifiedOn, modifiedBy, revokedOn, revokedBy);
        }

        public async Task<Guid> DeleteUser(Guid id)
        {
            return await _usersRepository.Delete(id);
        }
    }
}