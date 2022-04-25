using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InPr.Web.ViewModels
{
    public class AuthResultModel
    {
        public string? Name{get;set;}
        public string? Role{get;set;}

        public string? Token{get;set;}
        public string? mes{get;set;}  
        }
}