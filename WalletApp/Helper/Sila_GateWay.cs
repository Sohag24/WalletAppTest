using Microsoft.AspNetCore.Mvc;
using Sila.API.Client.Domain;
using Sila.API.Client.Entities;
using SilaAPI.silamoney.client.api;
using SilaAPI.silamoney.client.domain;
using System;
using System.Reflection.Metadata;
using System.Text.Json;
using Environments = SilaAPI.silamoney.client.domain.Environments;

class Sila_GateWay
{
    string privateKey = "83d6338ca0759cc54d3b278f60fe3bb8c67ebd8c83d606d5cf9478429bc1acc2";
    string appHandle = "test_app_jobayer_123";//"app.silamoney.eth";
    bool debug = false;  // To enable/disable debug you can pass true (debug enabled) or false (debug disabled).
    SilaApi api;
    public Sila_GateWay()
    {
        api = new SilaApi(Environments.SANDBOX, privateKey, appHandle, debug);
    }
    public string GenerateWallet()
    {
        // Generate wallet
        var wallet = api.GenerateWallet();
        var obj = new
        {
            Address = wallet.Address,
            PrivateKey = wallet.PrivateKey
        };
        var json = JsonSerializer.Serialize(obj);
        return json;
    }

    public string CheckHandle(string userHandle)
    {
        var handle= api.CheckHandle(userHandle);
        var obj = new
        {
            Data=handle.Data
        };
        var json = JsonSerializer.Serialize(obj);
        return json;
    }

    public string RegisterUser(dynamic userData)
    {
        User user = new User();
        user.UserHandle = userData.UserHandle;
        user.FirstName = userData.FirstName;
        user.LastName = userData.LastName;
        user.AddressAlias = userData.AddressAlias;
        user.StreetAddress1 = userData.StreetAddress1;
        user.StreetAddress2 = userData.StreetAddress2;
        user.City = userData.City;
        user.State = userData.State;
        //user.zip = '12345';
        user.Country = userData.Country;
        user.ContactAlias = userData.ContactAlias;
        user.Phone = userData.Phone;
        user.Email = userData.Email;
        user.Birthdate = userData.Birthdate;
        user.IdentityValue = userData.IdentityValue;
        user.CryptoAlias = userData.CryptoAlias;
        user.CryptoAddress = userData.CryptoAddress;
        user.Type = userData.Type;


        user.DeviceFingerprint = userData.DeviceFingerprint;
        user.SessionIdentifier = userData.SessionIdentifier;

        var register = api.Register(user);
        var obj = new
        {
             Data=register.Data
        };
        var json = JsonSerializer.Serialize(obj);
        return json;
    }

    public string RequestKYC(string userHandle,string userPrivateKey)
    {
        var kyc = api.RequestKYC(userHandle,userPrivateKey);
        var obj = new
        {
            Data = kyc.Data
        };
        var json = JsonSerializer.Serialize(obj);
        return json;
    }

    public string CheckKYC(string userHandle, string userPrivateKey)
    {
        var kyc = api.CheckKYC(userHandle, userPrivateKey);
        var obj = new
        {
            Data = kyc.Data
        };
        var json = JsonSerializer.Serialize(obj);
        return json;
    }

    public string PlaidLinkToken(string userHandle, string userPrivateKey)
    {
        var Link = api.PlaidLinkToken(userHandle, userPrivateKey);
        var obj = new
        {
            Data = Link.Data
        };
        var json = JsonSerializer.Serialize(obj);
        return json;
    }

    public string LinkAccount(string userHandle,string publicToken, string userPrivateKey,string accountName,string accountId,string plaidInTokenType)
    {
        var Link = api.LinkAccount(userHandle, publicToken, userPrivateKey,accountName,accountId,plaidInTokenType);
        var obj = new
        {
            Data = Link.Data
        };
        var json = JsonSerializer.Serialize(obj);
        return json;
    }

    public string GetEntities()
    {
        var Entity = api.GetEntities();
        var obj = new
        {
            Data = Entity.Data
        };
        var json = JsonSerializer.Serialize(obj);
        return json;
    }


}