using KerryCoAdmin.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KerryCoAdmin.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public UserRepository(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }


        //get user by id
        public async Task<IdentityUser> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }


        //get user by email
        public async Task<IdentityUser> GetUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }


        //get user by username
        public async Task<IdentityUser> GetUserByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        //check user password

        public async Task<bool> CheckPassword(IdentityUser user, string password)
        {
            var isPassword = await _userManager.CheckPasswordAsync(user, password);

            if (isPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public async Task<bool> CreateUser(IdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        // check if role exists
        public async Task<bool> FindRole(string role)
        {
            return await _roleManager.RoleExistsAsync(role);
        }


        // delete user
        public async Task<bool> DeleteUser(string userId)
        {
            var savedUser = await _userManager.FindByIdAsync(userId);

            if (savedUser != null)
            {
                var result = await _userManager.DeleteAsync(savedUser);

                if (result.Succeeded)
                    return true;

                return false;


            }
            throw new InvalidOperationException();
        }


        // get roles
        public async Task<ICollection<string>> GetRoles(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> AddRole(IdentityUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);

            if (result.Succeeded)
            {
                return true;
            }
            return false;

        }

        public async Task<ICollection<IdentityRole>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return roles;

        }

    }
}
