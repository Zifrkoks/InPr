using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using InPr.Domain.Services;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;
namespace InPr.Web.Controllers;

[ApiController]
public class UserController : Controller
{
    public UserService users;

    public UserController(UserService users){
        this.users = users;
    }
    [HttpGet]
    [Authorize]
    [Route("{Name}:string/")]
    public async Task<User> Read(string Name){
        User? user = await users.GetUserAsync(Name);
        if(user != null)
        return user;
        else
        return new User{id = 0};
    }
    [HttpGet]
    [Route("{Name}:string/articles")]
    public async Task<List<Article>> GetArticles(string Name){
        return await users.GetArticlesAsync(Name);
    }
    
}
