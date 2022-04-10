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
        [Required]
        public string Email{get;set;}
        [Range(14, 110, ErrorMessage = "Недопустимый возраст")]
        public string Age{get;set;}
    }
}