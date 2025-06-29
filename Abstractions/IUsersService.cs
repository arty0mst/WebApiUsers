using Models;

public interface IUsersService
{
    Task<(Guid, string)> CreateUser(User user);
    Task<Guid> DeleteUser(Guid id);
    Task<List<User>> GetAllUsers();
    Task<List<User>> GetAllActiveUsers();
    Task<List<User>> GetTargetUser(string login);
    Task<List<User>> GetAgeUser(int age);
    Task<Guid> UpdateUser(Guid guid, string login, string password, string name, int gender, DateTime? birthday, bool admin, DateTime createdOn, string createdBy, DateTime? ModifiedOn, string modifiedBy, DateTime? RevokedOn, string revokedBy);
    Task EnsureAdminExists();
    Task<(User? userCurrent, string error)> Authorization(string login, string password);
}