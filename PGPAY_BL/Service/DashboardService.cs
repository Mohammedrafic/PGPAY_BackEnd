using PGPAY_BL.IService;
using PGPAY_DL.IRepo;
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

        public async Task<ResponseModel> GetUserDetails(int HostelId)
        {
            return await _repo.GetUserDetails(HostelId);
        }
    }
}
