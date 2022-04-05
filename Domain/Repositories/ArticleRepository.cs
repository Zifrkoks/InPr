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
    public class ArticleRepository
    {
         private NewsDbContext db;
        public ArticleRepository(NewsDbContext context)
        {
            db = context;
        }
        
        public async Task<bool> Create(Article article, User user){
            Article NewArticle = await db.Articles.FirstOrDefaultAsync( ar => ar.Title == article.Title);
            if(NewArticle == null)
            {
                NewArticle = new Article {Title = article.Title, Text = article.Text, Autor = user, DateTimeCreated = DateTime.Now, Readers = 0};
                await db.Articles.AddAsync(NewArticle);
                await db.SaveChangesAsync();
                return true;
            }
            else
            return false;

        }
        public async Task<Article> Read(int Id){
            Article article = await db.Articles.FindAsync(Id);
            return article;
        } 

        public async Task<List<Article>> Read(string Title)
        {
            List<Article> Articles =await Task.Run(()=>db.Articles.AsParallel().Where(p => p.Title == Title).ToList());
            return Articles;
        }

        public async Task<bool> Update(ArticleModel article){
            Article UpdArticle = await db.Articles.FirstOrDefaultAsync(article1 => article1.id == article.id);
            if(UpdArticle != null)
            {
                UpdArticle.Title = article.Title;
                UpdArticle.Text = article.Text;
                UpdArticle.DateTimeCreated = DateTime.Now;
                await Task.Run(() => db.Articles.Update(UpdArticle));
                await db.SaveChangesAsync();
                return true;
            }
            else
            return false;

        }
        public async Task<bool> Delete(int id){
            Article deletedArticle = await db.Articles.FindAsync(id);
            if(deletedArticle != null){
                await Task.Run(() => db.Remove(deletedArticle));
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}