using KerryCoAdmin.Api.Entities.Models;

namespace KerryCoAdmin.Api.Interfaces
{
    public interface ISecretService
    {
        //void RefreshSecrets();
        List<VaultSecret> GetSecrets();
    }
}
