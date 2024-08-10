using UserManagement.Models.DTOs;

namespace UserManagement.Services.Contracts;

public interface IUserService
{
    Task<GetUserModel> GetUserData(string username);
    Task<int> AddUser(AddNewUserModel user);
}