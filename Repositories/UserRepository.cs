using DataAccess;
using Entites;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly UserStoreDbContext _context;
        public UsersRepository(UserStoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> Get()
        {
            var userEntites = await _context.Users
                .AsNoTracking()
                .ToListAsync();

            var users = userEntites
                .Select(u => User.Create(u.Guid, u.Login, u.Password, u.Name, u.Gender, u.Birthday, u.Admin, u.CreatedOn,
                        u.CreatedBy, u.ModifiedOn, u.ModifiedBy, u.RevokedOn, u.RevokedBy).User)
                .ToList();

            return users;
        }

        public async Task<Guid> Create(User user)
        {
            var userEntity = new UserEntity
            {
                Guid = user.Guid,
                Login = user.Login,
                Password = user.Password,
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Admin = user.Admin,
                CreatedOn = user.CreatedOn,
                CreatedBy = user.CreatedBy,
                ModifiedOn = user.ModifiedOn,
                ModifiedBy = user.ModifiedBy,
                RevokedOn = user.RevokedOn,
                RevokedBy = user.RevokedBy,
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.Guid;
        }

        public async Task<Guid> Update(Guid guid, string login, string password, string name, int gender, DateTime? birthday, bool admin,
            DateTime createdOn, string createdBy, DateTime modifiedOn, string modifiedBy, DateTime revokedOn, string revokedBy)
        {
            await _context.Users
                .Where(u => u.Guid == guid)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Login, login)
                    .SetProperty(u => u.Password, password)
                    .SetProperty(u => u.Name, name)
                    .SetProperty(u => u.Gender, gender)
                    .SetProperty(u => u.Birthday, birthday)
                    .SetProperty(u => u.Admin, admin)
                    .SetProperty(u => u.CreatedOn, createdOn)
                    .SetProperty(u => u.CreatedBy, createdBy)
                    .SetProperty(u => u.ModifiedOn, modifiedOn)
                    .SetProperty(u => u.ModifiedBy, modifiedBy)
                    .SetProperty(u => u.RevokedOn, revokedOn)
                    .SetProperty(u => u.RevokedBy, revokedBy));

            return guid;
        }

        public async Task<Guid> Delete(Guid guid)
        {
            await _context.Users
                .Where(u => u.Guid == guid)
                .ExecuteDeleteAsync();

            return guid;
        }

    }
}