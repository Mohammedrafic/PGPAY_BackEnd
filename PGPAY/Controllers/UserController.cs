﻿using Microsoft.AspNetCore.Mvc;
using PGPAY_BL.IService;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;

namespace PGPAY.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService service;
        public UserController(IUserService service)
        {
            this.service = service;
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromForm]UserDetailsdto UserDetails)
        {
            ResponseModel response = await service.AddUser(UserDetails);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetLayoutData")]
        public async Task<IActionResult> GetLayoutData(string UserRole)
        {
            ResponseModel response = await service.GetLayoutData(UserRole);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
