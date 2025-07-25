namespace Contracts
{
    public record UsersRequest(
        string Login,
        string Password,
        string Name, 
        int Gender,
        DateTime? Birthday, 
        bool Admin);
}