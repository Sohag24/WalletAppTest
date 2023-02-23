using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

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

        public GatWayController(ILogger<GatWayController> logger)
        {
            _logger = logger;
        }


        [HttpPost(Name = "SendWeatherForecast")]
        public IEnumerable<WeatherForecast> Post(int flag)
        {
            if (flag == 1)
            {
                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
            }
            else
            {
                WeatherForecast[] a = {};
                return a.ToArray();
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