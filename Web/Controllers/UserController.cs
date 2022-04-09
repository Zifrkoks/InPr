using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using InPr.Domain.Services;
using InPr.Web.ViewModels;
namespace InPr.Web.Controllers;

[ApiController]
public class UserController : Controller
{
    public UserService users;

    [HttpGet]
    [Authorize]
    [Route("/{Name}")]
    public async Task<JsonResult> Read(string Name){
        return Json(await users.GetUser(Name));
    }
    [Authorize]
    [HttpPost]
    [Route("/{name}/update/pass")]
    public async Task<JsonResult> Updatepass(string Name, AuthModel user){
        if(HttpContext.User.FindFirst("Name").Value == Name){
            if(ModelState.IsValid)
                await users.UpdatePass(user);
            return Json("success");
        }
        return Json("auth error");
    }
    [HttpGet]
    [Route("/{name}/articles")]
    public async Task<JsonResult> GetArticles(string name){
        return Json(await users.GetArticles(name));
    }
    
}
