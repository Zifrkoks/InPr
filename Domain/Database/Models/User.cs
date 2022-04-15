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
        public int              id              {get;set;}
        public string           Name            {get;set;}

        public string           Email           {get;set;}
        [Column("Verified_Email")]
        public bool             Verified_Email  {get;set;}

        public int              Age             {get;set;}

        public string           Password        {get;set;}
        
        public int              RoleId          {get;set;}

        [ForeignKey("RoleId")]
        public Role             UserRole        {get;set;}

        public List<Article>    Articles        {get;set;}

        
    }
}