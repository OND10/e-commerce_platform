using Email.API.DataBase;
using Email.API.Entities;
using Email.API.Features.DTOs.CartDTOs;
using Email.API.Message;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Email.API.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dboptions;

        public EmailService(DbContextOptions<AppDbContext> dboptions)
        {
            _dboptions = dboptions;
        }

        public async Task CreateUserAccountLog(string email)
        {
            string message = "Account is Created successfully with this . <br/> Email :" + email;

            await LogAnEmail(message, "osamadammag84@gmail.com");
        }

        public async Task EmailLoggingCart(CartDto cart)
        {
            StringBuilder messageStringBuilder = new StringBuilder();
            messageStringBuilder.Append("<br/> Cart Email Requested");
            messageStringBuilder.Append("<br/> Total : " + cart.CartHeaderResponse.CartTotal);
            messageStringBuilder.Append("<br/>");
            messageStringBuilder.Append("<ul>");
            foreach(var item in cart.CartDetailsResponse)
            {
                messageStringBuilder.Append("<li>");
                messageStringBuilder.Append(item.Product.Name + "x" + item.Count);
                messageStringBuilder.Append("</li>");
            }
            messageStringBuilder.Append("</ul>");

            await LogAnEmail(messageStringBuilder.ToString(), cart.CartHeaderResponse.Email);
        }

        public async Task LogOrderPlaced(RewardsMessage rewardsMessage)
        {
            string message = "New Order Placed. <br/> Order ID : " + rewardsMessage.OrderId;
            await LogAnEmail(message, "osamadammag84@gmail.com");
        }

        private async Task<bool>LogAnEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLogger = new()
                {
                    Email = email,
                    Message = message,
                    EmailSent = DateTime.Now,
                };
                await using var _db = new AppDbContext(_dboptions);
                await _db.EmailLoggers.AddAsync(emailLogger);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}
