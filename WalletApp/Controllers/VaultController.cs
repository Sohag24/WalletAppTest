using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WalletApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaultController : ControllerBase
    {
        // GET: api/<VaultController>
        //[HttpGet]
        //public async Task<string> Get()
        //{
        //    var baseurl = "https://api.fireblocks.io/";
        //    RestApiClient ApiClient = new RestApiClient(baseurl);
        //    var token = JwtGenerator.GenerateJwtToken("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\r\niLG15UJPY0pJhGq8cHB1wjsFi/5G6g==", issuer, audience, 1);
        //    return await ApiClient.PostAsync("/v1/vault/accounts", "Test", "MyHeader", "1");
        //}

        // GET api/<VaultController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<VaultController>
        [HttpPost]
        public async Task<string> Post([FromBody] JsonElement body)
        {
            string value = System.Text.Json.JsonSerializer.Serialize(body);

            var baseurl = "https://api.fireblocks.io/";
            RestApiClient ApiClient = new RestApiClient(baseurl);
            var token = JwtGenerator.GenerateJwtToken("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\r\niLG15UJPY0pJhGq8cHB1wjsFi/5G6g==", "", "", 1,value);
            return await ApiClient.PostAsync("/v1/vault/accounts", token, "X-API-Key", "31b2497e-5296-85a6-e57e-67180f647155", value);
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
