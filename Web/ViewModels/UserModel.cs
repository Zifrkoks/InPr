using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InPr.Web.ViewModels
{
    public class UserModel: AuthModel
    {
        public int id{get;set;}
        public string? Email{get;set;}
        public int Age{get;set;}
    }
}