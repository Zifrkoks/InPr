using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InPr.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

using InPr.Domain.Services;
namespace InPr.Web.Controllers;

    [ApiController]
    public class AuthController:ControllerBase
    {
        UserService users;
        public AuthController(UserService users){
            this.users = users;
        }
        [AllowAnonymous]
        [Route("auth/registration")]
        [HttpPost]
        public async Task<AuthResultModel> Registration([Bind("Name","Password","Age","Email","role")]UserModel user){
            
            return await users.RegistationAsync(user);

        }
        [AllowAnonymous]
        [Route("auth/login")]
        [HttpPost]
        public async Task<AuthResultModel> Login([Bind("Name","Password")]AuthModel user){
            
            return await users.LoginAsync(user);
        }
    }
