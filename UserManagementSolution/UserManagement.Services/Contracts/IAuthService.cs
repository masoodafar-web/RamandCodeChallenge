using UserManagement.Models.DTOs;

namespace UserManagement.Services.Contracts;

public interface IAuthService
{
    Task<string> Authenticate(LoginModel user);
    Task<CurrentUserModel> GetCurrentUser();
}