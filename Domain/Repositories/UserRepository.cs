using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;
using InPr.Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace InPr.Domain.Repositories
{
    //ready
    public class UserRepository
    {
         private NewsDbContext db;
        public UserRepository(NewsDbContext context)
        {
            db = context;
        }
        public async Task<Role> ReadRole(int id){
            Role role = await db.Roles.FirstOrDefaultAsync((r)=> r.id == id);
            return role;
        }
        public async Task<Role> ReadRole(string name){
            Role role = await db.Roles.FirstOrDefaultAsync((r)=> r.Name == name);
            return role;
        }
        

        public async Task<bool> Create(User userNodel){

            User user = await db.Users.FirstOrDefaultAsync(u => u.Name == userNodel.Name && u.Email == userNodel.Email);
            if (user == null)
            {
                user = new User{Name = userNodel.Name,Email = userNodel.Email,Password = userNodel.Password, Age =  userNodel.Age, UserRole = userNodel.UserRole};
            user.Name = userNodel.Name;
            user.Email = userNodel.Email;
            user.Password = userNodel.Password;
            user.Age = userNodel.Age;
            user.UserRole = userNodel.UserRole;
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return true;
            }
            else
                return false;
        }

        public async Task<User> Read(AuthModel auth){
            User user = await db.Users.FirstOrDefaultAsync(u => u.Name == auth.Name && u.Password == auth.Password);
            return user;
        }
        public async Task<User> Read(int id){
            User user = await db.Users.FirstOrDefaultAsync(u => u.id == id);
            return user;
        }
        public async Task<User> Read(string name){
            User user = await db.Users.FirstOrDefaultAsync(u => u.Name == name);
            return user;
        }

        public async Task<bool> UpdateAll(User user)
        {
            User newuser = await db.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
            if(newuser != null){
            newuser.Age = user.Age;
            if(newuser.Email != user.Email){
            newuser.Email = user.Email;
            newuser.Verified_Email = false;
            }
            db.Users.Update(newuser);
            await db.SaveChangesAsync();
            return true;
            }
            else
            return false;
        }
        public async Task<bool> UpdatePass(User user)
        {
            User newuser = await db.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
            if(newuser != null){
            newuser.Password = user.Password;
            db.Users.Update(newuser);
            await db.SaveChangesAsync();
            return true;
            }
            else
            return false;
        }
        public async Task<bool> Delete(string username){
            User deleteduser = await db.Users.FirstOrDefaultAsync(u => u.Name == username);
            if(deleteduser != null){
                db.Users.Remove(deleteduser);
                await db.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
        public async Task<bool> Delete(int id){
            User deleteduser = await db.Users.FirstOrDefaultAsync(u => u.id == id);
            if(deleteduser != null){
                db.Users.Remove(deleteduser);
                await db.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
        public async Task<bool> AddArticle(string name, Article article)
        {
            User user =  await db.Users.FirstOrDefaultAsync(u => u.Name == name);
            
            user.Articles.Add(article);
            await db.SaveChangesAsync();
            return true;
        }
    }
}