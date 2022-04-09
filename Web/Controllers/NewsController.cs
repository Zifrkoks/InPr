using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using InPr.Domain.Services;
using InPr.Web.ViewModels;
namespace InPr.Web.Controllers;

    [ApiController]
    [Route("/articles")]
    public class NewsController:Controller
    {
        ArticleService articles;
        public NewsController(ArticleService articles){
            this.articles = articles;

        }
        
        [Authorize(Roles = "publisher, admin")]
        public async Task<JsonResult> Create(ArticleModel article){
            if(ModelState.IsValid){
            if(await articles.Create(article,HttpContext.User.Identity.Name))
                return Json("article Created");
                else
                return Json("article error");
            }
            else
            return Json("model invalid");
        }
        
    }
