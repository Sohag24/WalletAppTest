using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WalletApp.Model;

namespace WalletApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DBClass _dbContext;


        public AuthController(IConfiguration configuration, DBClass dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // Validate the user credentials here (e.g. verify username and password)

            var userRepository = new Repository<User>(_dbContext);
            var Users= userRepository.GetAllAsync().Result;
            var user=Users.Where(a=>a.Email== loginDto.Username && a.Password== loginDto.Password).FirstOrDefault();

            if (user==null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, loginDto.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                expiration = token.ValidTo
            });
        }

        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            // Validate the registration data here (e.g. check if username already exists)

            // Create the user account here (e.g. add the user to the database)

            var newUser = new User() { Name = registrationDto.Name, Email = registrationDto.Email,Password=registrationDto.Password };
            var userRepository = new Repository<User>(_dbContext);
            var savedUser = await userRepository.SaveAsync(newUser);

            var rabbitMQ = new RabbitMQHelper("localhost", "guest", "guest");
            rabbitMQ.PublishMessage("New User Created! Id: "+newUser.Id.ToString()+" Name: "+newUser.Name, "myqueue");

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("ReceiveMessage")]
        public async Task<string> ReceiveMessage()
        {
            string message = "";
            var rabbitMQ = new RabbitMQHelper("localhost", "guest", "guest");
            message = await rabbitMQ.ReceiveMessages("myqueue");

            return message==""?"No Message": message;
        }

    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegistrationDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
