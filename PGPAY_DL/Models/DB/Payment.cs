using System;
using System.Collections.Generic;

namespace PGPAY_DL.Models.DB;

public partial class Payment
{
    public int PaymentId { get; set; }

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

    public virtual HostelDetail Hostel { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
