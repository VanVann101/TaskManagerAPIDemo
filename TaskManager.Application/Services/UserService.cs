using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common;
using TaskManager.Application.Common.Constants;
using TaskManager.Application.Common.DTO;
using TaskManager.Application.Common.Validator;
using TaskManager.Application.Contracts;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services {
    public class UserService : IUserService {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Result<UserDto?>> GetUserByIdAsync(Guid userId) {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null) {
                return Result<UserDto?>.Failure(ErrorMessages.NotFound);
            }

            return Result<UserDto?>.Success(new UserDto(user));
        }

        public async Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync() {
            var users = await _userManager.Users.ToListAsync();
            var usersDtos = users.Select(u => new UserDto(u));
            return Result<IEnumerable<UserDto>>.Success(usersDtos);
        }

        public async Task<Result<UserDto>> CreateUserAsync(CreateUserDto userDto) {
            var validator = new CreateUserDtoValidator();
            var validationResult = validator.Validate(userDto);

            if (!validationResult.IsValid) {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage);
                return Result<UserDto>.Failure(errors.First());
            }

            var user = new ApplicationUser {
                UserName = userDto.Email,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                PhoneNumber = userDto.Phone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded) {
                return Result<UserDto>.Success(new UserDto(user));
            }
            else {
                return Result<UserDto>.Failure(result.Errors.First().Description);
            }
        }

        public Task<Result> UpdateUserAsync(Guid userId, UpdateUserDto userDto) {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteUserAsync(Guid userId) {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null) {
                return Result.Failure(ErrorMessages.NotFound);
            }

            var result = await _userManager.DeleteAsync(user);
            return Result.Success();
        }

        public async Task<Result> AuthenticateAsync(string email, string password) {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null) {
                return Result.Failure(ErrorMessages.NotFound);
            }

            var result = await _userManager.CheckPasswordAsync(user, password);
            return result ? Result.Success() : Result.Failure(ErrorMessages.InvalidAuthData);
        }
    }
}
