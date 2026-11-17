using Application.DTO.Response;
using Application.Services;
using Domain.Enum;
using Domain.Interface;
using Domain.Model;
using NSubstitute;

namespace Tests
{
    public class UserServiceTests
    {

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfGetUserResponseDtos()
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var userService = new UserService(userRepository);

            User user = new User
            {
                Id = 1,
                FullName = "testuser",
                Email = "testuser@example.com",
                PasswordHash = new byte[] { 0x2d, 0x74, 0x15, 0x89, 0x4b, 0x93, 0xe2, 0x11, 0x77, 0x65, 0x48, 0x39, 0x20, 0x14, 0x55, 0x66, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff, 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77 },
                PasswordSalt = new byte[] { 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x00 },
                Role = Role.Entrepreneur,
                LocationId = null,
                Location = null,
                SpecializationId = null,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DeletedAt = null
            };

            User user2 = new User
            {
                Id = 2,
                FullName = "testuser2",
                Email = "testuser2@example.com",
                PasswordHash = new byte[] { 0x74, 0x2d, 0x15, 0x89, 0x4b, 0x93, 0xe2, 0x11, 0x77, 0x65, 0x48, 0x39, 0x20, 0x14, 0x55, 0x66, 0x88, 0x99, 0xaa, 0xbb, 0xcc, 0xdd, 0xee, 0xff, 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77 },
                PasswordSalt = new byte[] { 0xbb, 0xaa, 0xcc, 0xdd, 0xee, 0xff, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0x00 },
                Role = Role.Entrepreneur,
                LocationId = null,
                Location = null,
                SpecializationId = null,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DeletedAt = null
            };

            //  Act
            var Users = new List<User> { user, user2 };

            userRepository.GetAllAsync().Returns(Users);

            var userServiceResult = await userService.GetAllUsersAsync();

            var actualUsers = userServiceResult.ToList();

            //  Assert
            Assert.IsAssignableFrom<IEnumerable<UserResponse>>(userServiceResult);

            Assert.Equal(Users.Count, actualUsers.Count);

            for (int i = 0; i < Users.Count; i++)
            {
                Assert.Equal(Users[i].Id, actualUsers[i].Id);
                Assert.Equal(Users[i].FullName, actualUsers[i].FullName);
                Assert.Equal(Users[i].Email, actualUsers[i].Email);
                Assert.Equal(Users[i].Role, actualUsers[i].Role);
                Assert.Equal(Users[i].CreatedAt, actualUsers[i].CreatedAt);
            }
        }
    }
}
