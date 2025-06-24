namespace Contracts
{
     public record UsersTargetResponse(
        string Name, 
        int Gender,
        DateTime? Birthday, 
        DateTime? RevokedOn);
}