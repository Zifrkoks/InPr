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
    public DbSet<User> Users {get;set;}= null!;
    public DbSet<Role> Roles {get;set;}= null!;
    public DbSet<Article> Articles {get;set;}= null!;
    public NewsDbContext(DbContextOptions<NewsDbContext> options)
    : base(options)
    {
        Database.EnsureCreated();// создаем базу данных при первом обращении
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=newsapp.db");
        optionsBuilder.LogTo(Console.WriteLine);

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "reader";
            string PubRoleName = "publisher";
            Role adminRole = new Role { id = 1, Name = adminRoleName };
            Role userRole = new Role { id = 2, Name = userRoleName };
            Role PublisherRole = new Role { id = 3, Name = PubRoleName };
            User Admin = new User{id = 1, Age = 20, Name = "Zifrkoks",RoleId = adminRole.id, Password = "10122002Z", Email = "qweasdzxcwsxxsw1234@gmail.com", Verified_Email = true, };
            Article article = new Article{id = 1, Title = "lol", Text = "bruh", userId = Admin.id, DateTimeCreated = DateTime.Now, Readers = 0};
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole, PublisherRole });
            modelBuilder.Entity<User>().HasData( new User[] { Admin });
            modelBuilder.Entity<Article>().HasData( new Article[]{article});

            base.OnModelCreating(modelBuilder);

        }
}
}