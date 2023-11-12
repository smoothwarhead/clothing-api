using KerryCoAdmint.Modules.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace KerryCoAdmin.Modules.Tokens
{
    public class TokenGenerator : ITokenGenerator
    {
        public string GenerateJwtToken(IdentityUser user, byte[] key, DateTime expires, string userName, string userId, List<string> userRoles)
        {
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString()),
            };

            //add roles to clain
            foreach(var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var token = new JwtSecurityToken(

                claims: claims,
                expires: expires,
                signingCredentials: credentials

            );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodedToken;


            /*

            return new AuthResponse()
            {
                IsAuth = true,
                Token = encodedToken,
                StatusType = "success",
                Message = "user successfully signed in"
            };



            */
        }
    }
}
