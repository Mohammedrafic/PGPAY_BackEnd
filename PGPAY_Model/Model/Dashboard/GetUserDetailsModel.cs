using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_Model.Model.Dashboard
{
    public class GetUserDetailsModel
    {
        public int HostelId { get; set; }
        public string HostelName { get; set; } = null!;
        public string HostelAddress { get; set; }
        public int NoofRooms { get; set; }
        public decimal? Rent { get; set; }
        public string? DiscountPer { get; set; }
        public decimal? MinimunmRent { get; set; }
    }
}
