using Microsoft.EntityFrameworkCore;
using PGPAY_DL.Context;
using PGPAY_DL.IRepo;
using PGPAY_DL.Models.DB;
using PGPAY_Model.Model.Dashboard;
using PGPAY_Model.Model.Response;

namespace PGPAY_DL.Repo
{
    public class DashboardRepo : IDashboardRepo
    {
        private readonly ResponseModel response = new();
        private readonly PGPAYContext _context;
        public DashboardRepo(PGPAYContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> GetUserDetails(int UserId)
        {
            try
            {
                var hostelIds = await _context.HostelDetails
                                           .Where(x => x.UserId == UserId)
                                           .Select(x => x.HostelId)
                                           .ToListAsync();
                var TotalUser = _context.UserDetails.Where(x => x.UserId == UserId).Count();

                var HostelData = await (from H in _context.HostelDetails
                                        join D in _context.Discounts on H.HostelId equals D.HostelId into DGroup
                                        from D in DGroup.DefaultIfEmpty()
                                        where hostelIds.Contains(H.HostelId)
                                        select new GetUserDetailsModel
                                        {
                                            HostelId = H.HostelId,
                                            HostelName = H.HostelName,
                                            HostelAddress = H.HostalAddress,
                                            NoofRooms = H.NoOfRooms,
                                            Rent = H.Rent,
                                            DiscountPer = D.Offerper,
                                            MinimunmRent = H.MinimumRent
                                        }).ToListAsync();
                if (HostelData.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.Content = new
                    {
                        HostelDetails = HostelData,
                        TotalUser = TotalUser,
                        NoOfHostels = HostelData.Count(),
                        TotalIncome = HostelData.Sum(x => x.MinimunmRent),
                        PendingPaymentCount = 1,
                        minamt = HostelData.Min(x => x.MinimunmRent)
                    };
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "User is already exists!!!";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
    }
}
