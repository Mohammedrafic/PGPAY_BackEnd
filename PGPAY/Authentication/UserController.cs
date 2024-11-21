using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using PGPAY_DL.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace HRDesk.Authentication
{
    public class UserController : Controller
    {
        private readonly PGPAYContext _context;
        private readonly JWTTokenGeneratorModel _jwtTokenGeneratorModel;

        public UserController(PGPAYContext context, IOptions<JWTTokenGeneratorModel> options)
        {
            _context = context;
            _jwtTokenGeneratorModel = options.Value;
        }

        [Route("Authentication")]
        [HttpPost]
        public IActionResult Authentication(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_jwtTokenGeneratorModel.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.UserRole)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string finalToken = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Token = finalToken,
                Expiration = tokenDescriptor.Expires,
                Email = user.Email,
                Role = user.UserRole
            });
        }
    }
}
