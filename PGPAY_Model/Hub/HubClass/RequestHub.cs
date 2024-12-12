using Microsoft.AspNetCore.SignalR;
using PGPAY_BL.Hub.IHub;
using PGPAY_Model.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_BL.Hub.HubClass
{
    public class RequestHub: Hub<IRequestHubClient>
    {
        public async Task SendrequestToUser(object response)
        {
            await Clients.All.SendRequestToUser(response);
        }
    }
}
