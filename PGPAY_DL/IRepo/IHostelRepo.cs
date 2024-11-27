using PGPAY_DL.dto;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;
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
        Task<ResponseModel> GetHostelDetailsById(HostelFilter UserID);
        Task<ResponseModel> GetHostelRequestById(int UserID);
        Task<ResponseModel> GetHostelFullDetailsById(int HostelID);
        Task<ResponseModel> HostelBookingRequest(BookingRequestDto bookingRequest);
        Task<ResponseModel> AddHostelDetails(HostelDetails bookingRequest);
        Task<ResponseModel> RatingHostel(Ratingdto Ratingdto);

    }
}
