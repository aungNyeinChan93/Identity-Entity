using Identity_Entity.api_05.Models.Auth;
using Identity_Entity.Data_02.Data;
using Identity_Entity.Data_02.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity_Entity.api_05.Services
{
    public class AuthService
    {
        private readonly JwtAuthDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(JwtAuthDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }



        public async Task<AppUser?> RegisterAsync(RegisterRequestModel requestModel)
        {
            var checkuser = await _userManager.FindByEmailAsync(requestModel.Email);
            if (checkuser is not null)
            {
                return default!;
            }

            var newUser = new AppUser
            {
                UserName = requestModel.Name,
                Email = requestModel.Email,
                
            };

            var transaction = _context.Database.BeginTransaction();

            try
            {
                var user = await _userManager.CreateAsync(newUser, requestModel.Password);
                var role = await _userManager.AddToRoleAsync(newUser, requestModel.Role);

                if (!user.Succeeded || !role.Succeeded)
                {
                    transaction.Rollback();
                    return default!;
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            
            return newUser;

        }

        public async Task<string> LoginAsync(LoginRequestModel requestModel)
        {
            var user = await _userManager.FindByEmailAsync(requestModel.Email);
            if (user is null)
            {
                return default!;
            }

            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user,requestModel.password,false);

            if (!checkPassword.Succeeded)
            {
                return default!;
            }

            var token = await this.GenerateToken(user);
            if (token is null)
            {
                return default!;
            }
            return token;
        }

        private async Task<string?> GenerateToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:JWT")!));

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var claims = new ClaimsIdentity([
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Email!),
                    new Claim(ClaimTypes.Role ,roles.First())
                ]);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
           
            return token;
        }
    }
}
