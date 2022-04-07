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
    public NewsDbContext(DbContextOptions<NewsDbContext> options)
    : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении
    }
    
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb;Trusted_Connection=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "reader";
            string PubRoleName = "publisher";
 
            // добавляем роли
            Role adminRole = new Role { id = 1, Name = adminRoleName };
            Role userRole = new Role { id = 2, Name = userRoleName };
            Role PublisherRole = new Role { id = 3, Name = PubRoleName };

            User Admin = new User{id = 1, Name = "Zifrkoks",RoleId = adminRole.id, Password = "10122002Z"};
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData( new User[] { Admin });
            base.OnModelCreating(modelBuilder);
        }
}
}