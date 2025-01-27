using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using VaultSharp;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.UserPass;
using VaultSharp.V1.Commons;

namespace Unified.Connectors.Helpers
{
    /// <summary>
    /// HashiVault Helper
    /// </summary>
    public class HashiVaultHelper
    {
        /// <summary>
        /// Get Config
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetConfigAsync(string secretPath)
        {
            try
            {
                string baseUrl = "https://vault.us-east-1.dev.revealglobal.cloud/";
                string mountName = "unifiedkv2";
                string username = "vgodhari";
                string password = "Vertical@123";

                var authMethod = new UserPassAuthMethodInfo(username, password);

                var vaultClientSettings = new VaultClientSettings(baseUrl, authMethod);

                var vaultClient = new VaultClient(vaultClientSettings);


                var mounts = vaultClient.V1.System.GetSecretBackendsAsync().GetAwaiter().GetResult();
                if (mounts != null && mounts.Data.ContainsKey(mountName + "/"))
                {
                    var mountInfo = mounts.Data[mountName + "/"];

                    Secret<SecretData> secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(secretPath, null, mountInfo.Path);

                    var secretDicts = secret?.Data?.Data.ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.ToString());
                    if (secretDicts != null)
                    {
                        var config = JsonConvert.SerializeObject(secretDicts);
                        return config;
                    }
                    else
                    {
                        throw new Exception("No Data Found");
                    }
                }
                else
                {
                    throw new Exception("Mount Data Found");
                }
            }
            catch (VaultApiException)
            {
                throw new Exception("Error Processing while connecting to Hashi Vault");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vaultUrl"></param>
        /// <param name="authPath"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public  async Task<string> GenerateTokenAsync(string vaultUrl, string authPath, string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(vaultUrl);

                var payload = new
                {
                    password = password
                };

                string jsonPayload = JsonConvert.SerializeObject(payload);
                string url = $"{authPath}/login/{username}";

                HttpResponseMessage response = await client.PostAsync(url,
                    new StringContent(jsonPayload, Encoding.UTF8, "application/json")
                );

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    JObject responseObject = JsonConvert.DeserializeObject<JObject>(responseBody);
                    string token = responseObject["auth"]?["client_token"]?.ToString();

                    return token;
                }
                else
                {
                    string errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception("Error while Generating Token");
                }
            }
        }
    }
}
