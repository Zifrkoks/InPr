using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using System.IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using InPr.Domain.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        public async Task<AuthResultModel> LoginAsync(AuthModel auth){
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
                AuthResultModel tokenmodel = new AuthResultModel{Name = user.Name, Role = user.UserRole.Name,Token = encodedJwt};
                return tokenmodel;
            }
            else{
                if(user == null)
                 return new AuthResultModel{mes = "user equal null"};
                else if(user.UserRole == null)
                 return new AuthResultModel{mes = "user role equal null"};
                else if(user.UserRole.Name == null)
                 return new AuthResultModel{mes = "user role name equal null"};
                else if(user.Email == null)
                 return new AuthResultModel{mes = "user email equal null"};
                else
                    return new AuthResultModel{mes = "login error"};
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

        public async Task<AuthResultModel> RegistationAsync(UserModel usermodel){
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Name == usermodel.Name);
            if(user != null)
            {
                return new AuthResultModel{mes = "пользователь с таким логином уже существует"};
            }
            user = await db.Users.FirstOrDefaultAsync(u => u.Email == usermodel.Email);
            if(user != null)
            return new AuthResultModel{mes = "пользователь с таким email уже существует"};

            User newuser = new User{
                Name = usermodel.Name,
                Age = usermodel.Age,
                Password = usermodel.Password,
                Verified_Email = false,
                Email = usermodel.Email
                };
                if(usermodel.role =="publisher")
                {
                    Role? publisher = await db.Roles.FirstOrDefaultAsync((r)=> r.Name == "publisher");
                    if(publisher != null)
                    newuser.RoleId = publisher.id;
                    else{
                        Role? reader = await db.Roles.FirstOrDefaultAsync((r)=> r.Name == "reader");
                        if(reader != null){
                        newuser.RoleId = reader.id;
                    }
                    else
                    return new AuthResultModel{mes = "чё за..."}; 
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
                    AuthResultModel tokenmodel = new AuthResultModel{Name = newuser.Name, Role = newuser.UserRole.Name,Token = encodedJwt, mes = "success"};
                return tokenmodel;            
                }
            return new AuthResultModel{mes = "registration error"};
        }
        
         
        public async Task<User?> GetUserAsync(int id){
            User? user = await db.Users.FirstOrDefaultAsync(u => u.id == id);
            if(user != null)
            user.Password = "";
            return user;
        }
        
        public async Task<UserModel> GetUserAsync(string Name){
            User? user = await db.Users.Include(u=>u.UserRole).FirstOrDefaultAsync(u => u.Name == Name);
            if(user == null)
            return new UserModel{id = -1};
            UserModel usermodel = new UserModel{id = user.id, Name = user.Name, role = user.UserRole.Name, Email = user.Email, Age = user.Age};
            return usermodel;

        }
        
        public async Task<int> GetCountUsers(){
            return await db.Users.CountAsync();
        }
        public async Task<int> GetCountArticles(){
            return await db.Articles.CountAsync();
        }
        public async Task<List<ArticleModel>> GetArticlesAsync(string Name){
            User? user = await db.Users.Include(u=>u.Articles).FirstOrDefaultAsync(u => u.Name == Name);
            if(user == null)
            return new();
            List<ArticleModel> articles = new List<ArticleModel>();
            foreach(Article a in user.Articles)
            {
                ArticleModel article = new ArticleModel{id = a.id, Title = a.Title, Text = a.Text, Username = a.user.Name};
                articles.Add(article);
            }
            return articles;
            
        }
        
        public async Task<Role?> GetRoleAsync(string Name)
        {
            Role? role = await db.Roles.FirstOrDefaultAsync((r)=> r.Name == Name);
            return role;
        }
        
        

       
    }
}