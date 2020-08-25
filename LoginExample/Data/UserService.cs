using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using LoginExample.Helpers;
using LoginExample.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LoginExample.Data
{
    public class UserService : IUserService
    {
        private readonly Appsetting _appSettings;
        private readonly ApplicationDbContext _db;

        public UserService(IOptions<Appsetting> appSettings, ApplicationDbContext db)
        {
            _db = db;
            _appSettings = appSettings.Value;
        }

        public UserLogin Authenticate(UserLogin model)
        {
            var user = _db.UserLogin.FirstOrDefault
            (x => x.Username == model.Username && x.Password == model.Password);
            if (user == null) return null;

            return user;
        }

        public IEnumerable<UserLogin> GetAll()
        {
            return _db.UserLogin.ToList();
        }

        public UserLogin GetById(int id)
        {
            return _db.UserLogin.FirstOrDefault(x => x.Id == id);
        }


        private string generateJwtToken(UserLogin user)
        {
            var claims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("AppSettings:Secret"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = creds,
                Expires = DateTime.Now.AddDays(1)
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}