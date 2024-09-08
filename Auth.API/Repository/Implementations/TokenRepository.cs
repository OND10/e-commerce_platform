using Auth.API.Contract.Interfaces;
using Auth.API.Models;
using Auth.API.Models.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.API.Repository.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        private readonly JwtOptions _jWtoptions;
        public TokenRepository(IConfiguration configuration,IOptions<JwtOptions> jWtOptions)
        {
            _configuration = configuration;
            _jWtoptions = jWtOptions.Value;
        }
        public string CreateJWTTokenAsync(ApplicationUser user, IEnumerable<string> roles)
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            //Creating Claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName)
            };

            //Assigning claims to its roles
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            //JWT Security Tokan Params coming from the Appsetting.json
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:Secret"]));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWtoptions.Secret));

            var credentails = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //This method was used but you can not get the name of role and name if you want to set your cookie token and signIn a user
            //var token = new JwtSecurityToken(
            //    issuer: _jWtoptions.Issuer,
            //    audience: _jWtoptions.Audience,
            //    claims: claims,
            //    expires: DateTime.Now.AddMinutes(15),
            //    signingCredentials: credentails
            //    );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jWtoptions.Audience,
                Issuer = _jWtoptions.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = credentails
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

