using Microsoft.AspNetCore.SignalR;
using PGPAY_BL.Hub.IHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_BL.Hub.HubClass
{
    public class MessageHub: Hub<IMessageHubClient>
    {
        public async Task SendOffersToUser(List<string> message)
        {
            await Clients.All.SendOffersToUser(message);
        }
    }
}
