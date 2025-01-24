using Microsoft.AspNetCore.Mvc;
using ExchangePOC.Helpers;
using Newtonsoft.Json;
using Unified.Connectors.Model;
using Unified.Connectors.Constants;
using Unified.Connectors.DBContext;
using Unified.Connectors.Helpers;

namespace Unified.Connectors.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HoldController : ControllerBase
    {
        #region declarations
        private readonly ILogger<HoldController> _logger;
        private readonly UnifiedDbContext _unifiedDbContext;
        #endregion


        #region constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="unifiedDbContext"></param>
        public HoldController(ILogger<HoldController> logger, UnifiedDbContext unifiedDbContext)
        {
            _logger = logger;
            _unifiedDbContext = unifiedDbContext;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRequestModel"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost("Create")]
        public ActionResult Post([FromBody] UserRequestModel userRequestModel)
        {
            //Get Exchange Configuration
            var configuration = new ClientConfiguration().GetConfig(_unifiedDbContext, userRequestModel.CompanyId, userRequestModel.ClientId);

            var config = JsonConvert.DeserializeObject<AzureADConfig>(configuration);

            //Check User Exist in AZURE AD
            var userDetails = CheckIfUserExistsAsync(config!, userRequestModel.Email).Result;
            if (userDetails == null)
            {
                _logger.LogError("User does not exist.");
                throw new Exception("User does not exist.");
            }


            //Create Case in Purview
            var caseResponse = CreatePremiumCaseAsync(config, userRequestModel.CaseName).Result;
            if (caseResponse == null)
            {
                throw new Exception("Unable to Create Case");
            }


            string email = userRequestModel.Email;
            //Add Custodian To case
            var custodian = AddCustodianToCaseAsync(config, caseResponse.Id, email).Result;
            if (custodian == null)
            {
                throw new Exception("Unable to Add Custodian to Case");
            }


            //Add LegalHold
            var legalHold = AddLegalHoldAsync(config, caseResponse.Id, userRequestModel.HoldName).Result;
            if (legalHold == null)
            {
                throw new Exception("Unable to Create LegalHold");
            }

            // add user source to legalhold
            var userSource = AddUserSourceToLegalHoldAsync(config, caseResponse.Id, legalHold.Id, email, GraphConstants.Mailbox).Result;
            if (userSource == null)
            {
                throw new Exception("Unable to Add UserSource to LegalHold");
            }


            // add user source to Custodian
            var custUserSource = AddUserSourceToCustodianAsync(config, email, caseResponse.Id, custodian.Id, GraphConstants.Mailbox).Result;
            if (custUserSource == null)
            {
                throw new Exception("Unable to Add UserSource to Custodian");
            }

            //Create SourceCollection
            string custodianName = "NithishReddy";
            var sourceCollection = CreateSourceCollectionAsync(config, caseResponse.Id, custodian.Id, custUserSource.id, custodianName + DateTime.UtcNow).Result;
            if (sourceCollection == null)
            {
                throw new Exception("Unable to Create Source Collection");
            }


            //Estimate Collections
            var estimatedStatsUrl = EstimateCollectionsAsync(config, caseResponse.Id, sourceCollection.Id).Result;
            if (estimatedStatsUrl == null)
            {
                throw new Exception("Unable to Fetch EstimateCollections Url");
            }

            //Get EstimateStats
            var estimatedStatsStatus = GetEstimatedStatsStatus(config, estimatedStatsUrl).Result;

            //PollingHelper to verify EstimatedStats has Completed.
            if (!IsEstimatedStatsCompleted(config, estimatedStatsUrl))
                new PollingHelper().Execute(IsEstimatedStatsCompleted, config, estimatedStatsUrl);

            //Get Stats
            estimatedStatsStatus = GetEstimatedStatsStatus(config, estimatedStatsUrl).Result;

            var isSearchDeleted = DeleteCollectionAsync(config, caseResponse.Id, sourceCollection.Id).Result;

            var response = new HoldStats
            {
                indexedItemCount = Convert.ToInt32(estimatedStatsStatus.indexedItemCount),
                indexedItemsSize = Convert.ToInt64(estimatedStatsStatus.indexedItemsSize)
            };


            return Ok(response);
        }

        /// <summary>
        /// Check if the user exists in Azure AD
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userPrincipalName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<User> CheckIfUserExistsAsync(AzureADConfig config, string userPrincipalName)
        {
            try
            {
                config.AuthenticationMode = "ClientSecret";

                var accessToken = new AzureADHelper().GetGraphToken(config).Result;
                var requestUrl = $"https://graph.microsoft.com/v1.0/users/{userPrincipalName}";

                var response = new AzureADHelper().GetHttpContentWithTokenAsync(requestUrl, accessToken).Result;
                var responseBody = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<User>(responseBody);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }

        }

        /// <summary>
        /// Create Premium CaseAsync
        /// </summary>
        /// <returns></returns>
        public static async Task<EDiscoveryCase> CreatePremiumCaseAsync(AzureADConfig config, string caseName)
        {
            try
            {
                var createCaseAPIDetails = GraphAPIEndpoints.CreateCase;
                config.Scope = createCaseAPIDetails.Scope;
                config.AuthenticationMode = "Delegated";

                var accessToken = new AzureADHelper().GetGraphToken(config).Result;
                var @case = new
                {
                    displayName = caseName,
                    description = "This is a new premium eDiscovery case created via Microsoft Graph API.",
                };

                string payload = JsonConvert.SerializeObject(@case, Newtonsoft.Json.Formatting.Indented);

                string endpoint = string.Format(createCaseAPIDetails.APIEndPoint, config.BaseURL, "v1.0");

                var response = new AzureADHelper().PostHttpContentWithTokenAsync(endpoint, accessToken, payload).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var eDiscoveryCase = JsonConvert.DeserializeObject<EDiscoveryCase>(responseBody);

                    return eDiscoveryCase!;
                }
                else
                {
                    throw new Exception("Failed to create premium case");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="caseId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<Custodian> AddCustodianToCaseAsync(AzureADConfig config, string caseId, string email)
        {
            try
            {
                var custodian = new
                {
                    email = email,
                    applyHoldToSources = false
                };
                string payload = JsonConvert.SerializeObject(custodian, Newtonsoft.Json.Formatting.Indented);
                var endPointDetails = GraphAPIEndpoints.AddCustodianToCase;
                config.Scope = endPointDetails.Scope;
                config.AuthenticationMode = "Delegated";

                var endpoint = string.Format(endPointDetails.APIEndPoint, config.BaseURL, "v1.0", caseId);

                var accessToken = new AzureADHelper().GetGraphToken(config).Result;
                var response = new AzureADHelper().PostHttpContentWithTokenAsync(endpoint, accessToken, payload).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var custResponse = JsonConvert.DeserializeObject<Custodian>(responseBody);

                    return custResponse!;
                }
                else
                {
                    throw new Exception("Failed to create Case Custodian");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="caseId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<LegalHold> AddLegalHoldAsync(AzureADConfig config, string caseId, string holdName)
        {
            try
            {
                string legalHoldName = holdName;
                string contentQuery = null;
                var @hold = new
                {
                    displayName = legalHoldName,
                    isEnabled = true,
                    contentQuery = string.IsNullOrEmpty(contentQuery) ? null : contentQuery
                };
                string payload = JsonConvert.SerializeObject(@hold, Newtonsoft.Json.Formatting.Indented);
                var createHoldAPIDetails = GraphAPIEndpoints.CreateLegalHold;
                config.Scope = createHoldAPIDetails.Scope;
                config.AuthenticationMode = "Delegated";

                var endpoint = string.Format(createHoldAPIDetails.APIEndPoint, config.BaseURL, "v1.0", caseId);

                var accessToken = new AzureADHelper().GetGraphToken(config).Result;
                var response = new AzureADHelper().PostHttpContentWithTokenAsync(endpoint, accessToken, payload).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var legalHold = JsonConvert.DeserializeObject<LegalHold>(responseBody);

                    return legalHold!;
                }
                else
                {
                    throw new Exception("Failed to create LegalHold");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="caseID"></param>
        /// <param name="legalHoldID"></param>
        /// <param name="email"></param>
        /// <param name="sourceIncluded"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<UserSource> AddUserSourceToLegalHoldAsync(AzureADConfig config, string caseID, string legalHoldID, string email, string sourceIncluded)
        {
            try
            {
                var @userSource = new
                {
                    email = email,
                    includedSources = sourceIncluded
                };

                string payload = JsonConvert.SerializeObject(@userSource, Newtonsoft.Json.Formatting.Indented);
                var createCaseAPIDetails = GraphAPIEndpoints.AddUserSourceToHold;
                config.Scope = createCaseAPIDetails.Scope;
                config.AuthenticationMode = "Delegated";

                var endpoint = string.Format(createCaseAPIDetails.APIEndPoint, config.BaseURL, "v1.0", caseID, legalHoldID);

                var accessToken = new AzureADHelper().GetGraphToken(config).Result;
                var response = new AzureADHelper().PostHttpContentWithTokenAsync(endpoint, accessToken, payload).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var userSourceLH = JsonConvert.DeserializeObject<UserSource>(responseBody);

                    return userSourceLH!;
                }
                else
                {
                    throw new Exception("Failed to Add UserSource to LegalHold");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="email"></param>
        /// <param name="caseId"></param>
        /// <param name="custodianId"></param>
        /// <param name="sourceIncluded"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<UserSource> AddUserSourceToCustodianAsync(AzureADConfig config, string email, string caseId, string custodianId, string sourceIncluded)
        {
            try
            {
                var userSource = new
                {
                    email = email,
                    includedSources = sourceIncluded
                };

                string payload = JsonConvert.SerializeObject(userSource, Newtonsoft.Json.Formatting.Indented);
                var createCusUserSourceAPIDetails = GraphAPIEndpoints.AddUserSourceToCustodian;
                var endpoint = string.Format(createCusUserSourceAPIDetails.APIEndPoint, config.BaseURL, "v1.0", caseId, custodianId);
                config.Scope = createCusUserSourceAPIDetails.Scope;
                config.AuthenticationMode = "Delegated";

                var accessToken = new AzureADHelper().GetGraphToken(config).Result;
                var response = new AzureADHelper().PostHttpContentWithTokenAsync(endpoint, accessToken, payload).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var userSourceCust = JsonConvert.DeserializeObject<UserSource>(responseBody);

                    return userSourceCust!;
                }
                else
                {
                    throw new Exception("Failed toAdd UserSource to Custodian");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="caseId"></param>
        /// <param name="custodianId"></param>
        /// <param name="userSourceId"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<Collection> CreateSourceCollectionAsync(AzureADConfig config, string caseId, string custodianId, string userSourceId, string displayName)
        {
            try
            {
                string query = null;

                var sourceUrl = string.Format(GraphAPIEndpoints.GetCustodianUserSourceById.APIEndPoint,
                        config.BaseURL, "v1.0", caseId, custodianId, userSourceId);

                var sourceCollection = new CollectionReqForCustodialDS
                {
                    displayName = displayName,
                    contentQuery = string.IsNullOrEmpty(query) ? null : query,
                    custodianSourcesodatabind = new string[] { sourceUrl }
                };

                string payload = JsonConvert.SerializeObject(sourceCollection, Newtonsoft.Json.Formatting.Indented);
                var createSourceCollAPIDetails = GraphAPIEndpoints.CreateSourceCollection;
                var endpoint = string.Format(createSourceCollAPIDetails.APIEndPoint, config.BaseURL, "v1.0", caseId);
                config.Scope = createSourceCollAPIDetails.Scope;
                config.AuthenticationMode = "Delegated";

                var accessToken = new AzureADHelper().GetGraphToken(config).Result;
                var response = new AzureADHelper().PostHttpContentWithTokenAsync(endpoint, accessToken, payload).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var collectionResp = JsonConvert.DeserializeObject<Collection>(responseBody);

                    return collectionResp!;
                }
                else
                {
                    throw new Exception("Failed to create Source Collection");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="caseID"></param>
        /// <param name="collectionID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<string> EstimateCollectionsAsync(AzureADConfig config, string caseID, string collectionID)
        {
            try
            {
                var CreateCollEstimateStatsAPIDetails = GraphAPIEndpoints.CreateEstimateStats;
                var endpoint = string.Format(CreateCollEstimateStatsAPIDetails.APIEndPoint, config.BaseURL, "v1.0", caseID, collectionID);
                config.Scope = CreateCollEstimateStatsAPIDetails.Scope;
                config.AuthenticationMode = "Delegated";

                var accessToken = new AzureADHelper().GetGraphToken(config).Result;
                var response = new AzureADHelper().PostHttpContentWithTokenAsync(endpoint, accessToken).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Headers.Location.AbsoluteUri;
                }
                else
                {
                    throw new Exception("Failed to Fetch Estimate Collection");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<EstimateStatisticsOperation> GetEstimatedStatsStatus(AzureADConfig config, string url)
        {
            try
            {
                config.Scope = "eDiscovery.ReadWrite.All";
                config.AuthenticationMode = "Delegated";

                var accessToken = new AzureADHelper().GetGraphToken(config).Result;
                var response = new AzureADHelper().GetHttpContentWithTokenForPollingHelperAsync(url, accessToken).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var estimatedStats = JsonConvert.DeserializeObject<EstimateStatisticsOperation>(responseBody);
                    return estimatedStats!;
                }
                else
                {
                    throw new Exception("Failed to Fetch Estimate Collection");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="estimatedStatsUrl"></param>
        /// <returns></returns>
        public static bool IsEstimatedStatsCompleted(AzureADConfig config, string estimatedStatsUrl)
        {
            try
            {
                var exportResponse = GetEstimatedStatsStatus(config, estimatedStatsUrl).Result;

                if (exportResponse == null || string.IsNullOrEmpty(exportResponse.status) ||
                  (string.Compare(exportResponse.status, GraphConstants.Failed, StringComparison.OrdinalIgnoreCase) == 1
                  || string.Compare(exportResponse.status, GraphConstants.Succeeded, StringComparison.OrdinalIgnoreCase) == 1)
                 )
                    return false;
                else if (exportResponse.status.Equals(GraphConstants.Succeeded, StringComparison.OrdinalIgnoreCase) ||
                  exportResponse.status.Equals(GraphConstants.Failed, StringComparison.OrdinalIgnoreCase))
                    return true;
                else return false;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Delete source collection
        /// </summary>
        /// <param name="caseID"></param>
        /// <param name="collectionID"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteCollectionAsync(AzureADConfig config, string caseID, string collectionID)
        {
            try
            {
                var DeleteSourceCollectionAPIDetails = GraphAPIEndpoints.DeleteSourceCollection;
                var url = string.Format(DeleteSourceCollectionAPIDetails.APIEndPoint, config.BaseURL, "v1.0", caseID, collectionID);
                config.Scope = DeleteSourceCollectionAPIDetails.Scope;
                config.AuthenticationMode = "Delegated";

                var accessToken = new AzureADHelper().GetGraphToken(config).Result;
                var response = new AzureADHelper().GetHttpContentWithTokenForPollingHelperAsync(url, accessToken).Result;

                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }

    }
}
