using System.Runtime.Serialization;

namespace Unified.Connectors.Constants
{
    /// <summary>
    /// Graph constants
    /// </summary>
    public class GraphConstants
    {
        public const string Mailbox = "mailbox";
        public const string Site = "site";
        public const string Directory = "directory";
        public const string Pst = "pst";
        public const string Succeeded = "succeeded";
        public const string Failed = "failed";
        public const string DocumentsPartInWebUrl = "/Documents";
        public const string CollectionPrefix = "COL";
        public const string ReviewsetPrefix = "RVST_";
        public const string ExportPrefix = "EXP_";
        public const string OdataType_SiteSource = "microsoft.graph.ediscovery.siteSource";
        public const string OdataType_UserSource = "microsoft.graph.ediscovery.userSource";
        public const string SharedDocuments = "Shared%20Documents";

    }

    /// <summary>
    /// Data Source Scopes while creating source collection
    /// </summary>
    public enum DataSourceScopes
    {
        [EnumMember(Value = "allCaseNoncustodialDataSources")]
        AllCaseNoncustodialDataSources,
        [EnumMember(Value = "none")]
        None,

    }
}
