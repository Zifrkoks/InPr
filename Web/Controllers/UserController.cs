using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using InPr.Domain.Services;
using InPr.Web.ViewModels;
using InPr.Domain.Database.Models;

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
    [Route("{Name}/")]
    public async Task<UserModel> Read(string Name){
        return await users.GetUserAsync(Name);
    }
    [HttpGet]
    [Route("{Name}/articles")]
    public async Task<List<ArticleModel>> GetArticles(string Name){
        return await users.GetArticlesAsync(Name);
    }
    [HttpGet]
    [Authorize(Roles = "admin")]
    [Route("admin/users")]
    public async Task<List<User>> GetUsers(int count, int page){
        return await users.GetAllUsersAsync(count, page);
    }
    [HttpDelete]
    [Authorize(Roles = "admin")]
    [Route("admin/users/{id}")]
    public async Task<string> DeleteUser(int id){
        return await users.DeleteAsync(id);
    }

    [HttpDelete]
    [Authorize(Roles = "admin")]
    [Route("admin/users/{Name}")]
    public async Task<string> DeleteUser(string Name){
        UserModel user = await users.GetUserAsync(Name);
        if(user != null)
        return await users.DeleteAsync(user.id);
        else
        return "user not found";
    }
    [HttpGet]
    [Route("admin/users/{id}")]
    [Authorize(Roles = "admin")]
    public async Task<string> GetFullArticle(int id){
        return await users.DeleteAsync(id);
    }
    [HttpGet]
    [Authorize(Roles = "admin")]
    [Route("admin/users/count")]
    public async Task<int> GetCountUsers(){
        return await users.GetCountUsers();
    }
    [HttpGet]
    [Authorize(Roles = "admin")]
    [Route("admin/articles/count")]
    public async Task<int> GetCountArticles(){
        return await users.GetCountArticles();
    }
}
