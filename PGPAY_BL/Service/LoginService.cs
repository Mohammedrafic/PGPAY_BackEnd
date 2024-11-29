using PGPAY_BL.IService;
using PGPAY_DL.IRepo;
using PGPAY_Model.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_BL.Service
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepo _repo;
        public LoginService(ILoginRepo repo)
        {
            _repo= repo;
        }

        public async Task<ResponseModel> ForgotPassword(string Email)
        {
            return await _repo.ForgotPassword(Email);
        }

        public async Task<ResponseModel> Login(string Email, string Password)
        {
            return await _repo.Login(Email, Password);
        }
    }
}
