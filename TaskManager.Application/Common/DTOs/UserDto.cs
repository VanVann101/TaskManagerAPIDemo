using TaskManager.Domain.Entities;

namespace TaskManager.Application.Common.DTO {
    public class UserDto {
        public UserDto(ApplicationUser user) {
            if (user != null) {
                Id = user.Id;
                FirstName = user.FirstName;
                LastName = user.LastName;
                Email = user.Email;
                Phone = user.PhoneNumber;
                CreatedAt = user.CreatedAt;
            }
        }

        public UserDto() {
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
