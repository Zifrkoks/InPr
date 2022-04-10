using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;
using InPr.Domain.Repositories;
namespace InPr.Domain.Services
{
    public class ArticleService
    {
        ArticleRepository articles;
        UserRepository users;
        public ArticleService(ArticleRepository articles, UserRepository users){
            this.articles = articles;
            this.users = users;
        }
        public async Task<bool> Create(ArticleModel articlemodel, string Name){
            User user = await users.Read(Name);
            Article article = new Article{
                Title = articlemodel.Title, 
                Text = articlemodel.Text, 
                DateTimeCreated = DateTime.Now, 
                Readers = 0, 
                Author = user
            };
            int id = await articles.Create(article);
            users.AddArticle(Name,await articles.Read(id));
            return true;

        }
        public async Task<Article> Read(int id){
            Article article = await articles.Read(id);
            return article;
        }
        public async Task<List<Article>> ReadList(string name){
            List<Article> article = await articles.Read(name);
            return article;
        }
         public async Task<List<Article>> ReadList(int amount,int PageNum){
            return await articles.ReadList(amount, PageNum);
        }
        public async  Task<bool> Update(int id, ArticleModel articleModel){
            articleModel.id = id;
            return await articles.Update(articleModel);
        }
        public async Task<bool> Delete(int id,string name){
            Article article = await articles.Read(id);
            if((article.Author.Name == name) || (await users.ReadRole(name)).Name == "admin")
            return await articles.Delete(id);
            else
            return false;
        }
        public async Task<User> GetUser(ArticleModel model){
            return (await articles.Read(model.id)).Author;
        }
    }
}