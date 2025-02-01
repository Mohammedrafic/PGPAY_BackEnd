using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGPAY_DL.Context;
using PGPAY_DL.IRepo;
using PGPAY_DL.Models.DB;
using PGPAY_Model.Model.Login;
using PGPAY_Model.Model.Response;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using static System.Net.WebRequestMethods;

namespace PGPAY_DL.Repo
{
    public class LoginRepo : ILoginRepo
    {
        private readonly ResponseModel response = new();
        private readonly PGPAYContext _context;
        private readonly IConfiguration _configuration;
        public LoginRepo(PGPAYContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResponseModel> ForgotPassword(string Email)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                var result = await _context.Users.FirstOrDefaultAsync(x => x.Email == Email);

                if (result != null)
                {
                    // Read email credentials from config
                    string FromEmail = _configuration["FromEmail"];
                    string UserName = _configuration["UserEName"];
                    string Password = _configuration["Password"];

                    // Generate a unique key for password reset
                    Guid guid = Guid.NewGuid();
                    result.UniqueKey = guid;
                    _context.Users.Update(result);
                    await _context.SaveChangesAsync();

                    // Construct reset link and email body
                    string resetLink = $"https://pgpayforgotpassword.web.app/Id/{guid}";
                    string body = $@"
                    Dear {result.UserName},

                    We received a request from this email ('{Email}') to reset the password for your account.
                    If you made this request, please click the link below to reset your password:

                    {resetLink}

                    If you did not request a password reset, please ignore this email or contact our support team.

                    Thank you,
                    Mohammed Rafic S
                    mohammedrafic121@gmail.com";

                    // Configure and send email
                    MailMessage email = new MailMessage
                    {
                        From = new MailAddress(FromEmail),
                        Subject = "Forgot Password",
                        Body = body,
                        IsBodyHtml = false
                    };
                    email.To.Add(new MailAddress(Email));

                    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.EnableSsl = true;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(UserName, Password);

                        await smtp.SendMailAsync(email);
                    }

                    response.IsSuccess = true;
                    response.Message = "Your password reset request has been received. Please check your email.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Email is incorrect";
                }
            }
            catch (SmtpException smtpEx)
            {
                response.IsSuccess = false;
                response.Message = $"SMTP error: {smtpEx.Message}";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An unexpected error occurred: {ex.Message}";
            }

            return response;
        }


        public async Task<ResponseModel> GetUniqueIdForForgotPassword(Guid uniqueId)
        {
            try
            {
                var result = await _context.Users.Where(x => x.UniqueKey == uniqueId).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.UniqueKey = null;
                    _context.Update(result);
                    await _context.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Content = 1;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = "Something went wrong!!!!";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = true;
                response.Message = $"An unexpected error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ResponseModel> Login(string Email, string Password)
        {
            var result = await _context.Users.Where(x => x.Email == Email && x.Password == Password).FirstOrDefaultAsync();
            if (result == null)
            {
                response.IsSuccess = false;
                response.Message = "Email or Password is incorrect";
            }
            else
            {
                response.IsSuccess = true;
                response.Content = result;
            }
            return response;
        }

        public async Task<ResponseModel> resetpassword(ResetPassword RPassword)
        {
            try
            {
                var result = await _context.Users.Where(x => x.Email == RPassword.Email).FirstOrDefaultAsync();
                if (result != null)
                {
                    if (RPassword.Password == RPassword.ConfirmPassword)
                    {
                        result.Password = RPassword.Password;
                        _ = _context.Update(result);
                        await _context.SaveChangesAsync();
                        response.IsSuccess = true;
                        response.Message = "Password Updated Successfully";
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.Message = "Password and Confirm Password Is Incorrect";
                    }
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = "Email is incorrect";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = true;
                response.Message = $"An unexpected error occurred: {ex.Message}";
            }

            return response;
        }
    }
}
