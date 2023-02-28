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

    }
}
