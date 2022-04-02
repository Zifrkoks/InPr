using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InPr.Domain.Database.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("id")]
        public string       Name            {get;set;}

        public string       Email           {get;set;}
        [Column("Verified_Email")]
        public bool         Verified_Email  {get;set;}

        public string       Age             {get;set;}

        public string       Password        {get;set;}
        
        public int          Role_id         {get;set;}

        
    }
}