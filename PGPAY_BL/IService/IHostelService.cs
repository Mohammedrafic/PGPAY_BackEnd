﻿using PGPAY_Model.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_BL.IService
{
    public interface IHostelService
    {
        Task<ResponseModel> GetAllHostelDetails();
        Task<ResponseModel> GetHostelRequestById(int UserId);
    }
}
