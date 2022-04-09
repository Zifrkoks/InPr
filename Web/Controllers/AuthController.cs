using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InPr.Web.ViewModels;
using InPr.Domain.Services;
namespace InPr.Web.Controllers;

    [Controller]
    [Route("/Auth")]
    public class AuthController:Controller
    {
        UserService users;
        public AuthController(UserService users){
            this.users = users;
        }
        [Route("/Registration")]
        [HttpPost]
        public async Task<JsonResult> Registration(UserModel user, string Role){
            
            return Json(await users.Registation(user, Role));

        }
        [Route("/Login")]
        [HttpPost]
        public async Task<JsonResult> Login(AuthModel user){
            
            return Json(await users.Login(user));

        }
    }
