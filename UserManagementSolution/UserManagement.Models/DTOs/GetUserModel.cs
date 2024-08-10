namespace UserManagement.Models.DTOs;

public class GetUserModel
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalId { get; set; }
    public int Age { get; set; }
    public string Username { get; set; }
}