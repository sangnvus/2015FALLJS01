using System.Collections.Generic;
using DDL_CapstoneProject.Models;
using DDL_CapstoneProject.Ultilities;

namespace DDL_CapstoneProject.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DDL_CapstoneProject.Respository.DDLDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DDL_CapstoneProject.Respository.DDLDataContext context)
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

            var listUser = new List<DDL_User>
            {
                new DDL_User
                {
                    Username = "manhmaluc",
                    Password = "9a504921fa47cb96585ac19b3f143af9",// this password is 854821, encrypted by MD5
                    UserType = DDLConstants.UserType.USER,
                    IsActive = true,
                    IsVerify = true,
                    Email = "ngocmanh1712@gmail.com",
                    LoginType = DDLConstants.LoginType.NORMAL,
                    CreatedDate = DateTime.Now,
                    UserInfo = new UserInfo
                    {           
                        Address = "Hung Yen",
                        Country = "Viet Nam",
                        DateOfBirth = new DateTime(1993, 12, 17),
                        FullName = "Lưu Ngọc Mạnh",
                        Gender = DDLConstants.Gender.MALE,
                        ProfileImage = "avatar_manhmaluc.jpg",
                        Biography = "jdsbhjdsjhdvshjsvd\njkdsbdbkdsbdjkbkj\ndsbkjdsbkjbd",
                        FacebookUrl = "http://facebook.com/gatohy",
                        Website = "",
                        //DDL_UserID = context.DDL_Users.Single(x => x.Username == "manhmaluc").DDL_UserID,
                    }
                },
                new DDL_User
                {
                    Username = "admin001",
                    Password = "25d55ad283aa400af464c76d713c07ad",// this password is 12345678, encrypted by MD5
                    UserType = DDLConstants.UserType.ADMIN,
                    IsActive = true,
                    IsVerify = true,
                    Email = "admin001@gmail.com",
                    LoginType = DDLConstants.LoginType.NORMAL,
                    CreatedDate = DateTime.Now,
                    UserInfo = new UserInfo
                    {           
                        Address = "C213, Hoa Lac",
                        Country = "Viet Nam",
                        DateOfBirth = new DateTime(1993, 12, 17),
                        FullName = "Administrator",
                        Gender = DDLConstants.Gender.MALE,
                        ProfileImage = "avatar_admin001.jpg",
                        Biography = "jdsbhjdsjhdvshjsvd\njkdsbdbkdsbdjkbkj\ndsbkjdsbkjbd",
                        FacebookUrl = "http://facebook.com/gatohy",
                        Website = "",
                        //DDL_UserID = context.DDL_Users.Single(x => x.Username == "manhmaluc").DDL_UserID,
                    }
                },
                new DDL_User
                {
                    Username = "test0001",
                    Password = "25d55ad283aa400af464c76d713c07ad",// this password is 12345678, encrypted by MD5
                    UserType = DDLConstants.UserType.USER,
                    IsActive = true,
                    IsVerify = true,
                    Email = "test0001@gmail.com",
                    LoginType = DDLConstants.LoginType.NORMAL,
                    CreatedDate = DateTime.Now,
                    UserInfo = new UserInfo
                    {           
                        Address = "C213, Hoa Lac",
                        Country = "Viet Nam",
                        DateOfBirth = new DateTime(1993, 12, 17),
                        FullName = "Test Accoumt",
                        Gender = DDLConstants.Gender.MALE,
                        ProfileImage = "avatar_test0001.jpg",
                        Biography = "jdsbhjdsjhdvshjsvd\njkdsbdbkdsbdjkbkj\ndsbkjdsbkjbd",
                        FacebookUrl = "http://facebook.com/gatohy",
                        Website = "",
                        //DDL_UserID = context.DDL_Users.Single(x => x.Username == "manhmaluc").DDL_UserID,
                    }
                }
            };

            listUser.ForEach(u => context.DDL_Users.AddOrUpdate(x => x.Username, u));
            context.SaveChanges();

            var listCategory = new List<Category>
            {
                new Category
                {
                    Name = "Âm Nhạc",
                    Description = "abncbnbc cbcn nbcnbc",
                    IsActive = true,
                },
                new Category
                {
                    Name = "Nghệ Thuật",
                    Description = "abncbnbc cbcn nbcnbc",
                    IsActive = true,
                },
                new Category
                {
                    Name = "Truyện Tranh",
                    Description = "abncbnbc cbcn nbcnbc",
                    IsActive = true,
                },
                new Category
                {
                    Name = "Trò Chơi",
                    Description = "abncbnbc cbcn nbcnbc",
                    IsActive = true,
                },
                new Category
                {
                    Name = "Công Nghệ",
                    Description = "abncbnbc cbcn nbcnbc",
                    IsActive = true,
                },
            };

            listCategory.ForEach(u => context.Categories.AddOrUpdate(x => x.Name, u));
            context.SaveChanges();

            var listSlides = new List<Slide>
            {
                new Slide
                {
                    Title = "Emmett Louis Till, 1941–1955",
                    Description = "is murder catalyzed the civil rights movement. Help make the film that will tell his story.",
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    ImageUrl = "/images/slides/slider1.jpg",
                    ButtonText = "View Project",
                    ButtonColor = "btn-success",
                    Order = 1,
                    SlideUrl = "#"
                },
                new Slide
                {
                    Title = "Eco - Global Survival Game",
                    Description = "Collaborate to build civilization in a simulated ecosystem, creating laws to make group decisions.",
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    ImageUrl = "/images/slides/slider3.jpg",
                    ButtonText = "View Project",
                    ButtonColor = "btn-primary",
                    Order = 2,
                    SlideUrl = "#"
                },
                new Slide
                {
                    Title = "The World’s Cleanest Power Plant",
                    Description = "Support a team of architects who are working to reduce carbon emissions by making art in the sky.",
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    ImageUrl = "/images/slides/slider2.jpg",
                    ButtonText = "Download",
                    ButtonColor = "btn-success",
                    Order = 3,
                    SlideUrl = "#"
                },
            };

            listSlides.ForEach(u => context.Slides.AddOrUpdate(x => x.Title, u));
            context.SaveChanges();
        }
    }
}
