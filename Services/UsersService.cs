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

        public async Task<Guid> DeleteBook(Guid id)
        {
            return await _usersRepository.Delete(id);
        }
    }
}