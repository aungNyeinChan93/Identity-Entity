namespace Identity_Entity.api_05.Models.Auth
{
    public class LoginRequestModel
    {
        public required string Email { get; set; }
        public required string password { get; set; }

    }
}
