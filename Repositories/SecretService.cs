using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using KerryCoAdmin.Api.Entities.Models;
using KerryCoAdmin.Api.Interfaces;
using Microsoft.Extensions.Logging;

namespace KerryCoAdmin.Api.Repositories
{
    public class SecretService : ISecretService
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<SecretService> logger;


        public SecretService(IConfiguration configuration, ILogger<SecretService> logger)
        {
            _configuration = configuration;
            this.logger = logger;
        }


        public List<VaultSecret> GetSecrets()
        {
            var keyVaultSecrets = new List<VaultSecret>();
            try
            {

                var keyVaultEndpoint = _configuration["VaultKey"];
                //var clientId = _configuration["Azure:ClientId"];
                //var clientSecret = _configuration["Azure:ClientSecret"];
                //var tenantId = _configuration["Azure:TenantId"];

                //var secretClient = new SecretClient(new Uri(keyVaultEndpoint), new ClientSecretCredential(tenantId, clientId, clientSecret));
                var secretClient = new SecretClient(new Uri(keyVaultEndpoint), new DefaultAzureCredential());


                if (!string.IsNullOrEmpty(keyVaultEndpoint))
                {

                    // Get the root configuration section
                    var rootConfiguration = (IConfigurationRoot)_configuration;

                    // Create a dictionary to hold the secrets
                    var secrets = new Dictionary<string, string>();

                    // Get the keys of all the existing secrets
                    var secretProperties = secretClient.GetPropertiesOfSecrets();
                    foreach (var secretProperty in secretProperties)
                    {
                        var secretName = secretProperty.Name;
                        var secretValue = secretClient.GetSecret(secretName).Value.Value;

                        keyVaultSecrets.Add(new VaultSecret() { Name = secretName, Value = secretValue });

                    }
                }
                else
                {
                    logger.LogWarning("The KeyVault:BaseUrl configuration setting is missing or empty. No secrets were refreshed.");
                }


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while refreshing the secrets from Key Vault.");
            }
            return keyVaultSecrets;
        }



    }
}
