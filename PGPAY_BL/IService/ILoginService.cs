using PGPAY_Model.Model.Login;
using PGPAY_Model.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_BL.IService
{
    public interface ILoginService
    {
        Task<ResponseModel> Login(string Email, string Password);
        Task<ResponseModel> ForgotPassword(string Email);
        Task<ResponseModel> resetpassword(ResetPassword RPassword);
    }
}
