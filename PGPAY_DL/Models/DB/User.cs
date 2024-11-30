using System;
using System.Collections.Generic;

namespace PGPAY_DL.Models.DB;

public partial class User
{
    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int UserId { get; set; }

    public int? HostelId { get; set; }

    public Guid? UniqueKey { get; set; }

    public virtual HostelDetail? Hostel { get; set; }

    public virtual ICollection<HostelDetail> HostelDetails { get; set; } = new List<HostelDetail>();

    public virtual ICollection<HostelRequest> HostelRequests { get; set; } = new List<HostelRequest>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual UserDetail? UserDetail { get; set; }
}
