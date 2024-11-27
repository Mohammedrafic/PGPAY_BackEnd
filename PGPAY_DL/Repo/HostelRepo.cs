using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PGPAY_DL.Context;
using PGPAY_DL.dto;
using PGPAY_DL.IRepo;
using PGPAY_DL.Models.DB;
using PGPAY_Model.Enums;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PGPAY_DL.Repo
{
    public class HostelRepo : IHostelRepo
    {
        private readonly ResponseModel response = new();
        private readonly PGPAYContext _context;
        private readonly IConfiguration _configuration;
        public HostelRepo(PGPAYContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
            if (hostelPhotos.Count() > 0)
            {
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
            }
            else
            {
                photoUrls.Add("assets\\images\\beds-hostel-affordable-housing-36997317.webp");
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

        public async Task<ResponseModel> GetHostelDetailsById(HostelFilter filter)
        {
            try
            {
                var hostelDetailsQuery = from HDetails in _context.HostelDetails.AsNoTracking()
                                         join Ratings in _context.Ratings on HDetails.HostelId equals Ratings.HostelId
                                         join Discnt in _context.Discounts on HDetails.HostelId equals Discnt.HostelId
                                         join Photos in _context.HostelPhotos on HDetails.HostelId equals Photos.HostelId into hostelPhotosGroup
                                         where HDetails.UserId == filter.UserId &&
                                               (string.IsNullOrEmpty(filter.SearchTerm) || HDetails.HostelName.Contains(filter.SearchTerm)) &&
                                               (filter.priceRange == 0 || HDetails.MinimumRent <= filter.priceRange)
                                         select new
                                         {
                                             HDetails,
                                             Ratings,
                                             Discnt,
                                             Photos = GetPhotos(hostelPhotosGroup.ToList()),
                                             HostelFacility = _context.HostelFacilities.ToList(),
                                         };

                var hostelDetails = await hostelDetailsQuery.ToListAsync();

                hostelDetails = hostelDetails
                                .Where(x => filter.minRating == 0 || (double.TryParse(x.Ratings.Starrate, out var starRate) && starRate >= filter.minRating))
                                .ToList();

                hostelDetails = filter.SortOrder switch
                {
                    "LowtoHigh" => hostelDetails.OrderBy(item => item.HDetails.MinimumRent).ToList(),
                    "HightoLow" => hostelDetails.OrderByDescending(item => item.HDetails.MinimumRent).ToList(),
                    "GuestRatings" => hostelDetails.OrderByDescending(item =>
                        double.TryParse(item.Ratings.Starrate, out var guestRating) ? guestRating : 0).ToList(),
                    _ => hostelDetails,
                };

                hostelDetails.ForEach(x =>
                {
                    int offerPercentage = int.Parse(x.Discnt.Offerper.Replace("% off", ""));
                    x.HDetails.Rent = (x.HDetails.MinimumRent * offerPercentage) / 100;
                });

                if (hostelDetails.Any())
                {
                    response.IsSuccess = true;
                    response.Content = hostelDetails;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No hostel details found.";
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = "An error occurred while fetching hostel details.";
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
                                         Amount = hd.MinimumRent,
                                         Discount = (hd.MinimumRent * int.Parse(discount.Offerper.Replace("% off", "")) / 100),
                                         Offer = discount.Offerper,
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

        public async Task<ResponseModel> AddHostelDetails(HostelDetails bookingRequest)
        {
            try
            {
                var existingHostel = await _context.HostelDetails.Where(x => x.UserId == bookingRequest.UserID && x.HostelName == bookingRequest.HostelName).FirstOrDefaultAsync();
                if (existingHostel == null)
                {
                    HostelDetail hostelDetail = new HostelDetail
                    {
                        HostelName = bookingRequest.HostelName,
                        UserId = bookingRequest.UserID,
                        HostalAddress = bookingRequest.HostalAddress,
                        NoOfRooms = bookingRequest.NoOfRooms,
                        MinimumRent = bookingRequest.MinimumRent,
                        MaximunRent = bookingRequest.MaximunRent,
                        Rent = bookingRequest.MinimumRent,
                        ContactNumber = bookingRequest.ContactNumber,
                        OwnerName = bookingRequest.OwnerName,
                        CreateBy = _configuration["CreatedBy"],
                        UpdateBy = _configuration["UpdatedBy"],
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    await _context.HostelDetails.AddAsync(hostelDetail);
                    await _context.SaveChangesAsync();

                    //if (hostelDetail.HostelId != 0)
                    //{
                    //    var user = await _context.Users.Where(x => x.UserId == bookingRequest.UserID).FirstOrDefaultAsync();
                    //    if (user != null)
                    //    {
                    //        user.HostelId = hostelDetail.HostelId;
                    //        _ = _context.Users.Update(user);
                    //        await _context.SaveChangesAsync();
                    //    }
                    //}
                    await UploadFiles(hostelDetail.HostelId, null);
                    await RatingsandDiscount(hostelDetail.HostelId, null);
                    response.IsSuccess = true;
                    response.Content = hostelDetail.HostelId;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = "Hostel already exist!!!";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }

        private async Task<bool> UploadFiles(int hostelId, List<IFormFile>? files)
        {
            try
            {
                if (files == null || files.Count == 0)
                    return false;

                string fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                Directory.CreateDirectory(fileDirectory);

                List<HostelPhoto> hostelPhotos = new List<HostelPhoto>();

                foreach (var file in files)
                {
                    var guid = Guid.NewGuid();
                    string fileExtension = Path.GetExtension(file.FileName);
                    string uniqueFileName = $"media_{DateTime.Now:yyyyMMdd_HHmmss}_{guid}{fileExtension}";
                    string filePath = Path.Combine(fileDirectory, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    string fileUrl = $"{_configuration["BaseUrl"]}/Files/{uniqueFileName}";

                    HostelPhoto hostelPhoto = new HostelPhoto
                    {
                        HostelId = hostelId,
                        Imgpath = fileUrl,
                        FileName = uniqueFileName,
                        FileSize = file.Length,
                        PhotosId = guid.ToString()
                    };
                    hostelPhotos.Add(hostelPhoto);
                }

                if (hostelPhotos.Count > 0)
                {
                    await _context.HostelPhotos.AddRangeAsync(hostelPhotos);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<bool> RatingsandDiscount(int hostelId, UserDetailsdto? UserDetails)
        {
            try
            {
                Rating rating = new Rating
                {
                    HostelId = hostelId,
                    TotalRatings = 0,
                    Status = "Good",
                    Starrate = "4.0"
                };
                await _context.Ratings.AddAsync(rating);

                Discount discount = new Discount
                {
                    HostelId = hostelId,
                    DiscountCode = null,
                    Offerper = "50% off"
                };

                await _context.Discounts.AddAsync(discount);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ResponseModel> RatingHostel(Ratingdto ratingDto)
        {
            try
            {
                var existingRating = await _context.Ratings.FirstOrDefaultAsync(x => x.HostelId == ratingDto.HostelId);

                if (existingRating != null)
                {
                    double currentStarRate = 0;
                    if (!string.IsNullOrEmpty(existingRating.Starrate) &&
                        double.TryParse(existingRating.Starrate, out double parsedStarRate))
                    {
                        currentStarRate = parsedStarRate;
                    }
                    double totalRatings = existingRating.TotalRatings ?? 0;
                    existingRating.TotalRatings += 1;
                    double newStarRate =
                        (((currentStarRate * totalRatings) + ratingDto.Starrate)
                        / existingRating.TotalRatings.Value);
                    existingRating.Starrate = newStarRate.ToString("0.0");
                    existingRating.Status = GetRatingStatus(newStarRate);
                    _context.Ratings.Update(existingRating);
                    response.Content = 1;
                    response.Message = "Rating updated successfully!";
                }
                else
                {
                    double newStarRate = ratingDto.Starrate;

                    var newRating = new Rating
                    {
                        HostelId = ratingDto.HostelId,
                        TotalRatings = 1,
                        Starrate = newStarRate.ToString("0.0"),
                        Status = GetRatingStatus(newStarRate)
                    };
                    await _context.Ratings.AddAsync(newRating);
                    response.Content = 1;
                    response.Message = "Rating added successfully!";
                }

                await _context.SaveChangesAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"An error occurred while adding the rating: {ex.Message}";
            }

            return response;
        }

        private string GetRatingStatus(double starRate)
        {
            if (starRate >= 4.5)
            {
                return "Excellent";
            }
            else if (starRate >= 3.5)
            {
                return "Good";
            }
            else if (starRate >= 2.5)
            {
                return "Average";
            }
            else
            {
                return "Poor";
            }
        }
    }
}
