using System.Data.Common;

namespace UserManagement.Models.Entities;


    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NatioalId { get; set; }
        public int Age { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

