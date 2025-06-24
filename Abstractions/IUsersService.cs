using Models;

public interface IUsersService
{
    Task<Guid> CreateUser(User user);
    Task<Guid> DeleteBook(Guid id);
    Task<List<User>> GetAllUsers();
    Task<Guid> UpdateUser(Guid guid, string login, string password, string name, int gender, DateTime? birthday, bool admin, DateTime createdOn, string createdBy, DateTime modifiedOn, string modifiedBy, DateTime revokedOn, string revokedBy);
}