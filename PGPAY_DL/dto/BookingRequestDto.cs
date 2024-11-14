using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_DL.dto
{
    public class BookingRequestDto
    {
        public string RequestType { get; set; }
        public int HostelId { get; set; }
        public int UserId { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }
        public string? Response { get; set; }
        public string? ContactDetails { get; set; }
        public bool IsResolved { get; set; }
        public DateTime? LastUpdated { get; set; }
        public PaymentDto? payment { get; set; }

    }
}
