using PGPAY_DL.dto;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_BL.IService
{
    public interface IHostelService
    {
        Task<ResponseModel> GetAllHostelDetails();
        Task<ResponseModel> GetHostelDetailsById(int UserId);
        Task<ResponseModel> GetHostelRequestById(int UserId);
        Task<ResponseModel> GetHostelFullDetailsById(int HostelID);
        Task<ResponseModel> HostelBookingRequest(BookingRequestDto bookingRequest);
        Task<ResponseModel> AddHostelDetails(HostelDetails bookingRequest);
    }
}
