using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace InPr.Domain.Database.Models
{
    [Table("Articles")]
    public class Article
    {
        [Key]
        [Column("id")]
        public int      id              {get;set;}
        [Required]
        public string?  Title           {get;set;}

        public string?  Text            {get;set;}
        
        public int      userId          {get;set;}
        public User?    user            {get;set;}
        [Required]
        [Column("Date_Created")]
        public DateTime DateTimeCreated {get;set;}

        public int      Readers         {get;set;}


    }
}