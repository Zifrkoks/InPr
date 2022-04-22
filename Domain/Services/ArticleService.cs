using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;
using InPr.Domain.Database;
using Microsoft.EntityFrameworkCore;

namespace InPr.Domain.Services
{
    public class ArticleService
    {
        NewsDbContext db;
        public ArticleService( NewsDbContext db){
            this.db = db;
        }
        public async Task<bool> CreateAsync(ArticleModel articlemodel, string? Name){
            User? user = await db.Users.FirstOrDefaultAsync(u => u.Name == Name);
            if(user != null && Name != null){
                Article article = new Article{
                    Title = articlemodel.Title, 
                    Text = articlemodel.Text, 
                    DateTimeCreated = DateTime.Now, 
                    Readers = 0, 
                    user = user
                };
                    user.Articles.Add(article);
                    db.Users.Update(user);
                    await db.Articles.AddAsync(article);
                    await db.SaveChangesAsync();
                return true;
            }
            return false;

        }
        public async Task<Article?> ReadAsync(int id){
            Article? article = await db.Articles.FirstOrDefaultAsync(u => u.id == id);
            if(article != null)
            return article;
            else 
            return null;
        }
        public async Task<List<Article>> ReadListAsync(string name){
            List<Article> Articles = await Task.Run(()=>db.Articles.AsParallel().Where(p => p.Title == name).ToList());
            return Articles;
        }
         public async Task<List<Article>> ReadListAsync(int amount,int PageNum){
            return await Task.Run(()=>db.Articles.AsParallel().Skip(amount*PageNum).Take(amount).ToList());
        }
        public async  Task<bool> UpdateAsync(int id, ArticleModel articleModel){
            Article? UpdArticle = await db.Articles.FirstOrDefaultAsync(article1 => article1.id == id);
            if(UpdArticle != null)
            {
                UpdArticle.Title = articleModel.Title;
                UpdArticle.Text = articleModel.Text;
                UpdArticle.DateTimeCreated = DateTime.Now;
                await Task.Run(() => db.Articles.Update(UpdArticle));
                await db.SaveChangesAsync();
                return true;
            }
            else
            return false;
        }
        public async Task<string> DeleteAsync(int id,string name){
            Article? article = await db.Articles.FirstOrDefaultAsync((r)=> r.id == id);
            if(article == null)
                return "article not found";
            db.Articles.Remove(article);
            await db.SaveChangesAsync();
            return "article deleted";
        }
        
        public async Task<User?> GetUserAsync(int id){
            Article? article = await db.Articles.Include(u=>u.user).FirstOrDefaultAsync(a => a.id == id);
            if(article != null && article.user != null)
            return article.user;
            else
            return null;
        }
        
    }
}