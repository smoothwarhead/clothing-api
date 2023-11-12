using KerryCoAdmin.Api.Entities.Dtos.Responses;
using KerryCoAdmin.Api.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace KerryCoAdmin.Api.Modules
{
    public class GetInfo
    {

        public static AuthUser GetUser(IdentityUser user)
        {
            var authUser = new AuthUser
            {
                UserId = user.Id,
                UserName = user.UserName

            };

            return authUser;
        }

        public static string GetASecret(string name, List<VaultSecret> secrets)
        {
            string sec = "";

            foreach (var secret in secrets)
            {
                if(secret.Name == name)
                {
                    sec = secret.Value;
                }
                                

            }

            return sec;

            

        }


    }
}
