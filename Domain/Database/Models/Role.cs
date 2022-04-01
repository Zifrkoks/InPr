using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace InPr.Domain.Database.Models
{
    public class Role
    {
        [Key]
        [Column("id")]
        public int      id      {get;set;}
        public string   Name    {get;set;}
    }
}