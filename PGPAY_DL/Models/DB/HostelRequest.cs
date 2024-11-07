using System;
using System.Collections.Generic;

namespace PGPAY_DL.Models.DB;

public partial class HostelRequest
{
    public int RequestId { get; set; }

    public string RequestType { get; set; } = null!;

    public int HostelId { get; set; }

    public int UserId { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public string? AssignedTo { get; set; }

    public string? Response { get; set; }

    public string? ContactDetails { get; set; }

    public bool? IsResolved { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual HostelDetail Hostel { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
