using System;
using System.Collections.Generic;

namespace PGPAY_DL.Models.DB;

public partial class Discount
{
    public int Id { get; set; }

    public int HostelId { get; set; }

    public string? DiscountCode { get; set; }

    public string? Offerper { get; set; }

    public virtual HostelDetail Hostel { get; set; } = null!;
}
