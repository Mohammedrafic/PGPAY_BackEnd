using PGPAY_BL.IService;
using PGPAY_DL.dto;
using PGPAY_DL.IRepo;
using PGPAY_DL.Models.DB;
using PGPAY_DL.Repo;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;

namespace PGPAY_BL.Service
{
    public class HostelService: IHostelService
    {
        private readonly IHostelRepo _repo;
        public HostelService(IHostelRepo repo)
        {
            _repo = repo;
        }

        public async Task<ResponseModel> AddHostelDetails(HostelDetails bookingRequest)
        {
            return await _repo.AddHostelDetails(bookingRequest);
        }

        public async Task<ResponseModel> GetAllHostelDetails()
        {
            return await _repo.GetAllHostelDetails();
        }

        public async Task<ResponseModel> GetHostelDetailsById(int UserId)
        {
            return await _repo.GetHostelDetailsById(UserId);
        }

        public async Task<ResponseModel> GetHostelFullDetailsById(int HostelID)
        {
            return await _repo.GetHostelFullDetailsById(HostelID);
        }

        public async Task<ResponseModel> GetHostelRequestById(int UserId)
        {
            return await _repo.GetHostelRequestById(UserId);

        }

        public async Task<ResponseModel> HostelBookingRequest(BookingRequestDto bookingRequest)
        {
            return await _repo.HostelBookingRequest(bookingRequest);
        }
    }
}
