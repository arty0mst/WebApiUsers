namespace Contracts
{
     public record UsersResponse(
        Guid Guid,
        string Login,
        string Password,
        string Name, 
        int Gender,
        DateTime? Birthday, 
        bool Admin, 
        DateTime CreatedOn, 
        string CreatedBy, 
        DateTime? ModifiedOn, 
        string ModifiedBy, 
        DateTime? RevokedOn, 
        string RevokedBy);
}