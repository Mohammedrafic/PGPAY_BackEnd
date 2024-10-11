using Microsoft.EntityFrameworkCore;
using PGPAY_DL.Context;
using PGPAY_DL.IRepo;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                        UserRole = "Admin",
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

                    HostelDetail hostelDetail = new HostelDetail
                    {
                        HostelName = UserDetails.HostelName,
                        UserId = NewUser.UserId,
                        HostalAddress = UserDetails.HostalAddress,
                        NoOfRooms = UserDetails.NoOfRooms,
                        MinimumRent = UserDetails.MinimumRent,
                        MaximunRent = UserDetails.MaximunRent,
                        ContactNumber = UserDetails.ContactNumber,
                        OwnerName = UserDetails.OwnerName,
                        HostalPhotosPath = UserDetails.HostalPhotosPath,
                        CreateBy = "Rafic",
                        UpdateBy = "Rafic",
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    await _context.HostelDetails.AddAsync(hostelDetail);
                    await _context.SaveChangesAsync();
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
    }
}
