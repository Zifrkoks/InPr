using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using InPr.Domain.Services;
using InPr.Web.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
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
    [Route("{Name}/")]
    public async Task<User> Read(string Name){
        User? user = await users.GetUserAsync(Name);
        if(user != null)
        return user;
        else
        return new User{id = 0};
    }
    [HttpGet]
    [Route("{Name}/articles")]
    public async Task<List<ArticleModel>> GetArticles(string Name){
        return await users.GetArticlesAsync(Name);
    }
    [HttpGet]
    [Authorize("admin")]
    [Route("admin/users")]
    public async Task<List<User>> GetUsers(int count, int page){
        return await users.GetAllUsersAsync(count, page);
    }
    [HttpDelete]
    [Authorize("admin")]
    [Route("admin/users/{id}")]
    public async Task<string> DeleteUser(int id){
        return await users.DeleteAsync(id);
    }
    [Route("admin/users/{Name}")]
    public async Task<string> DeleteUser(string Name){
        User user = await users.GetUserAsync(Name);
        if(user != null)
        return await users.DeleteAsync(user.id);
        else
        return "user not found";
    }
    [Route("admin/users/{id}")]
    public async Task<string> GetFullArticle(int id){
        return await users.DeleteAsync(id);
    }

}
