using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using InPr.Domain.Services;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace InPr.Web.Controllers;

    [ApiController]
    public class NewsController:ControllerBase
    {
        ArticleService articles;
        public NewsController(ArticleService articles){
            this.articles = articles;

        }
        [Authorize(Roles = "publisher,admin")]
        [HttpPost]
        [Route("articles")]
        public async Task<string> Create([Bind("Title","Text")]ArticleModel article){
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
        [Authorize(Roles = "publisher,admin")]
        [HttpDelete]
        [Route("articles/{id}")]
        public async Task<string> Delete(int id)
        {
        if(HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null){
            return await articles.DeleteAsync(id, HttpContext.User.Identity.Name);
            }
            else
            return "you ar not logged in";
        }
        [HttpPut]
        [Authorize(Roles = "publisher,admin")]
        [Route("articles/{id}")]
        public async Task<string> Update(int id,[Bind("Title","Text"), FromBody]ArticleModel newarticle){
            if(HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.Name != null){
                User? user = await articles.GetUserAsync(id);
                if(user != null){
                    if(user.Name == HttpContext.User.Identity.Name)
                    {
                        await articles.UpdateAsync(id,newarticle);
                        return "article updated";
                    }
                    else
                        return "access denied, Article created:" + user.Name + ", you not he";
                }
                else 
                return "error in find user";
            }
            else
            return "error";
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("articles/{id}")]
        public async Task<ArticleModel?> Read(int id){
            return await articles.ReadAsync(id);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("articles/search/{name}")]
        public async Task<List<ArticleModel>> ReadByName(string name){
            return await articles.ReadListAsync(name);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("articles/page{number}/{count}")]
        public async Task<List<ArticleModel>> ReadPage(int number, int count)
        {
            return await articles.ReadListAsync(count,number);
        }


        
    }
