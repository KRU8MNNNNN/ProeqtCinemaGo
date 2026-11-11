namespace CinemaGoProject.Services
{
    internal interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string message);
    }
}