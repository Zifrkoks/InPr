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
        
        public string   Title           {get;set;}

        public string   Text            {get;set;}
        [Column("Date_Created")]


        public string   AutorId         {get;set;}
        [ForeignKey("AutorId")]
        public User     Autor           {get;set;}
        [Required]
        public DateTime DateTimeCreated {get;set;}

        public int      Readers         {get;set;}


    }
}