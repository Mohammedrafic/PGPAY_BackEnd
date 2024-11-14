using PGPAY_DL.dto;
using PGPAY_Model.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_DL.IRepo
{
    public interface IHostelRepo
    {
        Task<ResponseModel> GetAllHostelDetails();
        Task<ResponseModel> GetHostelDetailsById(int UserID);
        Task<ResponseModel> GetHostelRequestById(int UserID);
        Task<ResponseModel> HostelBookingRequest(BookingRequestDto bookingRequest);

    }
}
