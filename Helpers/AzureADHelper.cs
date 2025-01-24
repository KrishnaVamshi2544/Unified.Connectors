using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using Unified.Connectors.Constants;
using Unified.Connectors.Model;

namespace ExchangePOC.Helpers
{
    public class AzureADHelper
    {
        /// <summary>
        /// Get Access Token using Client Credentials
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetGraphToken(AzureADConfig azureADConfig)
        {
            string accessToken = string.Empty;
            List<KeyValuePair<string, string>> oAuth2Parameters = new List<KeyValuePair<string, string>>();

            var Scope = azureADConfig.Scope;

            if (azureADConfig.AuthenticationMode == "ClientSecret")
            {
                oAuth2Parameters.Add(new KeyValuePair<string, string>(ADConstants.GRANT_TYPE, ADConstants.GRANT_TYPE_VALUE));
                oAuth2Parameters.Add(new KeyValuePair<string, string>(ADConstants.CLIENT_ID, azureADConfig.ClientId));
                oAuth2Parameters.Add(new KeyValuePair<string, string>(ADConstants.CLIENT_SECRET, azureADConfig.ClientSecret));
                oAuth2Parameters.Add(new KeyValuePair<string, string>(ADConstants.SCOPE, ADConstants.CLIENTFLOW_SCOPE));

            }
            else if (azureADConfig.AuthenticationMode == "Delegated")
            {
                if (!Scope.Contains(ADConstants.GRAPH_MULTIFILE_DOWNLOAD_SCOPE))
                    Scope = Scope.Contains(azureADConfig.BaseURL) ? Scope : (azureADConfig.BaseURL + Scope);
                oAuth2Parameters.Add(new KeyValuePair<string, string>(ADConstants.GRANT_TYPE, ADConstants.PASSWORDFLOW_GRANT_TYPE));
                oAuth2Parameters.Add(new KeyValuePair<string, string>(ADConstants.CLIENT_ID, azureADConfig.ClientId));
                oAuth2Parameters.Add(new KeyValuePair<string, string>(ADConstants.CLIENT_SECRET, azureADConfig.ClientSecret));
                oAuth2Parameters.Add(new KeyValuePair<string, string>(ADConstants.SCOPE, Scope));
                oAuth2Parameters.Add(new KeyValuePair<string, string>(ADConstants.USERNAME, azureADConfig.UserName));
                oAuth2Parameters.Add(new KeyValuePair<string, string>(ADConstants.PASSWORD, azureADConfig.UserPassword));
            }

            using (HttpClient httpClient = new HttpClient())
            {
                var authority = string.Format(GraphAPIEndpoints.GetToken.APIEndPoint, azureADConfig.TenantId);
                FormUrlEncodedContent bodyEncode = new FormUrlEncodedContent(oAuth2Parameters);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ADConstants.MEDIATYPE_HEADER));

                HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, authority);
                msg.Content = bodyEncode;

                try
                {
                    Task<HttpResponseMessage> res = httpClient.SendAsync(msg);
                    Task<string> body = res.Result.Content.ReadAsStringAsync();
                    JObject jSon = JObject.Parse(body.Result);
                    accessToken = (string)jSon.SelectToken(ADConstants.ACCESS_TOKEN)!;

                    return accessToken!;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetHttpContentWithTokenAsync(string url, string token)
        {
            var requestUri = url;
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    response = httpClient.SendAsync(request).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                    }
                    return response;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
            }

        }

        /// <summary>
        /// Post HttpContent with Token returns the response from the server.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostHttpContentWithTokenAsync(string url, string token, string? payload = null)
        {
            var requestUri = url;
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, url);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    StringContent content = null;
                    if (!string.IsNullOrEmpty(payload))
                    {
                        content = new StringContent(payload, Encoding.UTF8, "application/json");
                        response = httpClient.PostAsync(url, content).Result;
                    }
                    else
                        response = httpClient.SendAsync(request).Result;

                    return response;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
            }
        }

        /// <summary>
        /// Get HttpContent with Token returns the response from the server For PollingHelper.
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="token">valid token</param>
        /// <returns>response</returns>
        public async Task<HttpResponseMessage> GetHttpContentWithTokenForPollingHelperAsync(string url, string token)
        {
            var requestUri = url;
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    response = httpClient.SendAsync(request).Result;
                    return response;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
            }
        }

        /// <summary>
        /// Delete HttpContent with Token returns the response from the server.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="telemetryHelper"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> DeleteHttpContentWithTokenAsync(string url, string token)
        {
            var requestUri = url;
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                HttpResponseMessage response = new HttpResponseMessage();
                try
                {
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, url);
                    //Add the token in Authorization header
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    response = httpClient.SendAsync(request).Result;
                    
                    return response;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {

                }
            }
        }
    }
}
