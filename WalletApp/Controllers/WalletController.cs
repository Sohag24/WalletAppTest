using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;
using WalletApp.Helper;
using WalletApp.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WalletApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        
        public WalletController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // POST api/<WalletController>
        [HttpPost("CreateVault")]
        public async Task<string> CreateVault([FromBody] JsonElement body)
        {
            FireBlocks_GateWay FG = new FireBlocks_GateWay(_configuration);
            return await FG.CallApi(EndPoints.VaultCreate,ApiMethods.Post,body);
        }

        [HttpGet("GetVaults")]
        public async Task<string> GetVaults()
        {
            JsonElement EmptyJson = new JsonElement();
            FireBlocks_GateWay FG = new FireBlocks_GateWay(_configuration);
            return await FG.CallApi(EndPoints.VaultAccounts, ApiMethods.Get, EmptyJson);
        }

        // POST api/<WalletController>
        [HttpPost("GetVaultsByVaultId")]
        public async Task<string> GetVaultsByVaultId([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic data = JObject.Parse(BodyStr);
            string endPoint = EndPoints.VaultCreate + "/" + data.vaultId;
            string? eosAccountName = data.eosAccountName;

            JsonElement EmptyJson = new JsonElement();

            FireBlocks_GateWay FG = new FireBlocks_GateWay(_configuration);
            return await FG.CallApi(endPoint, ApiMethods.Get, EmptyJson);
        }

        // Get api/<WalletController>
        [HttpGet("GetSupportedAssets")]
        public async Task<string> GetSupportedAssets()
        {
            JsonElement EmptyJson= new JsonElement();
            FireBlocks_GateWay FG = new FireBlocks_GateWay(_configuration);
            return await FG.CallApi(EndPoints.SupportedAssets, ApiMethods.Get, EmptyJson);
        }

        // POST api/<WalletController>
        [HttpPost("AddAssets")]
        public async Task<string> AddAssets([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic data = JObject.Parse(BodyStr);
            string endPoint = EndPoints.VaultCreate + "/" + data.vaultId + "/" + data.assetId;
            string? eosAccountName = data.eosAccountName;

            JsonElement newBody=new JsonElement();
            using (MemoryStream stream = new MemoryStream())
            {
                using (Utf8JsonWriter writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();

                    writer.WriteString("eosAccountName", eosAccountName);

                    writer.WriteEndObject();
                }

                byte[] json = stream.ToArray();
                JsonDocument document = JsonDocument.Parse(json);
                newBody = document.RootElement;
            }


            FireBlocks_GateWay FG = new FireBlocks_GateWay(_configuration);
            return await FG.CallApi(endPoint, ApiMethods.Post, newBody);
        }


        [HttpPost("HideVault")]
        public async Task<string> HideVault([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic data = JObject.Parse(BodyStr);
            string endPoint = EndPoints.VaultCreate + "/" + data.vaultAccountId+"/hide";        

            FireBlocks_GateWay FG = new FireBlocks_GateWay(_configuration);
            return await FG.CallApi(endPoint, ApiMethods.Post, body);
        }

        [HttpPost("UnhideVault")]
        public async Task<string> UnhideVault([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic data = JObject.Parse(BodyStr);
            string endPoint = EndPoints.VaultCreate + "/" + data.vaultAccountId + "/unhide";

            FireBlocks_GateWay FG = new FireBlocks_GateWay(_configuration);
            return await FG.CallApi(endPoint, ApiMethods.Post, body);
        }

        [HttpPost("Activate")]
        public async Task<string> Activate([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic data = JObject.Parse(BodyStr);
            string endPoint = EndPoints.VaultCreate + "/" + data.vaultAccountId+"/"+ data.assetId + "/activate";

            FireBlocks_GateWay FG = new FireBlocks_GateWay(_configuration);
            return await FG.CallApi(endPoint, ApiMethods.Post, body);
        }

        // Sila Part .............................................

        // Get api/<WalletController>
        [HttpGet("Sila/GenrateWallet")]
        public string GenrateWallet()
        {
            Sila_GateWay SG = new Sila_GateWay();
            return  SG.GenerateWallet();
        }

        // Get api/<WalletController>
        [HttpPost("Sila/CheckHandle")]
        public string CHeckHandle([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic data = JObject.Parse(BodyStr);

            Sila_GateWay SG = new Sila_GateWay();
            return SG.CheckHandle(Convert.ToString(data.userHandle));
        }

        // Get api/<WalletController>
        [HttpPost("Sila/RegisterUser")]
        public string RegisterUser([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic userData = JObject.Parse(BodyStr);

            Sila_GateWay SG = new Sila_GateWay();
            return SG.RegisterUser(userData);
        }

        // Get api/<WalletController>
        [HttpPost("Sila/RequestKYC")]
        public string RequestKYC([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic data = JObject.Parse(BodyStr);

            Sila_GateWay SG = new Sila_GateWay();
            return SG.RequestKYC(Convert.ToString(data.userHandle), Convert.ToString(data.userPrivateKey));
        }


        // Get api/<WalletController>
        [HttpPost("Sila/CheckKYC")]
        public string CheckKYC([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic data = JObject.Parse(BodyStr);

            Sila_GateWay SG = new Sila_GateWay();
            return SG.CheckKYC(Convert.ToString(data.userHandle), Convert.ToString(data.userPrivateKey));
        }

        // Get api/<WalletController>
        [HttpPost("Sila/PlaidLinkToken")]
        public string PlaidLinkToken([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic data = JObject.Parse(BodyStr);

            Sila_GateWay SG = new Sila_GateWay();
            return SG.PlaidLinkToken(Convert.ToString(data.userHandle), Convert.ToString(data.userPrivateKey));
        }

        // Get api/<WalletController>
        [HttpPost("Sila/LinkAccount")]
        public string LinkAccount([FromBody] JsonElement body)
        {
            string BodyStr = System.Text.Json.JsonSerializer.Serialize(body);
            dynamic data = JObject.Parse(BodyStr);

            Sila_GateWay SG = new Sila_GateWay();
            return SG.LinkAccount(Convert.ToString(data.userHandle), Convert.ToString(data.publicToken), Convert.ToString(data.userPrivateKey), Convert.ToString(data.accountName), Convert.ToString(data.accountId), Convert.ToString(data.plaidInTokenType));
        }

        // Get api/<WalletController>
        [HttpGet("Sila/GetEntities")]
        public string GetEntities()
        {
            Sila_GateWay SG = new Sila_GateWay();
            return SG.GetEntities();
        }


    }
}
