using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_DL.dto
{
    public class PaymentDto
    {
        public int HostelId { get; set; }

        public int UserId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = null!;

        public string? TransactionId { get; set; }

        public string PaymentStatus { get; set; } = null!;

        public bool? IsAdvancePayment { get; set; }

        public decimal? AdvanceAmount { get; set; }

        public decimal? RemainingBalance { get; set; }

        public string? Remarks { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
