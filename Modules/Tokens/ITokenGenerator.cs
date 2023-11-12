using Microsoft.AspNetCore.Identity;
using System.Collections;


namespace KerryCoAdmint.Modules.Tokens
{
    public interface ITokenGenerator
    {
        public string GenerateJwtToken(IdentityUser user, byte[] key, DateTime expires, string firstName, string userId, List<string> userRoles);
    }
}
