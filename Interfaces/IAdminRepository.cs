using KerryCoAdmin.Models;



namespace KerryCoAdmin.Interfaces
{
    public interface IAdminRepository
    {

        Task<bool> AdminByUserExists(string userId);
        Task<bool> AdminExists(string adminId);

        //Task<ICollection<Admin>> GetAdminByUser(string userId);
        //Task<ICollection<Admin>> GetAdminByAdminId(string adminId);

        Task<Admin> GetAdminByUser(string userId);
        Task<Admin> GetAdminByAdminId(string adminId);

        Task<ICollection<Admin>> GetAdmins();
        Task<bool> CreateAdmin(Admin admin);
        Task<bool> DeleteAdmin(string adminId);
        Task<bool> UpdateAdmin(Admin admin);

        bool Save();

    }
}
