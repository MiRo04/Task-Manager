using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TaskManagerAPI.Controllers;
using TaskManagerAPI.Data;
using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models.DTOs;
using TaskManagerAPI.Models.Entities;


namespace TaskManagerAPITest
{
    [TestClass]
    public class IntegrationTest1
    {
        [TestMethod]
        public async Task LoginTest()
        {
            // Arrange
            var testUser = new Users
            {
                Id = "1",
                Email = "test@example.com",
                UserName = "testuser"
            };

            var loginDto = new LoginUserDto
            {
                Email = testUser.Email,
                Password = "password123"
            };

            var options = new DbContextOptionsBuilder<ApplicationDBContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;

            var dbContext = new ApplicationDBContext(options);

            var userStoreMock = new Mock<IUserStore<Users>>();
            var userManagerMock = new Mock<UserManager<Users>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<Users>>();
            var signInManagerMock = new Mock<SignInManager<Users>>(
                userManagerMock.Object,
                contextAccessorMock.Object,
                userClaimsPrincipalFactoryMock.Object,
                null, null, null, null);

            var tokenServiceMock = new Mock<ITokenService>();

            userManagerMock.Setup(x => x.FindByEmailAsync(loginDto.Email)).ReturnsAsync(testUser);

            signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(testUser, loginDto.Password, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            tokenServiceMock.Setup(x => x.CreateToken(testUser)).Returns("mocked-token");

            var controller = new UserController(
                dbContextMock.Object,
                userManagerMock.Object,
                tokenServiceMock.Object,
                signInManagerMock.Object);

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;

            Assert.IsInstanceOfType(okResult.Value, typeof(NewUserDto));
            var newUser = (NewUserDto)okResult.Value;

            Assert.AreEqual(testUser.Id, newUser.Id);
            Assert.AreEqual(testUser.Email, newUser.Email);
            Assert.AreEqual(testUser.UserName, newUser.UserName);
            Assert.AreEqual("mocked-token", newUser.Token);
        }
    }
}

