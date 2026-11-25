using Application.DTO.Request;
using Application.DTO.Response;
using Application.Exceptions;
using Application.Services;
using Domain.Enum;
using Domain.Interface;
using Domain.Model;
using Infrastructure.Security;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;

namespace Tests
{
    public class UserServiceTests
    {

        [Fact]
        public async Task GetAllUsersAsync_ReturnsListOfUserResponse()
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

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

            //  Act
            var Users = new List<User> { user, user2 };

            userRepository.GetAllAsync().Returns(Users);

            var userServiceResult = await userService.GetAllUsersAsync();

            var actualUsers = userServiceResult.ToList();

            //  Assert
            await userRepository.GetAllAsync().Received(1);

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

        [Fact]
        public async Task GetAllUsersAsync_WhenNoUsersExist_ReturnsEmptyList()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            userRepository.GetAllAsync().Returns(new List<User>());

            // Act
            var userServiceResult = await userService.GetAllUsersAsync();
            var actualUsers = userServiceResult.ToList();

            // Assert
            await userRepository.GetAllAsync().Received(1);
            Assert.NotNull(userServiceResult);
            Assert.IsAssignableFrom<IEnumerable<UserResponse>>(userServiceResult);
            Assert.Empty(actualUsers);
        }

        [Fact]
        public async Task GetUserByIdAsync_WithValidId_ReturnsUserResponse()
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

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

            int userId = 1;

            //  Act
            userRepository.GetByIdAsync(1).Returns(user);

            var userServiceResult = await userService.GetUserByIdAsync(userId);

            //  Assert
            Assert.NotNull(userServiceResult);

            Assert.IsType<UserResponse>(userServiceResult);

            Assert.Equal(user.Id, userServiceResult.Id);
            Assert.Equal(user.FullName, userServiceResult.FullName);
            Assert.Equal(user.Email, userServiceResult.Email);
            Assert.Equal(user.Role, userServiceResult.Role);
            Assert.Equal(user.CreatedAt, userServiceResult.CreatedAt);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserNotFound_ReturnsNull()
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 99;
    
            //  Act
            userRepository.GetByIdAsync(userId).Returns((User?)null);

            var userServiceResult = await userService.GetUserByIdAsync(userId);

            //  Assert
            Assert.Null(userServiceResult);
        }

        [Fact]
        public async Task CreateUserAsync_WithValidCreateUserRequest_ReturnsUserResponse()
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            var requestDto = new CreateUserRequest
            {
                FullName = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!"
            };

            var expectedHash = new byte[] { 0x2d, 0x74, 0x15, 0x89 };
            var expectedSalt = new byte[] { 0xaa, 0xbb, 0xcc, 0xdd };


            var createdUser = new User
            {
                Id = 1,
                FullName = requestDto.FullName,
                Email = requestDto.Email,
                PasswordHash = expectedHash,
                PasswordSalt = expectedSalt,
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            //  Act
            passwordHasher.Hash(requestDto.Password).Returns((expectedHash, expectedSalt));

            userRepository.AddAsync(Arg.Any<User>()).Returns(createdUser);

            var result = await userService.CreateUserAsync(requestDto);

            //  Assert
            await userRepository.Received(1).GetByEmailAsync(requestDto.Email);

            passwordHasher.Received(1).Hash(requestDto.Password);

            await userRepository.Received(1).AddAsync(Arg.Any<User>());

            Assert.NotNull(result);

            Assert.IsType<UserResponse>(result);

            Assert.Equal(createdUser.Id, result.Id);
            Assert.Equal(createdUser.FullName, result.FullName);
            Assert.Equal(createdUser.Email, result.Email);
            Assert.Equal(createdUser.Role, result.Role);
            Assert.Equal(createdUser.CreatedAt, result.CreatedAt);
        }

        public static TheoryData<CreateUserRequest> InvalidCreateUserRequests = new TheoryData<CreateUserRequest>
        {
            new CreateUserRequest { FullName = null, Email = "testuser@example.com", Password = "Password123!" },
            new CreateUserRequest { FullName = "", Email = "testuser@example.com", Password = "Password123!" },
            new CreateUserRequest { FullName = " ", Email = "testuser@example.com", Password = "Password123!" },
            new CreateUserRequest { FullName = "testuser", Email = null, Password = "Password123!" },
            new CreateUserRequest { FullName = "testuser", Email = "", Password = "Password123!" },
            new CreateUserRequest { FullName = "testuser", Email = "\t", Password = "Password123!" },
            new CreateUserRequest { FullName = "testuser", Email = "testuser@example.com", Password = null },
            new CreateUserRequest { FullName = "testuser", Email = "testuser@example.com", Password = "" },
            new CreateUserRequest { FullName = "testuser", Email = "testuser@example.com", Password = "\n" }
        };

        [Theory]
        [MemberData(nameof(InvalidCreateUserRequests))]
        public async Task CreateUserAsync_WithMissingOrWhitespaceFields_ThrowsBadRequest(CreateUserRequest requestDto)
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            //  Act
            var exception = await Assert.ThrowsAsync<ErrorResponse>(() => userService.CreateUserAsync(requestDto));

            var expected = Assert.Throws<ErrorResponse>(() => GlobalExceptionHandler.ThrowInvalidArgument());

            //  Assert
            passwordHasher.DidNotReceive().Hash(Arg.Any<string>());

            await userRepository.DidNotReceive().GetByEmailAsync(Arg.Any<string>());

            await userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
            
            Assert.Equal(expected.StatusCode, exception.StatusCode);
            Assert.Equal(expected.Message, exception.Message);
        }

        [Fact]
        public async Task CreateUserAsync_WhenEmailAlreadyExists_ThrowsConflict()
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            var requestDto = new CreateUserRequest
            {
                FullName = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!"
            };

            var existingUser = new User
            {
                Id = 1,
                FullName = "otheruser",
                Email = requestDto.Email,
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            userRepository.GetByEmailAsync(requestDto.Email).Returns(existingUser);

            //  Act
            var exception = await Assert.ThrowsAsync<ErrorResponse>(() => userService.CreateUserAsync(requestDto));

            var expected = Assert.Throws<ErrorResponse>(() => GlobalExceptionHandler.ThrowEmailAlreadyExists());

            //  Assert
            await userRepository.Received(1).GetByEmailAsync(requestDto.Email);

            passwordHasher.DidNotReceive().Hash(requestDto.Password);

            await userRepository.DidNotReceive().AddAsync(Arg.Any<User>());
            
            Assert.Equal(expected.StatusCode, exception.StatusCode);
            Assert.Equal(expected.Message, exception.Message);
        }

        [Fact]
        public async Task CreateUserAsync_WhenOnlyFullNameMatchesExistingUser_ReturnsUserResponse()
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            var requestDto = new CreateUserRequest
            {
                FullName = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!"
            };

            var existingUser = new User
            {
                Id = 1,
                FullName = requestDto.FullName,
                Email = "exists@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var createdUser = new User
            {
                Id = 2,
                FullName = requestDto.FullName,
                Email = requestDto.Email,
                PasswordHash = new byte[] { 0x03 },
                PasswordSalt = new byte[] { 0x04 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            passwordHasher.Hash(requestDto.Password).Returns((createdUser.PasswordHash, createdUser.PasswordSalt));

            userRepository.GetByEmailAsync(requestDto.Email).Returns(existingUser);

            userRepository.AddAsync(Arg.Any<User>()).Returns(createdUser);

            //  Act
            var result = await userService.CreateUserAsync(requestDto);

            //  Assert
            await userRepository.Received(1).GetByEmailAsync(requestDto.Email);

            passwordHasher.Received(1).Hash(requestDto.Password);

            await userRepository.Received(1).AddAsync(Arg.Any<User>());

            Assert.NotNull(result);

            Assert.IsType<UserResponse>(result);

            Assert.Equal(createdUser.Id, result.Id);
            Assert.Equal(createdUser.FullName, result.FullName);
            Assert.Equal(createdUser.Email, result.Email);
            Assert.Equal(createdUser.Role, result.Role);
            Assert.Equal(createdUser.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async Task CreateUserAsync_WhenPasswordAlreadyExists_ReturnsUserResponse()
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            var requestDto = new CreateUserRequest
            {
                FullName = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!"
            };

            var existingUser = new User
            {
                Id = 1,
                FullName = "otheruser",
                Email = "other@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0xaa },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var newHash = new byte[] { 0x02 };
            var newSalt = new byte[] { 0xcc };

            passwordHasher.Hash(requestDto.Password).Returns((newHash, newSalt));

            userRepository.GetByEmailAsync(requestDto.Email).Returns((User?)null);

            var createdUser = new User
            {
                Id = 2,
                FullName = requestDto.FullName,
                Email = requestDto.Email,
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            userRepository.AddAsync(Arg.Any<User>()).Returns(createdUser);

            //  Act
            var result = await userService.CreateUserAsync(requestDto);

            //  Assert
            await userRepository.Received(1).GetByEmailAsync(requestDto.Email);

            passwordHasher.Received(1).Hash(requestDto.Password);

            await userRepository.Received(1).AddAsync(Arg.Is<User>(u =>
                u.PasswordHash == newHash && u.PasswordSalt == newSalt));

            Assert.NotNull(result);

            Assert.IsType<UserResponse>(result);

            Assert.Equal(createdUser.Id, result.Id);
            Assert.Equal(createdUser.FullName, result.FullName);
            Assert.Equal(createdUser.Email, result.Email);
            Assert.NotEqual(existingUser.PasswordHash, newHash);
            Assert.NotEqual(existingUser.PasswordSalt, newSalt);
            Assert.Equal(createdUser.Role, result.Role);
            Assert.Equal(createdUser.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async Task CreateUserAsync_WhenAddAsyncReturnsNull_ThrowsConflict()
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            var requestDto = new CreateUserRequest
            {
                FullName = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!"
            };

            passwordHasher.Hash(requestDto.Password).Returns((new byte[] { 0x03 }, new byte[] { 0x04 }));

            userRepository.AddAsync(Arg.Any<User>()).Returns((User?)null);

            //  Act
            var exception = await Assert.ThrowsAsync<ErrorResponse>(() => userService.CreateUserAsync(requestDto));

            var expected = Assert.Throws<ErrorResponse>(() => GlobalExceptionHandler.ThrowInvalidOperation());

            //  Assert
            await userRepository.Received(1).GetByEmailAsync(requestDto.Email);

            passwordHasher.Received(1).Hash(requestDto.Password);

            await userRepository.Received(1).AddAsync(Arg.Any<User>());
            
            Assert.Equal(expected.StatusCode, exception.StatusCode);
            Assert.Equal(expected.Message, exception.Message);
        }

        [Fact]
        public async Task CreateUserAsync_WhenRepositoryThrowsException_PropagatesException()
        {
            //  Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            var requestDto = new CreateUserRequest
            {
                FullName = "testuser",
                Email = "testuser@example.com",
                Password = "Password123!"
            };

            userRepository.GetByEmailAsync(Arg.Any<string>()).Throws(new Exception("Database error"));

            //  Act
            var exception = await Assert.ThrowsAsync<Exception>(() => userService.CreateUserAsync(requestDto));

            //  Assert
            await userRepository.Received(1).GetByEmailAsync(requestDto.Email);

            passwordHasher.DidNotReceive().Hash(Arg.Any<string>());

            await userRepository.DidNotReceive().AddAsync(Arg.Any<User>());

            Assert.Equal("Database error", exception.Message);

        }

        [Fact]
        public async Task UpdateUserAsync_WithValidIdAndRequest_ReturnsUserResponse()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 1;
            var previousUser = new User
            {
                Id = userId,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var existingUser = new User
            {
                Id = userId,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var requestDto = new UpdateUserRequest
            {
                FullName = "newname",
                Email = "new@example.com",
                Password = "NewPassword123!"
            };

            var expectedUser = new User
            {
                Id = userId,
                FullName = requestDto.FullName,
                Email = requestDto.Email,
                PasswordHash = new byte[] { 0x03 },
                PasswordSalt = new byte[] { 0x04 },
                Role = existingUser.Role,
                CreatedAt = existingUser.CreatedAt
            };

            // Act
            userRepository.GetByIdAsync(userId).Returns(existingUser);

            passwordHasher.Hash(requestDto.Password).Returns((expectedUser.PasswordHash, expectedUser.PasswordSalt));

            userRepository.UpdateAsync(Arg.Any<User>()).Returns(expectedUser);

            var result = await userService.UpdateUserAsync(userId, requestDto);

            // Assert
            await userRepository.Received(1).GetByIdAsync(Arg.Any<int>());

            passwordHasher.Received(1).Hash(Arg.Any<string>());

            await userRepository.Received(1).UpdateAsync(Arg.Any<User>());

            Assert.IsType<UserResponse>(result);

            Assert.Equal(requestDto.FullName, result.FullName);
            Assert.Equal(requestDto.Email, result.Email);
            Assert.NotEqual(previousUser.FullName, result.FullName);
            Assert.NotEqual(previousUser.Email, result.Email);
            Assert.Equal(previousUser.Role, result.Role);
            Assert.Equal(existingUser.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserNotFound_ReturnsNull()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 99;
            var requestDto = new UpdateUserRequest
            {
                FullName = "newname",
                Email = "new@example.com",
                Password = "NewPassword123!"
            };

            // Act
            
            userRepository.GetByIdAsync(userId).Returns((User?)null);

            var result = await userService.UpdateUserAsync(userId, requestDto);

            // Assert
            await userRepository.Received(1).GetByIdAsync(Arg.Any<int>());

            passwordHasher.DidNotReceive().Hash(Arg.Any<string>());

            await userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>());

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenNoValuesProvided_ReturnsNull()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 1;
            var previousUser = new User
            {
                Id = userId,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var existingUser = new User
            {
                Id = userId,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var requestDto = new UpdateUserRequest
            {
                FullName = null,
                Email = null,
                Password = null
            };

            // Act
            userRepository.GetByIdAsync(userId).Returns(existingUser);
            userRepository.UpdateAsync(Arg.Any<User>()).Returns(existingUser);

            var result = await userService.UpdateUserAsync(userId, requestDto);

            // Assert
            await userRepository.DidNotReceive().GetByIdAsync(Arg.Any<int>());

            passwordHasher.DidNotReceive().Hash(Arg.Any<string>());

            await userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>());

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserAsync_WithFullNameFieldUpdate_ReturnsUpdatedUserResponse()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 1;
            var previousUser = new User
            {
                Id = userId,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var existingUser = new User
            {
                Id = userId,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var requestDto = new UpdateUserRequest
            {
                FullName = "newname",
                Email = null,
                Password = null
            };

            var expectedUser = new User
            {
                Id = userId,
                FullName = requestDto.FullName,
                Email = existingUser.Email,
                PasswordHash = existingUser.PasswordHash,
                PasswordSalt = existingUser.PasswordSalt,
                Role = existingUser.Role,
                CreatedAt = existingUser.CreatedAt
            };

            // Act
            userRepository.GetByIdAsync(userId).Returns(existingUser);
            userRepository.UpdateAsync(Arg.Any<User>()).Returns(expectedUser);

            var result = await userService.UpdateUserAsync(userId, requestDto);

            // Assert
            await userRepository.Received(1).GetByIdAsync(Arg.Any<int>());

            passwordHasher.DidNotReceive().Hash(Arg.Any<string>());

            await userRepository.Received(1).UpdateAsync(Arg.Any<User>());

            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(requestDto.FullName, result.FullName);
            Assert.NotEqual(previousUser.FullName, result.FullName);
            Assert.Equal(previousUser.Email, result.Email);
            Assert.Equal(previousUser.Role, result.Role);
            Assert.Equal(existingUser.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async Task UpdateUserAsync_WithEmailFieldUpdate_ReturnsUpdatedUserResponse()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 1;
            var previousUser = new User
            {
                Id = userId,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var existingUser = new User
            {
                Id = userId,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var requestDto = new UpdateUserRequest
            {
                FullName = null,
                Email = "newname@example.com",
                Password = null
            };

            var expectedUser = new User
            {
                Id = userId,
                FullName = existingUser.FullName,
                Email = requestDto.Email,
                PasswordHash = existingUser.PasswordHash,
                PasswordSalt = existingUser.PasswordSalt,
                Role = existingUser.Role,
                CreatedAt = existingUser.CreatedAt
            };

            // Act
            userRepository.GetByIdAsync(userId).Returns(existingUser);
            userRepository.UpdateAsync(Arg.Any<User>()).Returns(expectedUser);

            var result = await userService.UpdateUserAsync(userId, requestDto);

            // Assert
            await userRepository.Received(1).GetByIdAsync(Arg.Any<int>());

            passwordHasher.DidNotReceive().Hash(Arg.Any<string>());

            await userRepository.Received(1).UpdateAsync(Arg.Any<User>());

            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(previousUser.FullName, result.FullName);
            Assert.Equal(requestDto.Email, result.Email);
            Assert.NotEqual(previousUser.Email, result.Email);
            Assert.Equal(previousUser.Role, result.Role);
            Assert.Equal(existingUser.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async Task UpdateUserAsync_WithPasswordFieldUpdate_ReturnsUpdatedUserResponse()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 1;
            var previousUser = new User
            {
                Id = userId,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var existingUser = new User
            {
                Id = userId,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var requestDto = new UpdateUserRequest
            {
                FullName = null,
                Email = null,
                Password = "newpassword"
            };

            var expectedUser = new User
            {
                Id = userId,
                FullName = existingUser.FullName,
                Email = existingUser.Email,
                PasswordHash = new byte[] { 0x03 },
                PasswordSalt = new byte[] { 0x05 },
                Role = existingUser.Role,
                CreatedAt = existingUser.CreatedAt
            };

            // Act
            userRepository.GetByIdAsync(userId).Returns(existingUser);

            passwordHasher.Hash(requestDto.Password).Returns((expectedUser.PasswordHash, expectedUser.PasswordSalt));

            userRepository.UpdateAsync(Arg.Any<User>()).Returns(expectedUser);

            var result = await userService.UpdateUserAsync(userId, requestDto);

            // Assert
            await userRepository.Received(1).GetByIdAsync(Arg.Any<int>());

            passwordHasher.Received(1).Hash(Arg.Any<string>());

            await userRepository.Received(1).UpdateAsync(Arg.Any<User>());

            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(previousUser.FullName, result.FullName);
            Assert.Equal(previousUser.Email, result.Email);
            Assert.Equal(previousUser.Role, result.Role);
            Assert.Equal(existingUser.CreatedAt, result.CreatedAt);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenRepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 1;
            var requestDto = new UpdateUserRequest
            {
                FullName = "newname",
                Email = "new@example.com",
                Password = "NewPassword123!"
            };

            // Act
            userRepository.GetByIdAsync(userId).Throws(new Exception("Database error"));

            var userServiceException = await Assert.ThrowsAsync<Exception>(() => userService.UpdateUserAsync(userId, requestDto));

            // Assert
            await userRepository.Received(1).GetByIdAsync(Arg.Any<int>());

            passwordHasher.DidNotReceive().Hash(Arg.Any<string>());

            await userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>());

            Assert.Equal("Database error", userServiceException.Message);
        }

        [Fact]
        public async Task UpdateUserAsync_WithWhitespaceFields_ThrowsArgumentException()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 1;
            var existingUser = new User
            {
                Id = 1,
                FullName = "oldname",
                Email = "old@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            var requestDto = new UpdateUserRequest
            {
                FullName = " ",
                Email = "",
                Password = "\n"
            };

            // Act
            userRepository.GetByIdAsync(userId).Returns(existingUser);
            userRepository.UpdateAsync(Arg.Any<User>()).Returns(existingUser);

            var result = await userService.UpdateUserAsync(userId, requestDto);

            // Assert
            await userRepository.DidNotReceive().GetByIdAsync(Arg.Any<int>());

            passwordHasher.DidNotReceive().Hash(Arg.Any<string>());

            await userRepository.DidNotReceive().UpdateAsync(Arg.Any<User>());

            Assert.Null(result);
        }

        [Fact]
        public async Task SoftDeleteUserAsync_WithValidIdAndExistingUser_ReturnsTrue()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 1;
            var existingUser = new User
            {
                Id = userId,
                FullName = "testuser",
                Email = "testuser@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            userRepository.GetByIdAsync(userId).Returns(existingUser);
            userRepository.SoftDeleteAsync(userId).Returns(true);

            // Act
            var result = await userService.SoftDeleteUserAsync(userId);

            // Assert
            await userRepository.Received(1).GetByIdAsync(userId);

            await userRepository.Received(1).SoftDeleteAsync(userId);

            Assert.True(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public async Task SoftDeleteUserAsync_WithIdLessThanOrEqualToZero_ThrowsInvalidArgument(int userId)
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            // Act
            var exception = await Assert.ThrowsAsync<ErrorResponse>(() => userService.SoftDeleteUserAsync(userId));

            var expected = Assert.Throws<ErrorResponse>(() => GlobalExceptionHandler.ThrowInvalidArgument());

            // Assert
            await userRepository.DidNotReceive().GetByIdAsync(Arg.Any<int>());

            await userRepository.DidNotReceive().SoftDeleteAsync(Arg.Any<int>());

            Assert.Equal(expected.StatusCode, exception.StatusCode);
            Assert.Equal(expected.Message, exception.Message);
        }

        [Fact]
        public async Task SoftDeleteUserAsync_WhenUserNotFound_ThrowsNotFound()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 99;

            userRepository.GetByIdAsync(userId).Returns((User?)null);

            // Act
            var exception = await Assert.ThrowsAsync<ErrorResponse>(() => userService.SoftDeleteUserAsync(userId));

            var expected = Assert.Throws<ErrorResponse>(() => GlobalExceptionHandler.ThrowNotFound());

            // Assert
            await userRepository.Received(1).GetByIdAsync(userId);

            await userRepository.DidNotReceive().SoftDeleteAsync(Arg.Any<int>());

            Assert.Equal(expected.StatusCode, exception.StatusCode);
            Assert.Equal(expected.Message, exception.Message);
        }

        [Fact]
        public async Task SoftDeleteUserAsync_WhenSoftDeleteAsyncReturnsFalse_ReturnsFalse()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 1;
            var existingUser = new User
            {
                Id = userId,
                FullName = "testuser",
                Email = "testuser@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            userRepository.GetByIdAsync(userId).Returns(existingUser);
            userRepository.SoftDeleteAsync(userId).Returns(false);

            // Act
            var result = await userService.SoftDeleteUserAsync(userId);

            // Assert
            await userRepository.Received(1).GetByIdAsync(userId);

            await userRepository.Received(1).SoftDeleteAsync(userId);

            Assert.False(result);
        }

        [Fact]
        public async Task SoftDeleteUserAsync_WhenRepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var userRepository = Substitute.For<IUserRepository>();
            var passwordHasher = Substitute.For<IPasswordHasher>();
            var userService = new UserService(userRepository, passwordHasher);

            int userId = 1;
            var existingUser = new User
            {
                Id = userId,
                FullName = "testuser",
                Email = "testuser@example.com",
                PasswordHash = new byte[] { 0x01 },
                PasswordSalt = new byte[] { 0x02 },
                Role = Role.Entrepreneur,
                CreatedAt = DateTime.UtcNow
            };

            userRepository.GetByIdAsync(userId).Returns(existingUser);
            userRepository.SoftDeleteAsync(userId).Throws(new Exception("Database error"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => userService.SoftDeleteUserAsync(userId));

            // Assert
            await userRepository.Received(1).GetByIdAsync(userId);

            await userRepository.Received(1).SoftDeleteAsync(userId);

            Assert.Equal("Database error", exception.Message);
        }
    }
}
