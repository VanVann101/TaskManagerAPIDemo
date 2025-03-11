using TaskManager.Application.Common;

namespace TaskManager.Application.Contracts {
    public interface IEmailService {
        Task<Result> SendEmailAsync(string email, string message, string subject = "");
    }
}
