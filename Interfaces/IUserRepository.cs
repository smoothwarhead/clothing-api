using Microsoft.AspNetCore.Identity;

namespace KerryCoAdmin.Interfaces
{
    public interface IUserRepository
    {
        //get specific user based on id
        Task<IdentityUser> GetUserById(string id);

        // get specific user based on email
        Task<IdentityUser> GetUserByEmail(string email);


        // get specific user based on userName
        Task<IdentityUser> GetUserByUserName(string userName);


        // check password
        Task<bool> CheckPassword(IdentityUser user, string password);


        // create a user
        Task<bool> CreateUser(IdentityUser user, string password);



        //delete user
        Task<bool> DeleteUser(string userId);


        // check if role exists
        Task<bool> FindRole(string role);

        Task<bool> AddRole(IdentityUser user, string role);

        //get roles
        Task<ICollection<string>> GetRoles(IdentityUser user);

        Task<ICollection<IdentityRole>> GetAllRoles();

    }
}
