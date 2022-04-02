using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InPr.Web.ViewModels;
namespace InPr.Web.Controllers;

    [Controller]
    public class AuthController:Controller
    {
        [HttpPost]
        public StatusCodeResult Registration(UserModel user){
            
            return StatusCode(500);

        }
        [HttpPost]
        public StatusCodeResult Login(AuthModel user){
            
            return StatusCode(500);

        }
    }
