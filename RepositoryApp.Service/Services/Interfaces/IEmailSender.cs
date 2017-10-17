using System.Threading.Tasks;

namespace RepositoryApp.Service.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}