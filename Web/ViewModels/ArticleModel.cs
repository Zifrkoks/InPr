using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InPr.Web.ViewModels
{
    public class ArticleModel
    {
        public int      id      {get;set;}
        public string?  Title   {get;set;}
        public string?  Text    {get;set;}
        public string?  Date    {get;set;}
        public string?  Username{get;set;}

    }
}