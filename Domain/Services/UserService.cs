using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InPr.Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace InPr.Domain.Services
{
    
    public class UserService
    {
        NewsDbContext db;
        IConfiguration appConfig;
        public UserService(IConfiguration appConfig, NewsDbContext db){
            this.db = db;
            this.appConfig = appConfig;
        }
        public async Task<string> LoginAsync(AuthModel auth){
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Name == auth.Name && u.Password == auth.Password);
;           
            if(user != null && user.UserRole != null && user.UserRole.Name != null && user.Email != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole.Name),
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
            return "login error";
             
        }
        
        public async Task<string> RegistationAsync(UserModel usermodel, string roleName){
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Name == usermodel.Name);
            if(user != null)
            {
                return "пользователь с таким логином уже существует";
            }
            user = await db.Users.FirstOrDefaultAsync(u => u.Email == usermodel.Email);
            if(user != null)
            return "пользователь с таким email уже существует";
            user = new User{
                Name = usermodel.Name,
                Age = usermodel.Age,
                Password = usermodel.Password,
                Verified_Email = false,
                Email = usermodel.Email
                };
                if(roleName =="publisher")
                {
                    user.UserRole = await db.Roles.FirstOrDefaultAsync((r)=> r.Name == roleName);
                }
                else
                {
                    user.UserRole = await db.Roles.FirstOrDefaultAsync((r)=> r.Name == "reader");
                }
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();            

            if(user != null 
            && user.UserRole != null 
            && user.UserRole.Name != null 
            && user.Email != null){
                var claims = new List<Claim> { 

                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole.Name),
                    new Claim("id",user.id.ToString())

                };
                    var jwt = new JwtSecurityToken(
                    issuer: appConfig["JwtToken:ISSUE"],
                    audience: appConfig["JwtToken:AUDIENCE"],
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(3)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appConfig["JwtToken:KEY"])), SecurityAlgorithms.HmacSha256));
                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                    return encodedJwt;
            }
            return "registration error";
        }
        
         
        public async Task<User?> GetUserAsync(int id){
            User? user = await db.Users.FirstOrDefaultAsync(u => u.id == id);
            if(user != null)
            user.Password = "";
            return user;
        }
        
        public async Task<User?> GetUserAsync(string Name){
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Name == Name);
            if(user != null)
            user.Password = "";
            return user;

        }
         
        public async Task<List<Article>> GetArticlesAsync(string Name){
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Name == Name);
            if(user != null)
            return user.Articles;
            else
            return new();
        }
        
        public async Task<Role?> GetRoleAsync(string Name)
        {
            Role? role = await db.Roles.FirstOrDefaultAsync((r)=> r.Name == Name);
            return role;
        }
        
        

       
    }
}