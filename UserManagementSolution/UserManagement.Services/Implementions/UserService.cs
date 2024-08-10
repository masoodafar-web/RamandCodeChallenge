using Dapper;
using UserManagement.Data;
using UserManagement.Models.DTOs;
using UserManagement.Services.Contracts;

namespace UserManagement.Services.Implementions;

public class UserService:IUserService
{
    private readonly DapperContext _context;

    public UserService(DapperContext context)
    {
        _context = context;
    }

    public async Task<GetUserModel> GetUserData(string username)
    {
        var query = $"SELECT * FROM Users WHERE UserName = @UserName";
        using (var connection = _context.CreateConnection())
        {
            var user = await connection.QueryFirstAsync<GetUserModel>(query, new { UserName = username });
            return user;
        }
    }
    public async Task<int> AddUser(AddNewUserModel user)
    {
        user.Password=BCrypt.Net.BCrypt.HashPassword(user.Password);
        var query = "INSERT INTO Users (UserName, FirstName, LastName, NationalId, Age, PasswordHash) " +
                    "VALUES (@UserName, @FirstName, @LastName, @NationalId, @Age,@Password);" +
                    "SELECT CAST(SCOPE_IDENTITY() as int);";

        using (var connection = _context.CreateConnection())
        {
            var userId = await connection.QuerySingleAsync<int>(query, user);
            return userId;
        }
    }
}