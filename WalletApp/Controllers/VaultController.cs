using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WalletApp.Helper;
using WalletApp.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WalletApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaultController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VaultController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //GET: api/<VaultController>
        [HttpGet]
        public  string Get()
        {
            return  "";
        }

        // GET api/<VaultController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VaultController>
        [HttpPost("CreateVault")]
        public async Task<string> CreateVault([FromBody] JsonElement body)
        {
            Configuration con = new Configuration(_configuration);
            ConfigurationDTO configuration = con.getConfiguration();

            string uri = "/v1/vault/accounts";
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            string key= configuration.Key;
            string baseurl = configuration.FireBlocks_BaseURL;
            var token = JwtGenerator.GenerateJwtToken(uri, BodyStr, key);

            RestApiClient ApiClient = new RestApiClient(baseurl);
            return await ApiClient.PostAsync(uri, token, "X-API-Key", key, BodyStr);
        }

        // PUT api/<VaultController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VaultController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
