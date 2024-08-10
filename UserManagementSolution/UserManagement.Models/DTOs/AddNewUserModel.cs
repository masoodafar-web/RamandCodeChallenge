namespace UserManagement.Models.DTOs;

public class AddNewUserModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalId { get; set; }
    public int Age { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}