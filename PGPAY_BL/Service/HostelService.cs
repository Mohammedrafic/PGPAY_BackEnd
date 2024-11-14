using PGPAY_BL.IService;
using PGPAY_DL.IRepo;
using PGPAY_DL.Repo;
using PGPAY_Model.Model.Response;

namespace PGPAY_BL.Service
{
    public class HostelService: IHostelService
    {
        private readonly IHostelRepo _repo;
        public HostelService(IHostelRepo repo)
        {
            _repo = repo;
        }

        public async Task<ResponseModel> GetAllHostelDetails()
        {
            return await _repo.GetAllHostelDetails();
        }

        public async Task<ResponseModel> GetHostelDetailsById(int UserId)
        {
            return await _repo.GetHostelDetailsById(UserId);
        }

        public async Task<ResponseModel> GetHostelRequestById(int UserId)
        {
            return await _repo.GetHostelRequestById(UserId);

        }
    }
}
