using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Projekat.Models
{
    public class JwtManager
    {
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public static string GenerateToken(User user)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    (new Claim(ClaimTypes.NameIdentifier, user.UserName)),
                    (new Claim(ClaimTypes.Name, user.Name)),
                    (new Claim(ClaimTypes.Surname, user.LastName)),
                    (new Claim(ClaimTypes.Email, user.Email)),
                    (new Claim(ClaimTypes.DateOfBirth, user.Birthday.ToString())),
                    (new Claim(ClaimTypes.Role, user.Role)),

                }),

                Expires = now.AddDays(1),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;

            
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return principal;
            }

            catch (Exception)
            {
                return null;
            }
        }

        public static User GetUserFromIdentity(ClaimsIdentity claimsIdentity)
        {
            User user = new User();
            if (claimsIdentity != null)
            {
                IEnumerable<Claim> claims = claimsIdentity.Claims;
                user.UserName = claims.Where(p => p.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                user.Role = claims.Where(p => p.Type == ClaimTypes.Role).FirstOrDefault()?.Value;
            }
            return user;
        }
    }
}