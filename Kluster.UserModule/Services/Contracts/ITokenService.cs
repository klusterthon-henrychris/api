namespace Kluster.UserModule.Services.Contracts;

public interface ITokenService
{
    string CreateUserJwt(string emailAddress, string userRole, string userId);
}