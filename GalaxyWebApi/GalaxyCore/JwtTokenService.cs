using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using GalaxyCore.Contracts;
using GalaxyCore.Models;
using GalaxyDto;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GalaxyCore
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IOptionsMonitor<CarsBLLOptions> _options;

        public JwtTokenService(IOptionsMonitor<CarsBLLOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public TokenDto GenerateToken(string username)
        {
            var utcNow = DateTime.UtcNow;
            var expires = utcNow.AddMonths(1);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_options.CurrentValue.JwtSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                NotBefore = utcNow,
                Subject = new ClaimsIdentity(new []
                {
                    new Claim("dt", utcNow.ToString("yyyy-MM-ddTHH:mm:ssK")),
                    new Claim("username", username)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var finalToken = tokenHandler.WriteToken(token);

            return new TokenDto
            {
                username = username,
                access_token = finalToken,
                token_type = "Bearer",
                expires_in = (expires - utcNow).TotalSeconds
            };
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.CurrentValue.JwtSecretKey))
            };

            try
            {
                _ = tokenHandler.ValidateToken(token, validationParameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
