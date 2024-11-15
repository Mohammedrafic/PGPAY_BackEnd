using Microsoft.EntityFrameworkCore;
using PGPAY_DL.Context;
using PGPAY_DL.dto;
using PGPAY_DL.IRepo;
using PGPAY_DL.Models.DB;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;

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
                                               Photos = GetPhotos(hostelPhotosGroup.ToList()),
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
        private static List<string> GetPhotos(List<HostelPhoto> hostelPhotos)
        {
            var photoUrls = new List<string>();
            foreach (var photo in hostelPhotos)
            {
                var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", photo.FileName);

                if (System.IO.File.Exists(photoPath))
                {
                    var bytes = System.IO.File.ReadAllBytes(photoPath);
                    var base64Image = Convert.ToBase64String(bytes);
                    var imageUrl = $"data:image/jpeg;base64,{base64Image}";
                    photoUrls.Add(imageUrl);
                }
                else
                {
                    photoUrls.Add("assets\\images\\beds-hostel-affordable-housing-36997317.webp");
                }
            }
            return photoUrls;
        }

        public async Task<ResponseModel> GetHostelRequestById(int userID)
        {
            var response = new ResponseModel();

            try
            {
                // Fetch all hostel IDs associated with the user
                var hostelIds = await _context.HostelDetails
                                              .Where(x => x.UserId == userID)
                                              .Select(x => x.HostelId)
                                              .ToListAsync();

                // Fetch the main request data
                var request = await (from hr in _context.HostelRequests
                                     join hd in _context.HostelDetails on hr.HostelId equals hd.HostelId
                                     join ud in _context.UserDetails on hr.UserId equals ud.UserId
                                     where hostelIds.Contains(hr.HostelId)
                                     select new HostelRequestDto
                                     {
                                         UserID = hr.UserId,
                                         HostelId = hr.HostelId,
                                         RequestId = hr.RequestId,
                                         HostelName = hd.HostelName,
                                         RequestType = hr.RequestType,
                                         UserName = ud.Name,
                                         RequestDate = hr.RequestDate,
                                         Status = hr.Status,
                                         Description = hr.Description,
                                         AssignedTo = hr.AssignedTo,
                                         Response = hr.Response,
                                         ContactDetails = ud.PhoneNo,
                                         LastUpdated = hr.LastUpdated
                                     })
                                     .GroupBy(x => x.RequestId)
                                     .Select(g => g.First())
                                     .ToListAsync();

                var payments = _context.Payments
                                       .Where(x => hostelIds.Contains(x.HostelId))
                                       .AsEnumerable()
                                       .Select(p => new
                                       {
                                           HostelName = request.FirstOrDefault(x => x.UserID == p.UserId)?.HostelName,
                                           UserName = request.FirstOrDefault(x => x.UserID == p.UserId)?.UserName,
                                           Amount = p.Amount,
                                           PaymentMethod = p.PaymentMethod,
                                           TransactionId = p.TransactionId,
                                           IsAdvancedPayment = p.IsAdvancePayment,
                                           PaymentStatus = p.PaymentStatus,
                                           AdvanceAmount = p.AdvanceAmount,
                                           RemainingAmount = p.RemainingBalance,
                                           Remarks = p.Remarks,
                                           UpdatedAt = p.UpdatedAt
                                       }).ToList();

                if (request.Any())
                {
                    response.IsSuccess = true;
                    response.Content = new
                    {
                        Details = request,
                        Payment = payments
                    };
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No requests found for the specified User ID.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ResponseModel> GetHostelDetailsById(int UserID)
        {
            try
            {
                var HostelDetails = await (from HDetails in _context.HostelDetails.AsNoTracking()
                                           join Ratings in _context.Ratings on HDetails.HostelId equals Ratings.HostelId
                                           join Discnt in _context.Discounts on HDetails.HostelId equals Discnt.HostelId
                                           join Photos in _context.HostelPhotos on HDetails.HostelId equals Photos.HostelId into hostelPhotosGroup
                                           where HDetails.UserId == UserID
                                           select new
                                           {
                                               HostelDetails = HDetails,
                                               Ratings = Ratings,
                                               Discounts = Discnt,
                                               Photos = GetPhotos(hostelPhotosGroup.ToList()),
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

        public async Task<ResponseModel> HostelBookingRequest(BookingRequestDto bookingRequest)
        {
            try
            {
                var exisingReq = await _context.HostelRequests.Where(x => x.UserId == bookingRequest.UserId && x.RequestType == "Booking").ToListAsync();
                if (exisingReq.Count() > 0)
                {
                    var Payment = await _context.Payments.Where(x => x.UserId == bookingRequest.UserId && x.PaymentStatus == "Completed" && x.HostelId == bookingRequest.HostelId).FirstOrDefaultAsync();
                    if (Payment != null && Payment.IsAdvancePayment == true)
                    {
                        response.IsSuccess = true;
                        response.Message = "User already booked this Hostel";
                    }
                }
                else
                {
                    HostelRequest hostelRequest = new HostelRequest
                    {
                        RequestType = bookingRequest.RequestType,
                        HostelId = bookingRequest.HostelId,
                        UserId = bookingRequest.UserId,
                        RequestDate = DateTime.Now,
                        Status = bookingRequest.Status,
                        Description = bookingRequest.Description,
                        AssignedTo = bookingRequest.AssignedTo,
                        Response = bookingRequest.Response,
                        ContactDetails = bookingRequest.ContactDetails,
                        IsResolved = bookingRequest.IsResolved,
                        LastUpdated = DateTime.Now
                    };
                    await _context.AddAsync(hostelRequest);
                    await _context.SaveChangesAsync();

                    if (bookingRequest.payment != null)
                    {
                        var existPayment = await _context.Payments.Where(x => x.UserId == bookingRequest.UserId && x.PaymentStatus == "Completed" && x.HostelId == bookingRequest.HostelId).FirstOrDefaultAsync();

                        if (existPayment == null)
                        {
                            Payment payment = new Payment
                            {
                                HostelId = bookingRequest.payment.HostelId,
                                UserId = bookingRequest.payment.UserId,
                                PaymentDate = DateTime.Now,
                                Amount = bookingRequest.payment.Amount,
                                PaymentMethod = bookingRequest.payment.PaymentMethod,
                                TransactionId = bookingRequest.payment.TransactionId,
                                PaymentStatus = bookingRequest.payment.PaymentStatus,
                                IsAdvancePayment = bookingRequest.payment.IsAdvancePayment,
                                AdvanceAmount = bookingRequest.payment.AdvanceAmount,
                                RemainingBalance = bookingRequest.payment.RemainingBalance,
                                Remarks = bookingRequest.payment.Remarks,
                                CreatedAt = DateTime.Now,
                                UpdatedAt = DateTime.Now
                            };
                            await _context.AddAsync(payment);
                            await _context.SaveChangesAsync();
                        }
                    }
                    response.IsSuccess = true;
                    response.Content = hostelRequest.HostelId;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }

        public async Task<ResponseModel> GetHostelFullDetailsById(int HostelID)
        {
            try
            {
                var Hostels = await (from hd in _context.HostelDetails
                                     join d in _context.Discounts
                                         on hd.HostelId equals d.HostelId into discountJoin
                                     from discount in discountJoin.DefaultIfEmpty()
                                     join r in _context.Ratings
                                         on hd.HostelId equals r.HostelId into ratingJoin
                                     from rating in ratingJoin.DefaultIfEmpty()
                                     where hd.HostelId == HostelID
                                     select new
                                     {
                                         Name = hd.HostelName,
                                         Location = hd.HostalAddress,
                                         Rating = rating.Starrate,
                                         ReviewCount = rating.TotalRatings,
                                         ReviewText = "Good size room complete with sitting area for two. Even had a fridge. Staff were so helpful and helped us arrange our onward journey.",
                                         Reviewer = new
                                         {
                                             Name = hd.OwnerName,
                                             Country = "Chennai"
                                         },
                                         Amenities = _context.HostelFacilities.Select(x => new
                                         {
                                             Icon = x.Imgpath,
                                             Label = x.Name
                                         }).ToList()
                                     }).FirstOrDefaultAsync();
                if (Hostels != null)
                {
                    response.IsSuccess = true;
                    response.Content = Hostels;
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
