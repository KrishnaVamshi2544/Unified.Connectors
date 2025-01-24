using Unified.Connectors.Model;

namespace Unified.Connectors.Constants
{
    public static class GraphAPIEndpoints
    {
        public static GraphAPIModel GetToken = new GraphAPIModel
        {
            APIEndPoint = "https://login.microsoftonline.com/{0}/oauth2/v2.0/token",
            Scope = ".default",
            isDelegated = false
        };

        public static GraphAPIModel GetUserDetails = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/users/{2}",
            Scope = "User.Read.All",
            isDelegated = false
        };

        public static GraphAPIModel GetUserMailboxes = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/users/{2}/outlook",
            Scope = "User.Read.All",
            isDelegated = false
        };

        public static GraphAPIModel GetUserOneDrive = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/users/{2}/drive",
            Scope = "User.Read.All",
            isDelegated = false
        };

        public static GraphAPIModel GetCases = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel CreateCase = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel GetCaseCustodian = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/custodians?$filter=email eq '{3}'",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel AddCustodianToCase = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/custodians",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel GetCaseNonCustodialDataSources = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/noncustodialDataSources",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel GetCaseNonCustodialDataSourceById = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/noncustodialDataSources/{3}",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel GetNonCustodialDataSourceById = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/noncustodialDataSources/{3}/datasource",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel AddNonCustodialDataSourceToCase = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/noncustodialDataSources",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel GetHoldByName = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/legalHolds?$filter=displayName eq '{3}'",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel CreateLegalHold = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/legalHolds",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel AddUserSourceToHold = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/legalHolds/{3}/userSources",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel AddSiteSourceToHold = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/legalHolds/{3}/siteSources",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel DeleteUserSourceFromHold = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/legalHolds/{3}/userSources/{4}",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel DeleteSiteSourceFromHold = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/legalHolds/{3}/siteSources/{4}",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel GetHoldUserSourceByEmail = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/legalHolds/{3}/userSources?$filter=email eq '{4}'",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel GetHoldSiteSourceByEmail = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/legalHolds/{3}/siteSources",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel UpdateLegalHold = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/legalHolds/{3}",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel GetCustodianUserSourceById = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/custodians/{3}/userSources/{4}",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel GetCustodianUserSourceByEmail = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/custodians/{3}/userSources?$filter=email eq '{4}'",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel GetCustodianUserSourceByWebUrl = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/custodians/{3}/userSources?$filter=sitewebUrl eq '{4}'",
            Scope = "Ediscovery.Read.All",
            isDelegated = true
        };

        public static GraphAPIModel AddUserSourceToCustodian = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/custodians/{3}/userSources",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel GetSourceCollectionList = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/searches",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel GetSourceCollection = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/searches/{3}",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel CreateSourceCollection = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/searches",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel AddNonCustodialDataSourcesToCollection = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/searches/{3}/noncustodialSources/$ref",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };
        public static GraphAPIModel GetNonCustodialDataSourcesFromCollection = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/searches/{3}/noncustodialSources",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };
        public static GraphAPIModel UpdateSourceCollection = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/searches/{3}",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel DeleteSourceCollection = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/searches/{3}",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel CreateEstimateStats = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/searches/{3}/estimateStatistics",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };
        /// <summary>
        /// Get All Review Sets
        /// </summary>
        public static GraphAPIModel GetAllReviewSets = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/reviewSets",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel GetReviewSet = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/reviewSets?$filter=displayName eq '{3}'",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel CreateReviewSet = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/reviewSets",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel AddReviewSetToSourceCollection = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/reviewSets/{3}/addToReviewSet",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel CreateExport = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/reviewSets/{3}/export",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };

        public static GraphAPIModel GetDownloadUrl = new GraphAPIModel
        {
            APIEndPoint = "/microsoft.graph.ediscovery.caseExportOperation/getDownloadUrl"
        };

        public static GraphAPIModel GetUserSP = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/sites",
            Scope = "Sites.Read.All",
            isDelegated = false
        };
        public static GraphAPIModel GetMemberOf = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/users/{2}/memberOf",
            Scope = "GroupMember.Read.All",
            isDelegated = false

        };

        public static GraphAPIModel GetCaseOperationforDownload = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/security/cases/ediscoveryCases/{2}/operations/{3}",
            Scope = "eDiscovery.ReadWrite.All",
            isDelegated = true
        };
        public static GraphAPIModel GetSPAdminUsers = new GraphAPIModel
        {
            APIEndPoint = "{0}{1}/sites/{2}/lists/User Information List/items?expand=fields",
            Scope = "eDiscovery.Read.All",
            isDelegated = true
        };
    }
}
