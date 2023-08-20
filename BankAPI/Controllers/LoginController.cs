using BankAPI.BankModels;
using BankAPI.DTOs;
using BankAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService loginService;
        private IConfiguration config;
        public LoginController(LoginService service, IConfiguration config)
        {
            this.loginService = service;
            this.config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminDTO adminDto)
        {
            var admin = await loginService.GetAdmin(adminDto);

            if (admin is null)
            {
                return BadRequest(new {message = "Credenciales invalidas."});
            }

            string jwtToken = GenerateToken(admin);

            return Ok(new {token = jwtToken});
        }

        [HttpPost("logClient")]
        public async Task<IActionResult> LoginClient(ClientDTO clientDto)
        {
            var client = await loginService.GetClient(clientDto);

            if (client is null)
            {
                return BadRequest(new { message = "Credenciales invalidas." });
            }

            string jwtToken = GenerateTokenClient(client);

            return Ok(new { token = jwtToken });
        }

        private string GenerateTokenClient(Client client)
        {
            var claims = new[]
{
                new Claim(ClaimTypes.Name, client.Name!),
                new Claim(ClaimTypes.Email, client.Email!)
              
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value!)
                );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        private string GenerateToken(Administrator admin)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, admin.Name),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim("AdminType",admin.AdminType)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value!)
                );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}
