using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_BL.Hub.IHub
{
    public interface IMessageHubClient
    {
        Task SendOffersToUser(List<string> message);
    }
}
