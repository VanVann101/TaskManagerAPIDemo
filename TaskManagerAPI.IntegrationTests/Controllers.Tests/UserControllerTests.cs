using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Http.Json;
using TaskManager.Application.Common.Constants;
using TaskManager.Application.Common.DTO;

namespace TaskManagerAPI.IntegrationTests.Controllers.Tests {
    public class UserControllerTests : IClassFixture<TaskManagerWebApplicationFactory> {
        private readonly TaskManagerWebApplicationFactory _factory;

        public UserControllerTests(TaskManagerWebApplicationFactory factory) {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnList() {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync("/api/users");

            //Assert
            response.EnsureSuccessStatusCode();
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            Assert.NotNull(users);
            Assert.NotEmpty(users);
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task GetUserById_NonExistId_ShouldReturnListUsers() {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var response = await client.GetAsync("/api/users/10461ac3-c991-4a83-84a2-172648cb11a0");

            //Assert
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.Equal(Guid.Empty, user.Id);
        }

        [Fact]
        public async Task Register_CorrectModel_ShouldCreateUser() {
            //Arrange
            var client = _factory.CreateClient();

            var newUser = new CreateUserDto() {
                FirstName = "Jason",
                LastName = "Mabon",
                Email = "Jasonmabon@example.com",
                Password = "Password123!",
                Phone = "8901456791"
            };

            var content = JsonContent.Create(newUser);

            //Act
            var response = await client.PostAsync("/api/users/register", content);

            //Assert
            response.EnsureSuccessStatusCode();
            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.NotNull(user);
            Assert.Equal(user.FirstName, newUser.FirstName);
            Assert.Equal(user.LastName, newUser.LastName);
            Assert.Equal(user.Email, newUser.Email);
            Assert.Equal(user.Phone, newUser.Phone);
        }

        [Fact]
        public async Task Register_InvalidEmail_ShouldReturnError() {
            //Arrange
            var client = _factory.CreateClient();

            var newUser = new CreateUserDto() {
                FirstName = "Jason",
                LastName = "Mabon",
                Email = "noemailstring",
                Password = "Password123!",
                Phone = "8901456791"
            };

            var content = JsonContent.Create(newUser);

            //Act
            var response = await client.PostAsync("/api/users/register", content);

            //Assert
            var error = await response.Content.ReadAsStringAsync();
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(ErrorMessages.EmailInvalid, error);
        }
    }
}