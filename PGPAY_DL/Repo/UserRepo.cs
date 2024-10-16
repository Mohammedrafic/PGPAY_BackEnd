using Microsoft.EntityFrameworkCore;
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
        public UserRepo(PGPAYContext context)
        {
            _context = context;
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
                        CreateBy = "Rafic",
                        UpdateBy = "Rafic",
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
                        CreateBy = "Rafic",
                        UpdateBy = "Rafic",
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
                    response.Message = "Error!!!!";
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
                        HostalPhotosPath = UserDetails.HostelDetails.HostalPhotosPath,
                        CreateBy = "Rafic",
                        UpdateBy = "Rafic",
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    await _context.HostelDetails.AddAsync(hostelDetail);
                    await _context.SaveChangesAsync();

                    var hostelId = await _context.HostelDetails.Include(x => x.User).Where(x=> x.UserId == hostelDetail.UserId).Select(x => x.UserId).FirstOrDefaultAsync();
                    //if (hostelId != 0)
                    //{
                    //    HostelPhoto hostelPhoto = new HostelPhoto
                    //    {
                    //        HostelId = hostelId,
                    //        Imgpath = hostelPhoto.Imgpath,
                    //        FileName = hostelPhoto.FileName,
                    //        FileSize = hostelPhoto.FileSize
                    //    };
                    //    await _context.HostelPhotos.AddAsync(hostelPhoto);
                    //    await _context.SaveChangesAsync();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
