using System.ComponentModel.DataAnnotations;

namespace Identity_Entity.api_06.Models.Accounts
{
    public class LoginRequestModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string password { get; set; }
    }
}
