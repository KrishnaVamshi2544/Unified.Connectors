using Microsoft.Data.SqlClient;
using Unified.Connectors.DBContext;
using Unified.Connectors.EntityModels;
using VaultSharp.Core;

namespace Unified.Connectors.Helpers
{
    public class CompanyConfig
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
                var config = (from cc in unifiedDbContext.CompanyConfigurations
                             join con in unifiedDbContext.Connectors
                             on cc.ConnectorId equals con.Id
                             where cc.IsDeleted == false && con.IsDeleted == false
                             select new CompanyConfiguration
                             {
                                 Id = cc.Id,
                                 CompanyId = cc.CompanyId,
                                 ClientId = cc.ClientId,
                                 ConnectorId = cc.ConnectorId,
                                 Path = cc.Path,
                                 CreatedById = cc.CreatedById,
                                 ModifiedById = cc.ModifiedById,
                                 CreationDate = cc.CreationDate,
                                 ModificationDate = cc.ModificationDate,
                             }).FirstOrDefault();

                if (config == null)
                {
                    throw new NullReferenceException("Configuration Getting Null from DataBase");
                }

                //Get Exchange Configuration
                var configuration = new HashiVaultHelper().GetConfigAsync(config.Path).Result;

                return configuration;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error Processing while Getting Data");
            }
        }
    }
}
