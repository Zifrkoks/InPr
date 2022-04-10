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
        [HttpPost]
        [Route("/")]
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
        [HttpDelete]
        [Route("/{id}")]
        [Authorize("publisher, admin")]
        public async Task<JsonResult> Delete(int id)
        {
            if(await articles.Delete(id,HttpContext.User.Identity.Name))
                return Json("article Deleted");
            else
                return Json("access denied");
        }
        [HttpPut]
        [Authorize("publisher, admin")]
        [Route("/{id}")]
        public async Task<JsonResult> Update(int id,ArticleModel newarticle){
            if((await articles.GetUser(newarticle)).Name == HttpContext.User.Identity.Name)
            {
                await articles.Update(id,newarticle);
                return Json("article updated");
            }
            else
                return Json("access denied");
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("/{id}")]
        public async Task<JsonResult> Read(int id){
            return Json(await articles.Read(id));
        }
        [Route("/page{number}/{count}")]
        public async Task<JsonResult> ReadPage(int number, int count)
        {
            return Json(articles.ReadList(count,number));
        }


        
    }
