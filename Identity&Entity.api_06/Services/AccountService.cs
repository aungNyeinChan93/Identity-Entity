using Identity_Entity.api_06.Data;
using Identity_Entity.api_06.Interfaces;
using Identity_Entity.api_06.Models;
using Identity_Entity.api_06.Models.Accounts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity_Entity.api_06.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<User> CreateUserAsync(CreateUserRequestModel requestModel)
        {
            var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == requestModel.Email);
            if (checkUser is not null)
            {
                return default!;
            }
            var newUser = new User
            {
                UserName = requestModel.UserName,
                Email = requestModel.Email,
                Pssword = requestModel.Password,
                Role = requestModel.Role
            };

            //TODO hashpassword

            await _context.Users.AddAsync(newUser);
            var result = await _context.SaveChangesAsync();
            return result >= 1 ? newUser : default!;
        }

        public async Task<bool> LoginAsync(LoginRequestModel requestModel ,HttpContext ctx)
        {
            //check userExist
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x=>x.Email.Equals(requestModel.Email));

            if (user is null)
            {
                return false;
            }

            //check password
            if (user.Pssword != requestModel.password)
            {
                return false;
            }
            //create cookie
            var claims = new ClaimsIdentity([
                    new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role,user.Role)
                ], "cookieAuth");

            var userPrinciple = new ClaimsPrincipal(claims);

            await ctx.SignInAsync("cookieAuth",userPrinciple);
            return true;
        }

        public async Task<User?> GetUser(string email)
        {
            var user = await _context.Users.AsNoTracking()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return user;
        }
    }
}
