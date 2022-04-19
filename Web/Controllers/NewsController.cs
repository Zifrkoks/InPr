using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using InPr.Domain.Services;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;

namespace InPr.Web.Controllers;

    [ApiController]
    public class NewsController:ControllerBase
    {
        ArticleService articles;
        public NewsController(ArticleService articles){
            this.articles = articles;

        }
        [HttpPost]
        [Route("articles")]
        [Authorize(Roles = "publisher, admin")]
        public async Task<string> Create(ArticleModel article){
            if(HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null){
            if(ModelState.IsValid){
            if(await articles.CreateAsync(article,HttpContext.User.Identity.Name))
                return "article Created";
                else
                return "article error";
            }
            else
            return "model invalid";
            }
            else
            return "you ar not logged in";
        }
        [HttpDelete]
        [Route("articles/{id}")]
        [Authorize("publisher, admin")]
        public async Task<string> Delete(int id)
        {
        if(HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null){
            if(await articles.DeleteAsync(id,HttpContext.User.Identity.Name))
                return "article Deleted";
            else
                return "access denied";
            }
            else
            return "you ar not logged in";
        }
        [HttpPut]
        [Authorize("publisher, admin")]
        [Route("articles/{id}")]
        public async Task<string> Update(int id,ArticleModel newarticle){
            if(HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null){
            User? user = await articles.GetUserAsync(newarticle);
            if(user != null && user.Name == HttpContext.User.Identity.Name)
            {
                await articles.UpdateAsync(id,newarticle);
                return "article updated";
            }
            else
                return "access denied";
            }
            else
            return "you ar not logged in"; 
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("articles/{id}")]
        public async Task<Article> Read(int id){
            Article? article = await articles.ReadAsync(id);
            if(article != null)
            return article;
            else
            return new Article{ id = 0};
        }
        [Route("/page{number}/{count}")]
        public async Task<List<Article>> ReadPage(int number, int count)
        {
            return await articles.ReadListAsync(count,number);
        }


        
    }
