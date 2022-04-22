using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InPr.Web.ViewModels;
using InPr.Domain.Services;
namespace InPr.Web.Controllers;

    [ApiController]
    public class AuthController:ControllerBase
    {
        UserService users;
        public AuthController(UserService users){
            this.users = users;
        }
        [Route("auth/registration")]
        [HttpPost]
        public async Task<string> Registration([Bind("Name","Password","Age","Email")]UserModel user, string Role){
            
            return await users.RegistationAsync(user, Role);

        }
        [Route("auth/login")]
        [HttpPost]
        public async Task<string> Login([Bind("Name","Password")]AuthModel user){
            
            string JwtBearerToken = await users.LoginAsync(user);
            if(JwtBearerToken != null)
            return JwtBearerToken;
            else
            return "lmoa";

        }
    }
