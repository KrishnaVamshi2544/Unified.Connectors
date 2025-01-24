using Microsoft.Data.SqlClient;
using Unified.Connectors.DBContext;
using VaultSharp.Core;

namespace Unified.Connectors.Helpers
{
    public class ClientConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unifiedDbContext"></param>
        /// <param name="companyId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetConfig(UnifiedDbContext unifiedDbContext,int companyId, int clientId)
        {
            try
            {
                var config = unifiedDbContext.ClientConfigurations.FirstOrDefault(x => x.CompanyId == companyId && x.ClientId == clientId);
                if (config == null)
                {
                    throw new NullReferenceException("Configuration Getting Null from DataBase");
                }

                //Get Exchange Configuration
                var configuration = new HashiVaultHelper().GetConfigAsync(config.Path).Result;

                return configuration;
            }
            catch (SqlException)
            {
                throw new Exception("Error Processing while Getting Data");
            }
        }
    }
}
