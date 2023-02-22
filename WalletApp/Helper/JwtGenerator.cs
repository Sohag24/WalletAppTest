using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

public class JwtGenerator
{
    public static string GenerateJwtToken(string base64PrivateKey, string issuer, string audience, int expirationMinutes)
    {
        var rsa = new RSACryptoServiceProvider();
        var privateKeyBytes = Convert.FromBase64String(base64PrivateKey);
        rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, "myUsername")
    };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

}