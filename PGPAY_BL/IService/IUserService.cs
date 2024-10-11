using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_BL.IService
{
    public interface IUserService
    {
        Task<ResponseModel> AddUser(UserDetailsdto UserDetails);
    }
}
