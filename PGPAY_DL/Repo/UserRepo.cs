using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PGPAY_DL.Context;
using PGPAY_DL.IRepo;
using PGPAY_DL.Models.DB;
using PGPAY_Model.Enums;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;

namespace PGPAY_DL.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly ResponseModel response = new();
        private readonly PGPAYContext _context;
        private readonly IConfiguration _configuration;
        public UserRepo(PGPAYContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ResponseModel> AddUser(UserDetailsdto UserDetails)
        {
            try
            {
                var ExistingUser = await _context.Users.Where(x => x.UserName == UserDetails.UserName || x.Email == UserDetails.Email).ToListAsync();
                if (!ExistingUser.Any())
                {
                    User user = new User
                    {
                        UserName = UserDetails.UserName,
                        Email = UserDetails.Email,
                        Password = UserDetails.Password,
                        UserRole = UserDetails.UserRole,
                        CreateBy = _configuration["CreatedBy"],
                        UpdateBy = _configuration["UpdatedBy"],
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    var NewUser = await _context.Users.Where(x => x.Email == UserDetails.Email && x.Password == UserDetails.Password).FirstOrDefaultAsync();

                    UserDetail userDetail = new UserDetail
                    {
                        Name = UserDetails.Name,
                        UserId = NewUser.UserId,
                        Age = UserDetails.Age,
                        PhoneNo = UserDetails.PhoneNo,
                        AltPhoneNo = UserDetails.AltPhoneNo,
                        DateOfBirth = UserDetails.DateOfBirth,
                        MaritalStatus = UserDetails.MaritalStatus,
                        State = UserDetails.State,
                        Address = UserDetails.Address,
                        ProofPath = UserDetails.ProofPath,
                        Sex = UserDetails.Sex,
                        CreateBy = _configuration["CreatedBy"],
                        UpdateBy = _configuration["UpdatedBy"],
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    await _context.UserDetails.AddAsync(userDetail);
                    await _context.SaveChangesAsync();

                    await AddHostelDetails(NewUser, UserDetails);

                    response.IsSuccess = true;
                    response.Content = 1;
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

        public async Task<ResponseModel> GetLayoutData(string UserRole)
        {
            try
            {
                var menuItems = await _context.MenuItems
                                   .Include(m => m.SubMenuItems)
                                   .Select(x => new
                                   {
                                       x.Name,
                                       x.IsOpen,
                                       Path = UserRole == "Admin" ? x.AdminPath : x.UserPath,
                                       x.ImgPath,
                                       SubMenuItems = x.SubMenuItems.Select(s => new
                                       {
                                           s.Name,
                                           s.AdminPath,
                                           s.ImgPath,
                                           Path = UserRole == "Admin" ? s.AdminPath : s.UserPath,
                                           s.UserPath
                                       }).ToList()
                                   })
                                   .ToListAsync();
                if (menuItems.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.Content = menuItems;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No Data Found!!";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }

        private async Task AddHostelDetails(User NewUser, UserDetailsdto UserDetails)
        {
            try
            {
                if (NewUser.UserRole == CommonEnum.Admin.ToString())
                {
                    HostelDetail hostelDetail = new HostelDetail
                    {
                        HostelName = UserDetails.HostelDetails.HostelName,
                        UserId = NewUser.UserId,
                        HostalAddress = UserDetails.HostelDetails.HostalAddress,
                        NoOfRooms = UserDetails.HostelDetails.NoOfRooms,
                        MinimumRent = UserDetails.HostelDetails.MinimumRent,
                        MaximunRent = UserDetails.HostelDetails.MaximunRent,
                        Rent = UserDetails.HostelDetails.MinimumRent,
                        ContactNumber = UserDetails.HostelDetails.ContactNumber,
                        OwnerName = UserDetails.HostelDetails.OwnerName,
                        CreateBy = _configuration["CreatedBy"],
                        UpdateBy = _configuration["UpdatedBy"],
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    await _context.HostelDetails.AddAsync(hostelDetail);
                    await _context.SaveChangesAsync();

                    var hostelId = await _context.HostelDetails.Include(x => x.User).Where(x => x.UserId == hostelDetail.UserId).Select(x => x.HostelId).FirstOrDefaultAsync();
                    if (hostelId != 0)
                    {
                        NewUser.HostelId = hostelId;
                        _= _context.Users.Update(NewUser);
                        await _context.SaveChangesAsync();
                        await UploadFiles(hostelId, null); //UserDetails.HostelDetails.HostalPhotosPath);
                    }
                    await RatingsandDiscount(hostelId, UserDetails);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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
        private async Task<bool> RatingsandDiscount(int hostelId, UserDetailsdto UserDetails)
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
    }
}
