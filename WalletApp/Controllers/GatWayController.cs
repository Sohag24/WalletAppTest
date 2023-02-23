using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using WalletApp.Model;

namespace WalletApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GatWayController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<GatWayController> _logger;
        private readonly DBClass _dbContext;

        public GatWayController(ILogger<GatWayController> logger,DBClass dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        
        [HttpPost(Name = "SendWeatherForecast")]
        public async Task<string> Post(int flag)
        {
            try
            {
                
                
                var newUser = new User() { Name = "John Smith", Email = "john.smith@example.com" };
                //_dbContext.Users.Add(newUser);
                var userRepository = new Repository<User>(_dbContext);
                var savedUser = await userRepository.SaveAsync(newUser);
                return "Data Successfully Save";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //[HttpGet(Name = "GetPosts")]
        //public  async  Task<string> Get()
        //{

        //    HttpClient client = new HttpClient(); // Git Test 
        //   // Call asynchronous network methods in a try/catch block to handle exceptions.
        //    try
        //    {
        //        using HttpResponseMessage response = await client.GetAsync("https://jsonplaceholder.typicode.com/posts");
        //        response.EnsureSuccessStatusCode();
        //        string responseBody = await response.Content.ReadAsStringAsync();
        //        // Above three lines can be replaced with new helper method below
        //        // string responseBody = await client.GetStringAsync(uri);

        //        return responseBody;
        //    }
        //    catch (HttpRequestException e)
        //    {
        //        return "";
        //    }
        //}


        [HttpGet(Name = "GetPosts")]
        public async Task<string> Get()
        {
            var baseurl = "https://jsonplaceholder.typicode.com/";
            RestApiClient ApiClient = new RestApiClient(baseurl);
            return await ApiClient.GetAsync("posts","Test","MyHeader","1");
        }


    }
}