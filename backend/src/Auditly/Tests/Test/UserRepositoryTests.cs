using Application.DTO.Response;
using Application.Exceptions;
using Domain.Enum;
using Domain.Model;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
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

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsUser()
        {
            //  Arrange
            int userId = 1;

            User user = new User
            {
                Id = userId,
                FullName = "testuser",
                Email = "testuser@example.com",
                PasswordHash = new byte[] { 0x2d },
                PasswordSalt = new byte[] { 0xaa },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            dbSet.FindAsync(userId).Returns(user);

            context.Set<User>().Returns(dbSet);

            var userRepository = new UserRepository(context);

            //  Act
            var userRepositoryResult = await userRepository.GetByIdAsync(userId);

            //  Assert
            await dbSet.Received(1).FindAsync(userId);

            Assert.NotNull(userRepositoryResult);

            Assert.IsType<User>(userRepositoryResult);

            Assert.Equal(user.Id, userRepositoryResult.Id);
            Assert.Equal(user.FullName, userRepositoryResult.FullName);
            Assert.Equal(user.Email, userRepositoryResult.Email);
            Assert.Equal(user.PasswordHash, userRepositoryResult.PasswordHash);
            Assert.Equal(user.PasswordSalt, userRepositoryResult.PasswordSalt);
            Assert.Equal(user.Role, userRepositoryResult.Role);
            Assert.Equal(user.CreatedAt, userRepositoryResult.CreatedAt);
        }

        [Fact]
        public async Task GetByIdAsync_WhenUserNotFound_ThrowsNotFound()
        {
            //  Arrange
            int userId = 99;

            dbSet.FindAsync(userId).Returns((User?)null);

            context.Set<User>().Returns(dbSet);

            var userRepository = new UserRepository(context);

            //  Act
            var exception = await Assert.ThrowsAsync<ErrorResponse>(() => userRepository.GetByIdAsync(userId));

            var expected = Assert.Throws<ErrorResponse>(() => GlobalExceptionHandler.ThrowNotFound());

            //  Assert
            await dbSet.Received(1).FindAsync(userId);

            Assert.Equal(expected.StatusCode, exception.StatusCode);
            Assert.Equal(expected.Message, exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public async Task GetByIdAsync_WithIdLessThanOrEqualToZero_ThrowsInvalidArgument(int userId)
        {
            //  Arrange
            context.Set<User>().Returns(dbSet);

            var userRepository = new UserRepository(context);

            //  Act
            var exception = await Assert.ThrowsAsync<ErrorResponse>(() => userRepository.GetByIdAsync(userId));

            var expected = Assert.Throws<ErrorResponse>(() => GlobalExceptionHandler.ThrowInvalidArgument());

            //  Assert
            await dbSet.DidNotReceive().FindAsync(Arg.Any<object[]>());

            Assert.Equal(expected.StatusCode, exception.StatusCode);
            Assert.Equal(expected.Message, exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_WhenDatabaseThrowsException_PropagatesException()
        {
            //  Arrange
            int userId = 1;

            dbSet.FindAsync(userId).Throws(new Exception("Database error"));

            context.Set<User>().Returns(dbSet);

            var userRepository = new UserRepository(context);

            //  Act
            var exception = await Assert.ThrowsAsync<Exception>(() => userRepository.GetByIdAsync(userId));

            //  Assert
            await dbSet.Received(1).FindAsync(userId);

            Assert.Equal("Database error", exception.Message);
        }
    }
}
