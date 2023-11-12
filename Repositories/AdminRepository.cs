using Microsoft.EntityFrameworkCore;
using KerryCoAdmin.Data;
using KerryCoAdmin.Models;
using KerryCoAdmin.Interfaces;

namespace KerryCoAdmin.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;




        public AdminRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<bool> AdminByUserExists(string userId)
        {
            return await _context.Admins.AnyAsync(a => a.User.Id == userId);
        }



        public async Task<bool> AdminExists(string adminId)
        {
            return await _context.Admins.AnyAsync(a => a.AdminId.ToString() == adminId);
        }

        public async Task<Admin> GetAdminByUser(string userId)
        {

            var admins = await _context.Admins
                               .Include(a => a.User)
                               .ToListAsync();


            var admin = new List<Admin>();

            foreach (var a in admins)
            {
                if (a.User.Id.ToString() == userId)
                {
                    Console.WriteLine(a);

                    admin.Add(a);

                }
            }

         
            return admin.First();

        }

        public async Task<Admin> GetAdminByAdminId(string adminId)
        {
            var admin = await _context.Admins
                               .Where(a => a.AdminId.ToString() == adminId)
                               .Include(a => a.User)
                               .ToListAsync();

            return admin.First();
        }


        public async Task<ICollection<Admin>> GetAdmins()
        {
            var admins = await _context.Admins
                                .Include(a => a.User)
                                .ToListAsync();

            return admins;
        }


        public async Task<bool> CreateAdmin(Admin admin)
        {
            await _context.Admins.AddAsync(admin);

            return Save();
        }


        public async Task<bool> DeleteAdmin(string adminId)
        {
            var savedAdmin = _context.Admins.FirstOrDefault(a => a.AdminId.ToString() == adminId);

            if (savedAdmin != null)
            {
                _context.Admins.Remove(savedAdmin);

                return Save();
            }

            throw new InvalidOperationException();
        }


        public async Task<bool> UpdateAdmin(Admin updatedAdmin)
        {
            _context.Update(updatedAdmin);

            return Save();

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }


    }
}
