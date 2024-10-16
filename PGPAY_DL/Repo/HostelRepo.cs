using Microsoft.EntityFrameworkCore;
using PGPAY_DL.Context;
using PGPAY_DL.IRepo;
using PGPAY_Model.Model.Response;

namespace PGPAY_DL.Repo
{
    public class HostelRepo : IHostelRepo
    {
        private readonly ResponseModel response = new();
        private readonly PGPAYContext _context;
        public HostelRepo(PGPAYContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> GetAllHostelDetails()
        {
            try
            {
                var HostelDetails = await (from HDetails in _context.HostelDetails.AsNoTracking()
                                           join Ratings in _context.Ratings on HDetails.HostelId equals Ratings.HostelId
                                           join Discnt in _context.Discounts on HDetails.HostelId equals Discnt.HostelId
                                           join Photos in _context.HostelPhotos on HDetails.HostelId equals Photos.HostelId into hostelPhotosGroup
                                           select new
                                           {
                                               HostelDetails = HDetails,
                                               Ratings = Ratings,
                                               Discounts = Discnt,
                                               Photos = hostelPhotosGroup.ToList(),
                                               HostelFacility = _context.HostelFacilities.ToList(),
                                           }).ToListAsync();
                HostelDetails.ForEach(x =>
                {
                    int offerPercentage = int.Parse(x.Discounts.Offerper.Replace("% off", ""));
                    x.HostelDetails.Rent = (x.HostelDetails.MinimumRent * offerPercentage) / 100;
                });
                if (HostelDetails.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.Content = HostelDetails;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Error!!!!";
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
