using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace InPr.Web.Controllers
{
    [Controller]
    public class HomeController : Controller
    {
    [Route("/")]
    [Route("/index")]
    [Route("/home/index")]
    [HttpGet]
    public string Index(){
        return "working";
    }
    
    }
    
}