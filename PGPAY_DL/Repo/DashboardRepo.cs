using Microsoft.EntityFrameworkCore;
using PGPAY_DL.Context;
using PGPAY_DL.IRepo;
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

        public async Task<ResponseModel> GetUserDetails(int HostelId)
        {
            try
            {
                var UserData = await (from U in _context.Users
                                      join UD in _context.UserDetails on U.UserId equals UD.UserId
                                      join H in _context.HostelDetails on U.HostelId equals H.HostelId
                                      where H.HostelId == HostelId
                                      select new GetUserDetailsModel
                                      {
                                          UserId = U.UserId,
                                          Name = UD.Name,
                                          Age = UD.Age,
                                          PhoneNo = UD.PhoneNo,
                                          DateOfBirth = UD.DateOfBirth,
                                          MaritalStatus = UD.MaritalStatus,
                                          State = UD.State,
                                          Address = UD.Address,
                                          AltPhoneNo = UD.AltPhoneNo,
                                          Sex = UD.Sex,
                                          CreateDate = UD.CreateDate
                                      }).ToListAsync();
                if (UserData.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.Content = UserData;
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
