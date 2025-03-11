using TaskManager.Application.Common;
using TaskManager.Application.Common.DTO;

namespace TaskManager.Application.Contracts {
    public interface IUserService {
        Task<Result<UserDto>> CreateUserAsync(CreateUserDto userDTO);
        Task<Result<UserDto?>> GetUserByIdAsync(Guid userId);
        Task<Result<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<Result> UpdateUserAsync(Guid userId, UpdateUserDto userDto);  
        Task<Result> DeleteUserAsync(Guid userId);
        Task<Result> AuthenticateAsync(string email, string password);
    }
}
