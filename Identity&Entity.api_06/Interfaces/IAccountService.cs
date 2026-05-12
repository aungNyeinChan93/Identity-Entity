using Identity_Entity.api_06.Models;
using Identity_Entity.api_06.Models.Accounts;

namespace Identity_Entity.api_06.Interfaces
{
    public interface IAccountService
    {
        Task<User> CreateUserAsync(CreateUserRequestModel requestModel);
    }
}