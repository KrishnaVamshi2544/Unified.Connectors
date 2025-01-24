namespace Unified.Connectors.Model
{
    public class AzureADConfig
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthenticationMode { get; set; }
        public string BaseURL { get; set; }
        public string Scope { get; set; }

    }
}
