using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.IdentityModel;
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
            User? user = await db.Users.Include(u => u.UserRole).FirstOrDefaultAsync(u => u.Name == auth.Name && u.Password == auth.Password);
;           
            if(user != null && user.UserRole != null && user.UserRole.Name != null && user.Email != null && user.Name != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.UserRole.Name),
                    new Claim("id",user.id.ToString())
                };    // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                    issuer: appConfig["JwtToken:ISSUER"],
                    audience: appConfig["JwtToken:AUDIENCE"],
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(3)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appConfig["JwtToken:KEY"])), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return encodedJwt;
            }
            else{
                if(user == null)
                return "user equal null";
                else if(user.UserRole == null)
                return "user role equal null";
                else if(user.UserRole.Name == null)
                return "user role name equal null";
                else if(user.Email == null)
                return "user email equal null";
                else
                    return "login error";
            }
            
             
        }

        public async Task<string> DeleteAsync(int id)
        {
            User? user = db.Users.Find(id);
            if(user == null)
                return "not found";
            await Task.Run(()=>db.Users.Remove(user));
            return "user deleted";
        }

        public async Task<List<User>> GetAllUsersAsync(int count, int page)
        {
            return await Task.Run(()=>db.Users.AsParallel().Skip(count*page).Take(count).ToList());
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

            User newuser = new User{
                Name = usermodel.Name,
                Age = usermodel.Age,
                Password = usermodel.Password,
                Verified_Email = false,
                Email = usermodel.Email
                };
                if(roleName =="publisher")
                {
                    Role? publisher = await db.Roles.FirstOrDefaultAsync((r)=> r.Name == roleName);
                    if(publisher != null)
                    newuser.RoleId = publisher.id;
                    else{
                        Role? reader = await db.Roles.FirstOrDefaultAsync((r)=> r.Name == "reader");
                        newuser.RoleId = reader.id;
                    }

                }
                else
                {
                    Role? reader = await db.Roles.FirstOrDefaultAsync((r)=> r.Name == "reader");
                    if(reader != null)
                    newuser.RoleId = reader.id;
                }
                await db.Users.AddAsync(newuser);
                await db.SaveChangesAsync();            

            if(newuser != null 
            && newuser.UserRole != null 
            && newuser.UserRole.Name != null 
            && newuser.Email != null
            && newuser.Name != null){
                var claims = new List<Claim> { 

                    new Claim(ClaimTypes.Name, newuser.Name),
                    new Claim(ClaimTypes.Role, newuser.UserRole?.Name),
                    new Claim("id",newuser.id.ToString())

                };
                    var jwt = new JwtSecurityToken(
                    issuer: appConfig["JwtToken:ISSUER"],
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