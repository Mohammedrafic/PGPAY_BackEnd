using System;
using System.Collections.Generic;

namespace PGPAY_DL.Models.DB;

public partial class Rating
{
    public int Id { get; set; }

    public int HostelId { get; set; }

    public long? TotalRatings { get; set; }

    public string? Status { get; set; }

    public string? Starrate { get; set; }

    public virtual HostelDetail Hostel { get; set; } = null!;
}
