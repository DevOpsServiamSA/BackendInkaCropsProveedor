using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProveedorApi.Helpers;
using ProveedorApi.Models;

namespace ProveedorApi.Auth;

public class TokenAuth
{
    private readonly IConfiguration _config;
    public TokenAuth(IConfiguration config)
    {
        _config = config;
    }

    public string Token(TokenModel tokenModel)
    {
        DateTime expire_token = UtilityHelper.ExpireToken(_config["ExpiresToken"]);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    /*[0]*/new Claim(ClaimTypes.SerialNumber, tokenModel.ruc??""),
                    /*[1]*/new Claim(ClaimTypes.NameIdentifier,  tokenModel.razonsocial??""),
                    /*[2]*/new Claim(ClaimTypes.Name, tokenModel.username),
                    /*[3]*/new Claim(ClaimTypes.Actor, tokenModel.nombre),
                    /*[4]*/new Claim(ClaimTypes.Role, tokenModel.perfil_id.ToString()),
                    /*[5]*/new Claim("isprov", tokenModel.isproveedor ? "true" : "false"),                    
                    /*[5]*/new Claim("readonly", tokenModel.read_only ? "true" : "false"),
                    /*[6]*/new Claim("hash", Guid.NewGuid().ToString()),
                    /*[7]*/new Claim("api", "proveedorapi")
            }),
            Expires = expire_token,
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}