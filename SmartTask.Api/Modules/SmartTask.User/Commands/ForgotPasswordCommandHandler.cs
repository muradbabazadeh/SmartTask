using MediatR;
using SmartTask.Domain.AggregatesModel.UserAggregate;
using SmartTask.Domain.Exceptions;
using SmartTask.Identity.Queries;
using SmartTask.Infrastructure.Commands;
using SmartTask.Infrastructure.Idempotency;
using SmartTask.SharedKernel.Infrastructure;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask.User.Commands
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserQueries _userService;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IUserQueries userQueries)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _userService = userQueries ?? throw new ArgumentNullException(nameof(userQueries));
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new DomainException("Provided user name is not associated with any account");
            }

            if (user.Locked)
            {
                throw new DomainException("Unable to reset password. Please, contact administrator");
            }

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new DomainException("User does not have an email address");
            }

            var newPassword = Guid.NewGuid().ToString("N").Substring(0, 8);

            var newPasswordHash = PasswordHasher.HashPassword(newPassword);
            user.ResetPassword(newPasswordHash);

            var result = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            SendPasswordResetEmail(user.Email, newPassword);

            return result;
        }
        public async void SendPasswordResetEmail(string userName, string password)
        {
            var user = await _userService.FindByNameAsync(userName);

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            client.UseDefaultCredentials = false;
            client.TargetName = "SmartTask.AZ";
            client.Credentials = new NetworkCredential("SmartTaskpharmabaku@gmail.com", "SmartTask135%");
            MailMessage message = new MailMessage(new MailAddress("SmartTaskpharmabaku@gmail.com", "SmartTask Pharmaceuticals CRM"), new MailAddress(user.Email));
            client.EnableSsl = true;
            message.IsBodyHtml = true;
            message.Subject = "CRM şifrəniz yeniləndi";
            message.Body = @"<html><body><p><strong>Hörmətli, "+user.FirstName+" "+user.LastName+ ", </strong> </p><br></br><p>SmartTask Pharmaceuticals CRM tətbiqi üçün istifadəçi şifrəniz yeniləndi. Yeni şifrəniz aşağıdakı kimidir:<br></br> <br></br><strong>" + password + "<strong></p><br></br> Hər hansı çətinliyiniz olarsa sistem administratoru ilə əlaqə saxlamağınız tövsiyyə olunur.</p><br><p> Hörmətlə,</p><p> SmartTask Pharmaceuticals </p> ";
            message.IsBodyHtml = true;
            await client.SendMailAsync(message);
        }

    }

    public class ForgotPasswordIdentifiedCommandHandler : IdentifiedCommandHandler<ForgotPasswordCommand, bool>
    {
        public ForgotPasswordIdentifiedCommandHandler(IMediator mediator, IRequestManager requestManager) : base(mediator, requestManager)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true;
        }
    }
}
