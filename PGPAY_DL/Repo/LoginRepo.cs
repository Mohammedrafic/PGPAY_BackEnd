using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGPAY_DL.Context;
using PGPAY_DL.IRepo;
using PGPAY_DL.Models.DB;
using PGPAY_Model.Model.Login;
using PGPAY_Model.Model.Response;
using System.Net;
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
            try
            {
                var result = await _context.Users.Where(x => x.Email == Email).FirstOrDefaultAsync();
                if (result != null)
                {
                    Guid guid = Guid.NewGuid();
                    result.UniqueKey = guid;
                    _ = _context.Update(result);
                    await _context.SaveChangesAsync();
                    string resetLink = $"https://pgpayf.github.io/PGPAYForgotPassword/Id/{guid}";
                    string body = $"Dear {result.UserName},\r\n\r\nWe received a request from this mail '{Email}' to reset the password for your account. If you made this request, please click the link below to reset your password:\r\n\r\n{resetLink}\r\n\r\nIf you did not request a password reset, please ignore this email or contact our support team if you have concerns.\r\n\r\nThank you,\r\nMohammed Rafic S/ mohammedrafic121@gmail.com";

                    MailAddress to = new MailAddress(Email);
                    MailAddress from = new MailAddress("mohammedrafic.s@colanonline.com");
                    MailMessage email = new MailMessage(from, to);
                    email.Body = body;
                    email.Subject = "Forgot Password";


                    using var smtp = new SmtpClient();
                    smtp.Credentials = new NetworkCredential("mohammedrafic.s@colanonline.com", "pzURJ!*4");
                    smtp.Host = "smtp.colanonline.com";
                    smtp.Port = 587;
                    smtp.Send(email);

                    response.IsSuccess = true;
                    response.Message = "Your password reset request has been received. Please check your email for further instructions.";
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
                response.Message = $"Error sending email: {smtpEx.Message}";
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
