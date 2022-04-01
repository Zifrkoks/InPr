using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace InPr.Domain.Database.Models
{
    public class News
    {
        [Key]
        [Column("id")]
        int             id              {get;set;}
        
        public string   Title           {get;set;}

        public string   Text            {get;set;}
        [Column("Date_Created")]
        public DateOnly DateCreated     {get;set;}

        public string   Autor           {get;set;}


    }
}