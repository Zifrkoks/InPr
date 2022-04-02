using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InPr.Web.ViewModels
{
    public class UserModel
    {
        [Required]
        [Range(3, 32, ErrorMessage = "недопустимый никнейм")]
        public string Name {get;set;}
        [Required]
        public string Email{get;set;}
        [Range(14, 110, ErrorMessage = "Недопустимый возраст")]
        public string Age{get;set;}
        [Required]
        [Range(8, 32, ErrorMessage = "Недопустимый пароль")]
        public string Password{get;set;}      
    }
}