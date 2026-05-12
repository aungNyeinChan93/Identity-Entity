using System.ComponentModel.DataAnnotations;

namespace Identity_Entity.api_06.Models.Accounts
{
    public class CreateUserRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
