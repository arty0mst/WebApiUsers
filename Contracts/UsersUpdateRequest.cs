namespace Contracts
{
    public record UsersUpdateRequest(
        string Login,
        string Password,
        string Name, 
        int Gender,
        DateTime? Birthday);
}