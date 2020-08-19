using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;

namespace LiquidQuoine.Net
{
    public class LiquidQuoineAuthenticationProvider : AuthenticationProvider
    {
        public LiquidQuoineAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
            
        }
  
        public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postParameterPosition, ArrayParametersSerialization arraySerialization)
        {
            var result = new Dictionary<string, string>();
            result.Add("X-Quoine-API-Version", "2");
            if (!signed)
                return result;
            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(Credentials.Secret.GetString()));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);
            var payload = new JwtPayload
            {
                { "path", uri },
                { "nonce", (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds },
                { "token_id", Credentials.Key.GetString() }
            };
            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);
            result.Add("X-Quoine-Auth", tokenString);
            return result;
        }

    }
}
