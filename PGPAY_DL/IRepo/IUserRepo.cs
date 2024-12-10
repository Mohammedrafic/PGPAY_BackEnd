using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_DL.IRepo
{
    public interface IUserRepo
    {
        Task<ResponseModel> AddUser(UserDetailsdto UserDetails);
        Task<ResponseModel> GetLayoutData(string UserRole);
        Task<ResponseModel> GetUserDetailsById(int UserId, int hostelId);
    }
}
