namespace Identity_Entity.api_06.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }

        public required string Email { get; set; }

        public required string Pssword { get; set; }

        public required string Role { get; set; }

        public string? Department { get; set; }

        public int? Age { get; set; }

    }
}
