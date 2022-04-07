using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;
using InPr.Domain.Repositories;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.Cookies;
namespace InPr.Domain.Services
{
    
    public class AuthService
    {
        UserRepository users;
        IConfiguration appConfig;
        AuthService(UserRepository users, IConfiguration appConfig){
            this.users = users;
            this.appConfig = appConfig;
        }
        public async Task<string> Login(AuthModel auth){
            User user = await users.Read(auth);
            if(user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole?.Name),
                    new Claim("id",user.id.ToString())
                };    // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                    issuer: appConfig["JwtToken:ISSUE"],
                    audience: appConfig["JwtToken:AUDIENCE"],
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appConfig["JwtToken:KEY"])), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return encodedJwt;
            }
            else
            return null;
             
        }
        public async Task<string> Registation(UserModel usermodel, string roleName){
            User user = new User{
                Name = usermodel.Name,
                Age = usermodel.Age,
                Password = usermodel.Password,
                Verified_Email = false,
                Email = usermodel.Email
                };
            if(roleName =="publisher")
            {
                user.UserRole = await users.ReadRole(roleName);
            }
            else
                user.UserRole = await users.ReadRole("reader");
            
            if(await users.Create(user) == true){
                var claims = new List<Claim> { 
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole?.Name)
                };
                    var jwt = new JwtSecurityToken(
                    issuer: appConfig["JwtToken:ISSUE"],
                    audience: appConfig["JwtToken:AUDIENCE"],
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appConfig["JwtToken:KEY"])), SecurityAlgorithms.HmacSha256));
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                    return encodedJwt;
            }
            else{
                return null;
            }

        }
       
    }
}