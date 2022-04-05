using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InPr.Domain.Database.Models;

namespace InPr.Domain.Database
{
    public class NewsDbContext: DbContext
    {
    
    public DbSet<Article> Articles = null!;
    public DbSet<User> Users => null!;
    public DbSet<Role> Roles => null!;

    public NewsDbContext()
    {
        Database.EnsureCreatedAsync();
    }
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
    }
}
}