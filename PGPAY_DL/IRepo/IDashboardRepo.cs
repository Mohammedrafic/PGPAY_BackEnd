using PGPAY_Model.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_DL.IRepo
{
    public interface IDashboardRepo
    {
        Task<ResponseModel> GetUserDetails(int HostelId);
        Task<ResponseModel> GetMinimumRent(int HostelId);
    }
}
