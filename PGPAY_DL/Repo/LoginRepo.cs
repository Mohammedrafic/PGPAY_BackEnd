using Microsoft.EntityFrameworkCore;
using PGPAY_DL.Context;
using PGPAY_DL.IRepo;
using PGPAY_Model.Model.Response;


namespace PGPAY_DL.Repo
{
    public class LoginRepo : ILoginRepo
    {
        private readonly ResponseModel response = new();
        private readonly PGPAYContext _context;
        public LoginRepo(PGPAYContext context)
        {
            _context = context;
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
    }
}
