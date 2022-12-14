using Course_project.Controllers;
using Course_project.Data;
using Course_project.Tests.Mocks;
using Course_project.ViewModels;
using Course_project.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course_project.Tests.Tests
{
    public class AccountControllerTests : IDisposable
    {
        private static DbContextOptions<ApplicationDbContext> dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("AccountTest")
            .Options;

        ApplicationDbContext context;

        public AccountControllerTests()
        {
            context = new ApplicationDbContext(dbContextOptions);
            context.Database.EnsureCreated();
            PopulateDb();

        }

        private void PopulateDb()
        {
            var users = FakeData.Users;
            context.Users.AddRange(users);

            var collections = FakeData.Collections;
            context.Collections.AddRange(collections);

            var items = FakeData.Items;
            context.Items.AddRange(items);

            var tags = FakeData.Tags;
            context.Tags.AddRange(tags);

            var tagsConnections = FakeData.TagConnections;
            context.TagConnections.AddRange(tagsConnections);
            context.SaveChanges();
        }

        [Fact]
        public async Task GetUserPage_WithData()
        {
            var controller = new AccountController(null, null,context);

            var response = await controller.UserPage(Models.Enums.CollectionType.Books,
                new Guid("10061240-8b43-47a7-9efe-d2c176cc8bb8").ToString(), "default");

            Assert.NotNull(response);
            Assert.IsType<ViewResult>(response);
            var viewResult = (ViewResult)response;
            var model = viewResult.ViewData.Model;
            Assert.IsType<UserPageViewModel>(model);
            var retVM = (UserPageViewModel)model;
            Assert.Equal(1, retVM.Collections.Count());
        }

        [Fact]
        public async Task GetUserPage_WithoutData()
        {
            var controller = new AccountController(null, null, context);

            var response = await controller.UserPage(Models.Enums.CollectionType.Books, "user without collections", "default");

            Assert.NotNull(response);
            Assert.IsType<ViewResult>(response);
            var viewResult = (ViewResult)response;
            var model = viewResult.ViewData.Model;
            Assert.IsType<UserPageViewModel>(model);
            var retVM = (UserPageViewModel)model;
            Assert.Empty(retVM.Collections);
        }

        [Fact]
        public async Task GetUserPage_NotFound()
        {
            var controller = new AccountController(null, null, context);

            var response = await controller.UserPage(Models.Enums.CollectionType.Books,
                Guid.NewGuid().ToString(), "default");

            Assert.NotNull(response);
            Assert.IsType<NotFoundResult>(response);
        }


        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }

        public void Dispose()
        {
            CleanUp();
        }
    }
}
