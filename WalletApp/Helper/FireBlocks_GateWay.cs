using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WalletApp.Model;

namespace WalletApp.Helper
{
    public  class FireBlocks_GateWay
    {
        private readonly IConfiguration _configuration;

        public FireBlocks_GateWay(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CallApi(string EndPoint,string ApiMethod,JsonElement body)
        {
            Configuration con = new Configuration(_configuration);
            ConfigurationDTO configuration = con.getConfiguration();

            string endPoint = EndPoint;
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            string key = configuration.Key;
            string baseurl = configuration.FireBlocks_BaseURL;
            var token = JwtGenerator.GenerateJwtToken(endPoint, BodyStr, key);

            RestApiClient ApiClient = new RestApiClient(baseurl);
            return await ApiClient.PostAsync(endPoint, token, "X-API-Key", key, BodyStr);
        }
    }
}
