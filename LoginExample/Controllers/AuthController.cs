using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LoginExample.Data;
using LoginExample.Helpers;
using LoginExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LoginExample.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private readonly IConfiguration _Config;
        private readonly IOptions<Appsetting> _appSettings;

        public AuthController(IUserService userService, IConfiguration Config, IOptions<Appsetting> appSettings)
        {
            _appSettings = appSettings;
            _Config = Config;
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(UserLogin model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier,model.Id.ToString()),
                    new Claim(ClaimTypes.Name, model.Username)
            };
            var key = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(_Config.GetSection("AppSettings:Secret").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(1)
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { token = tokenHandler.WriteToken(token) , response.Username , response.Password});
        }
    }
}