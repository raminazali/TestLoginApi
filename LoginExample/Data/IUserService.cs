using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LoginExample.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using LoginExample.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace LoginExample.Data
{
    public interface IUserService
    {
        UserLogin Authenticate(UserLogin model);
        IEnumerable<UserLogin> GetAll();
        UserLogin GetById(int id);
    }
}