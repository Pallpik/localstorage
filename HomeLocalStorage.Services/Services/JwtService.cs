using HomeLocalStorage.Core.Models;
using Microsoft.CodeAnalysis.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HomeLocalStorage.Services.Services
{
    public class JwtService(IOptions<AuthSettings> options)
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new("login", user.Login),
                new("name", user.Name),
                new("role", user.Role),
                new("sub", user.Id.ToString()),

            };

            var jwtToken = new JwtSecurityToken(
                
                expires: DateTime.UtcNow.Add(options.Value.Expires),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                    SecurityAlgorithms.HmacSha256)
                 );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
