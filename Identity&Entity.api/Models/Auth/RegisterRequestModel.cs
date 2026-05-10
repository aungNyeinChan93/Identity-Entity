namespace Identity_Entity.api.Models.Auth
{
    public class RegisterRequestModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; } = "guest";
    }
}
