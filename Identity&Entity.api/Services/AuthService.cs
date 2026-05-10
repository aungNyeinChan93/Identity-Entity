using Identity_Entity.api.Models.Auth;
using Identity_Entity.data_01.Data;
using Identity_Entity.data_01.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity_Entity.api.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        private readonly IConfiguration configuration;

        public AuthService(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            this.configuration = configuration;
        }

        public async Task<AppUser?> RegisterAsync(RegisterRequestModel requestModel)
        {
            var user = await _userManager.FindByEmailAsync(requestModel.Email);
            if (user is not null)
            {
                return default!;
            }
            
           var transaction = await _context.Database.BeginTransactionAsync();

            var newUser = new AppUser { UserName = requestModel.Name, Email = requestModel.Email };
            if (newUser is null)
            {
                return default!;
            }

            try
            {
                var userResult = await _userManager.CreateAsync(newUser, requestModel.Password);
                if (userResult.Succeeded)
                {
                    var role = await _userManager.AddToRoleAsync(newUser, requestModel.Role);
                    await transaction.CommitAsync();
                    if (!role.Succeeded)
                    {
                        await transaction.RollbackAsync();
                    }
                }
                else
                {
                    await transaction.RollbackAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return newUser;

        }

        public async Task<string?> LoginAsync(LoginRequestModel loginRequestModel)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestModel.Email);
            if (user is null)
            {
                return default!;
            }

            var checkPassword = await _signInManager.CheckPasswordSignInAsync(user, loginRequestModel.Password, false);
            if (!checkPassword.Succeeded)
            {
                return default!;
            }

            var token = await this.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
            {
                return default!;
            }

            return token;
        }

        private async Task<string> GenerateToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:JWT_SECRET")!));

            var crendials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var claims = new ClaimsIdentity(new List<Claim>
            {
                new Claim( ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.Role ,roles.First())

            });

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = crendials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);

        }
    }
}
