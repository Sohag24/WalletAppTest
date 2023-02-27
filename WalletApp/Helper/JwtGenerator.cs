using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WalletApp.Migrations;

public class JwtGenerator
{
    public static string GenerateJwtTokenOLD(string base64PrivateKey, string issuer, string audience, int expirationMinutes,string body)
    {
        var rsa = new RSACryptoServiceProvider();
        var privateKeyBytes = Convert.FromBase64String(base64PrivateKey);
        rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
        /*
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
        return tokenHandler.WriteToken(token);*/

        // Set the expiration and issued at times
        /*
        DateTime issuedAt = DateTime.Now;
        DateTime expiresAt = issuedAt.AddSeconds(30);

        var issueAtepoch = ConvertToTimestamp(issuedAt);
        var expiresAtepoch = ConvertToTimestamp(expiresAt);*/

        //DateTime issuedAt = DateTime.UtcNow;

        

        DateTime utcTime = DateTime.UtcNow;
        //TimeZoneInfo BdZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        DateTime issuedAt = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
        //issuedAt = issuedAt.AddHours(-18);
        //issuedAt= issuedAt.AddSeconds(-50000);
        // Set the expiration time to 30 seconds after the issue time

        DateTime expiresAt =TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone); //issuedAt.AddSeconds(30);
                                                                                //expiresAt.AddHours(6);
       // expiresAt = expiresAt.AddHours(-6);
        //expiresAt = expiresAt.AddHours(-18);
        expiresAt =expiresAt.AddSeconds(30);

        // Convert the issue time and expiration time to Unix time
        //int issueAtepoch = (int)(issuedAt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        //int expiresAtepoch = (int)(expiresAt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        byte[] messageBodyBytes = Encoding.UTF8.GetBytes(body);
        string hashedString = "";
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] hashedBytes = sha256Hash.ComputeHash(messageBodyBytes);

             hashedString = Convert.ToBase64String(hashedBytes);

            // Use the hashedString variable as needed
        }

        

        DateTimeOffset iat = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);

        DateTimeOffset exp = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone); ;
        exp= exp.AddSeconds(30);
        /*
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
        // Get the current time in Bangladesh Standard Time
        DateTime bdTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);
        // Format the date/time string
        string bdTimeString = bdTime.ToString("ddd MMM dd yyyy HH:mm:ss 'GMT'zzz ' (Bangladesh Standard Time)'");
        iat = DateTimeOffset.ParseExact(bdTimeString, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz ' (Bangladesh Standard Time)'", CultureInfo.InvariantCulture);
        //long unixTimestamp = (long)bdTime2.ToUnixTimeSeconds();

        */

/*
        DateTime bdTimeExp = bdTime.AddSeconds(30);
        // Format the date/time string
        string bdTimeStringExp = bdTimeExp.ToString("ddd MMM dd yyyy HH:mm:ss 'GMT'zzz ' (Bangladesh Standard Time)'");
        exp = DateTimeOffset.ParseExact(bdTimeStringExp, "ddd MMM dd yyyy HH:mm:ss 'GMT'zzz ' (Bangladesh Standard Time)'", CultureInfo.InvariantCulture);
*/


        // Create the JWT token
        var claims = new[] {
    new Claim("sub", "31b2497e-5296-85a6-e57e-67180f647155"), // Add additional claims as needed
    new Claim("nonce", Guid.NewGuid().ToString()), // Add a unique nonce claim
    new Claim("iat", iat.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer),
    new Claim("exp", exp.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer),
    new Claim("uri","/v1/vault/accounts"),
    new Claim("bodyHash",hashedString)
};

        var identity = new ClaimsIdentity(claims);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            IssuedAt = issuedAt,
            Expires = expiresAt,
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Get the JWT token string
        return tokenHandler.WriteToken(token);



    }



    public static long ConvertToTimestamp(DateTime value)
    {
        long epoch = (value.Ticks - 621355968000000000) / 10000000;
        return epoch;
    }



    public static string GenerateJwtToken(string base64PrivateKey, string issuer, string audience, int expirationMinutes, string body)
    {
        var UTCDateTime = GetUTCDateTime();
        // String BodyStr = JsonConvert.SerializeObject(body);

        int iats = (int)(UTCDateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        int exps = (int)(UTCDateTime.AddSeconds(30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

        /*
        // Define the payload of the JWT
        var payload = new
        {
            sub = "31b2497e-5296-85a6-e57e-67180f647155",
            nonce = "hello",
            iat = iats,
            exp = exps,
            uri = "/v1/vault/accounts",
            bodyHash = GetBase64Sha256Hash(body)
        };
        */

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
        {
        new Claim("sub", "31b2497e-5296-85a6-e57e-67180f647155"),
        new Claim("nonce",Guid.NewGuid().ToString()),
        new Claim("iat",iats.ToString(),ClaimValueTypes.Integer),
        new Claim("exp",exps.ToString(),ClaimValueTypes.Integer),
        new Claim("uri","/v1/vault/accounts"),
        new Claim("bodyHash",GetBase64Sha256Hash(body))
      });

        // Convert the payload to a JSON string
        //var payloadJson = JsonConvert.SerializeObject(payload);

        // Load the private key from a file or some other secure storage location
        var privateKey = LoadPrivateKey();

        // Create the signing credentials using the RSA256 algorithm and the private key
        var signingCredentials = new SigningCredentials(new RsaSecurityKey(privateKey), SecurityAlgorithms.RsaSha256);

        /*
        // Create the JWT token with the payload and signing credentials
        var jwtToken = new JwtSecurityToken(
            issuer: "Sohag",//"https://clearcryptos.com/",
            audience: "Sam",//"clearcryptos.com",
            claims: new[] { new Claim("payload", payloadJson) },
            notBefore: UTCDateTime,//DateTime.UtcNow,
            expires: UTCDateTime.AddSeconds(30),//DateTime.UtcNow.AddSeconds(30),
            signingCredentials: signingCredentials
        );
        */

        var tokenHandler = new JwtSecurityTokenHandler();

        //create the jwt
        var token = (JwtSecurityToken)
                tokenHandler.CreateJwtSecurityToken(issuer: "Sam", audience: "sohag",
                    subject: claimsIdentity, notBefore: UTCDateTime,issuedAt:UTCDateTime, expires: UTCDateTime.AddSeconds(30), signingCredentials: signingCredentials);
        var tokenString =tokenHandler.WriteToken(token);

        return tokenString;

        // Encode the JWT token as a string
        //return new JwtSecurityTokenHandler().WriteToken(jwtToken);



    }

    private static string GetBase64Sha256Hash(string input)
    {
        string hashString;
        using (var sha256 = SHA256.Create())
        {
            var hash = sha256.ComputeHash(Encoding.Default.GetBytes(input));
            hashString = ToHex(hash, false);
        }

        return hashString;
    }

    private static string ToHex(byte[] bytes, bool upperCase)
    {
        StringBuilder result = new StringBuilder(bytes.Length * 2);
        for (int i = 0; i < bytes.Length; i++)
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));
        return result.ToString();
    }


    private static RSA LoadPrivateKey()
    {
        // Replace this path with the path to your private key file
        string privateKeyFilePath = "D:/privatekey.pem";

        // Read the contents of the private key file
        string privateKeyText = File.ReadAllText(privateKeyFilePath);

        // Create a new RSA key object to hold the private key
        RSA privateKey = RSA.Create();

        // Load the private key from the file
        privateKey.ImportFromPem(privateKeyText);

        return privateKey;
    }

    public static DateTime GetUTCDateTime()
    {
        var httpClient = new HttpClient();

        // Set the URL of the World Time API endpoint
        var apiUrl = "http://worldtimeapi.org/api/timezone/Asia/Dhaka";

        // Send an HTTP GET request to the API endpoint and get the response
        var response = httpClient.GetAsync(apiUrl).Result;

        // Read the response content as a string
        var responseContent = response.Content.ReadAsStringAsync().Result;

        var jsonObject = JObject.Parse(responseContent);

        // Get the value of the "name" variable as a string
        var utc_datetime = (DateTime)jsonObject["utc_datetime"];


        // Parse the response JSON to get the current UTC datetime
        //var dateTimeUtc = JsonConvert.DeserializeObject<string>(responseContent);

        return utc_datetime;

    }

}