using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InPr.Web.ViewModels;
namespace InPr.Web.Controllers;

    [Controller]
    [Route("/Auth")]
    public class AuthController:Controller
    {
        [Route("/Registration")]
        [HttpPost]
        public StatusCodeResult Registration(UserModel user){
            
            return StatusCode(500);

        }
        [Route("/Login")]
        [HttpPost]
        public StatusCodeResult Login(AuthModel user){
            
            return StatusCode(500);

        }
    }
