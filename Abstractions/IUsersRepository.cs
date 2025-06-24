using Models;

namespace Repositories
{
    public interface IUsersRepository
    {
        Task<Guid> Create(User user);
        Task<Guid> Delete(Guid guid);
        Task<List<User>> Get();
        Task<Guid> Update(Guid guid, string login, string password, string name, int gender, DateTime? birthday, bool admin,
            DateTime createdOn, string createdBy, DateTime? ModifiedOn, string modifiedBy, DateTime? RevokedOn, string revokedBy);
    }
}