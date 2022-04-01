using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InPr.Domain.Database.Models;

namespace InPr.Domain.Database
{
    public class AppDBContext: DbContext
    {
    public DbSet<User> Users => Set<User>();
    public AppDBContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
    }
}
}