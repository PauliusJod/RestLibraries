using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestLibraries.Auth
{
    public interface IJwtTokenService
    {
        string CreateAccessToken(string userName, string userId, IEnumerable<string> userRoles);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private SymmetricSecurityKey _authSigningKey;
        private string _issuer;
        private string _audience;

        public JwtTokenService(IConfiguration configuration)
        {
            _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            _issuer = configuration["JWT:ValidIssuer"];
            _audience = configuration["JWT:ValidAudience"];
        }
        public string CreateAccessToken(string userName, string userId, IEnumerable<string> userRoles)
        {
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, userName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, userId),
            };

            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var accessSecurityToken = new JwtSecurityToken
            (
            issuer: _issuer,
            audience: _audience,
            expires: DateTime.UtcNow.AddHours(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(_authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(accessSecurityToken);
        }
        //public string DeleteAccessToken(JwtSecurityToken tokenOld)
        //{
        //    var a = new JwtSecurityToken();
        //    var accessSecurityToken = new JwtSecurityToken
        //    (
        //    issuer: tokenOld.Issuer,
        //    audience: "TrustedClient",
        //    expires: DateTime.Now,
        //    claims: tokenOld.Claims,
        //    signingCredentials: tokenOld.SigningCredentials// new SigningCredentials(_authSigningKey, SecurityAlgorithms.HmacSha256)
        //    );



        //    return new JwtSecurityTokenHandler().TokenLifetimeInMinutes(accessSecurityToken);
        //}

    }
}
