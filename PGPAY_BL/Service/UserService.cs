using PGPAY_BL.IService;
using PGPAY_DL.IRepo;
using PGPAY_DL.Models.DB;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;
using System;


namespace PGPAY_BL.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _repo;
        public UserService(IUserRepo repo)
        {
            _repo = repo;
        }
        public async Task<ResponseModel> AddUser(UserDetailsdto UserDetails)
        {
            return await _repo.AddUser(UserDetails);
        }

        public async Task<ResponseModel> GetLayoutData(string UserRole)
        {
            return await _repo.GetLayoutData(UserRole);
        }
    }
}
