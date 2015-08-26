namespace DIO_FALL15.Migrations
{
    using DIO_FALL15.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DIO_FALL15.Respository.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DIO_FALL15.Respository.DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var listUser = new List<User>{
                new User
                {
                    Username = "manhmaluc",
                    Password = "9a504921fa47cb96585ac19b3f143af9",// this password is 854821, encrypted by MD5
                    FirstName = "Ngoc Manh",
                    LastName = "Luu",
                    Email = "ngocmanh1712@gmail.com",
                    PhoneNumber = "0973232734",
                    Genrer = Genrer.Male,
                    Address = "Hung Yen"
                },
                new User
                {
                    Username = "chinhtinhnhanh",
                    Password = "25d55ad283aa400af464c76d713c07ad",// this password is 12345678, encrypted by MD5
                    FirstName = "Cong Chinh",
                    LastName = "Vu",
                    Email = "abcxyz@gmail.com",
                    PhoneNumber = "090987654",
                    Genrer = Genrer.Male,
                    Address = "Bac Ninh"
                }
            };

            listUser.ForEach(x => context.Users.AddOrUpdate(y => y.Username, x));
            context.SaveChanges();

            var listCategories = new List<Category>{
                new Category
                {
                    Name = "Art",
                    Status = Status.Active,
                    Description = "There is a description of Art category."
                },
                new Category
                {
                    Name = "Technology",
                    Status = Status.Active,
                    Description = "There is a description of Technology category."
                },
                new Category
                {
                    Name = "Comics",
                    Status = Status.Active,
                    Description = "There is a description of Comics category."
                },
                new Category
                {
                    Name = "Games",
                    Status = Status.Active,
                    Description = "There is a description of Games category."
                },
            };

            listCategories.ForEach(x => context.Categories.AddOrUpdate(y => y.Name, x));
            context.SaveChanges();

            var listProjects = new List<Project>{
                new Project
                {
                    UserId = context.Users.Single(x => x.Username == "chinhtinhnhanh").Id,
                    Title = "Dream of Inovation",
                    CreatedDate = DateTime.Now,
                    TargetFund = 10000,
                    Deadline = DateTime.Now.AddDays(30),
                    CategoryId = context.Categories.Single(x => x.Name == "Technology").Id,
                    Status = Status.Active,
                    CurrentFund = 0,
                    Description = "There is a description of Dream of Inovation project",
                    ImageLink = "Image1.jpg"

                },
                new Project
                {
                    UserId = context.Users.Single(x => x.Username == "manhmaluc").Id,
                    Title = "Dream of Inovation 2",
                    CreatedDate = DateTime.Now,
                    TargetFund = 300000,
                    Deadline = DateTime.Now.AddDays(15),
                    CategoryId = context.Categories.Single(x => x.Name == "Art").Id,
                    Status = Status.Active,
                    CurrentFund = 0,
                    Description = "There is a description of Dream of Inovation 2 project",
                    ImageLink = "Image2.jpg"
                },
            };
            listProjects.ForEach(x => context.Projects.AddOrUpdate(y => y.Title, x));
            context.SaveChanges();
        }
    }
}
