using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WalletApp.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

string connString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DBClass>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();
app.MapGet("/security/getMessage", () => "Hello World!").RequireAuthorization();
app.MapPost("/security/createToken",
[AllowAnonymous] (UserCredential user) =>
{
    if (user.UserName == "Sohag" && user.Password == "123456")
    {
        var issuer = builder.Configuration["Jwt:Issuer"];
        var audience = builder.Configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
        (builder.Configuration["Jwt:Key"]);

        /*
        var key = Encoding.ASCII.GetBytes
        (builder.Configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        var stringToken = tokenHandler.WriteToken(token);
        return Results.Ok(stringToken);
        */

        var token=JwtGenerator.GenerateJwtToken("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\r\nzbXa9rPgSxQY6saqksyxB4MQCo9rSA==", issuer, audience, 1);
        return Results.Ok(token);
    }
    return Results.Unauthorized();
});

app.MapControllers();

app.Run();
