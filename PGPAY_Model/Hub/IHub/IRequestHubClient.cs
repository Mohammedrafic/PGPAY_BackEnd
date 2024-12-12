using PGPAY_Model.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_BL.Hub.IHub
{
    public interface IRequestHubClient
    {
        Task SendRequestToUser(object respnse);
    }
}
