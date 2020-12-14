﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using LetsDoStuff.WebApi.SettingsForAuth;
using LetsDoStuff.WebApi.SettingsForAuthJwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LetsDoStuff.WebApi.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly LdsContext context;

        public AccountController(LdsContext context)
        {
            this.context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest userAuthParam)
        {
            var identity = GetIdentity(userAuthParam.Login, userAuthParam.Password);

            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid login or password" });
            }

            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                login = identity.Name
            };

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string login, string password)
        {
            var user = context.Users.FirstOrDefault<User>(x => x.Email == login && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(UserClaimIdentity.DefaultNameClaimType, user.Email),
                        new Claim(UserClaimIdentity.DefaultRoleClaimType, user.Role),
                        new Claim(UserClaimIdentity.DefaultIdClaimType, user.Id.ToString())
                    };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims);
                return claimsIdentity;
            }

            return null;
        }
    }
}