namespace Kluster.Shared.SharedContracts.UserModule;

// registered in userModule
public interface ICurrentUser
{
    string? UserId { get; }
    string? Email { get; }
    string? Role { get; }
}