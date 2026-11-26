using Domain.Enum;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Runtime.InteropServices;
using static System.Net.WebRequestMethods;

namespace Tests.Test
{
    public class UserRepositoryTests
    {
        private ConnectionDbContext context = Substitute.For<ConnectionDbContext>(new DbContextOptions<ConnectionDbContext>());

        private DbSet<User> dbSet = Substitute.For<DbSet<User>, IAsyncEnumerable<User>>();

        [Fact]
        public async Task GetAllAsync_ReturnsListOfUsers()
        {
            //  Arrange
            User user = new User
            {
                Id = 1,
                FullName = "testuser",
                Email = "testuser@example.com",
                PasswordHash = new byte[] { 0x2d },
                PasswordSalt = new byte[] { 0xaa },
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
                PasswordHash = new byte[] { 0x74 },
                PasswordSalt = new byte[] { 0xbb },
                Role = Role.Entrepreneur,
                LocationId = null,
                Location = null,
                SpecializationId = null,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                DeletedAt = null
            };

            var users = new List<User> { user, user2 };

            IAsyncEnumerable<User> asyncUsers = users.ToAsyncEnumerable();

            IAsyncEnumerator<User> userEnumerator = asyncUsers.GetAsyncEnumerator();

            ((IAsyncEnumerable<User>)dbSet).GetAsyncEnumerator().Returns(userEnumerator);

            context.Set<User>().Returns(dbSet);

            var userRepository = new UserRepository(context);

            //  Act
            var userRepositoryResult = await userRepository.GetAllAsync();

            var actualUsers = userRepositoryResult.ToList();

            //  Assert
            Assert.NotNull(userRepositoryResult);

            Assert.IsAssignableFrom<IEnumerable<User>>(userRepositoryResult);

            Assert.Equal(users.Count, actualUsers.Count);

            for (int i = 0; i < users.Count; i++)
            {
                Assert.Equal(users[i].Id, actualUsers[i].Id);
                Assert.Equal(users[i].FullName, actualUsers[i].FullName);
                Assert.Equal(users[i].Email, actualUsers[i].Email);
                Assert.Equal(users[i].PasswordHash, actualUsers[i].PasswordHash);
                Assert.Equal(users[i].PasswordSalt, actualUsers[i].PasswordSalt);
                Assert.Equal(users[i].Role, actualUsers[i].Role);
                Assert.Equal(users[i].CreatedAt, actualUsers[i].CreatedAt);
            }
        }

        [Fact]
        public async Task GetAllAsync_WhenNoUsersExist_ReturnsEmptyList()
        {
            //  Arrange
            context.Set<User>().Returns(dbSet);

            var userRepository = new UserRepository(context);

            //  Act
            var userRepositoryResult = await userRepository.GetAllAsync();

            var actualUsers = userRepositoryResult.ToList();

            //  Assert

            Assert.NotNull(userRepositoryResult);
            Assert.IsAssignableFrom<IEnumerable<User>>(userRepositoryResult);
            Assert.Empty(actualUsers);
        }

        [Fact]
        public async Task GetAllAsync_WhenDatabaseThrowsException_PropagatesException()
        {
            //  Arrange
            ((IAsyncEnumerable<User>)dbSet).Throws(new Exception("Database error"));

            context.Set<User>().Returns(dbSet);

            var userRepository = new UserRepository(context);

            //  Act
            var exception = await Assert.ThrowsAsync<Exception>(() => userRepository.GetAllAsync());

            //  Assert
            Assert.Equal("Database error", exception.Message);
        }
    }
}
