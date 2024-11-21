using PGPAY_BL.IService;
using PGPAY_DL.IRepo;
using PGPAY_DL.Models.DB;
using PGPAY_Model.Model.Response;

namespace PGPAY_BL.Service
{
    public class DashboardService: IDashboardService
    {
        private readonly IDashboardRepo _repo;
        public DashboardService(IDashboardRepo repo)
        {
            _repo = repo;
        }

        public async Task<ResponseModel> GetMinimumRent(int HostelId)
        {
            return await _repo.GetMinimumRent(HostelId);
        }

        public async Task<ResponseModel> GetUserDetails(int UserId)
        {
            return await _repo.GetUserDetails(UserId);
        }
    }
}
