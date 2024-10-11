using System;
using System.Collections.Generic;

namespace PGPAY_DL.Context;

public partial class HostelDetail
{
    public int HostelId { get; set; }

    public int UserId { get; set; }

    public string HostelName { get; set; } = null!;

    public string HostalAddress { get; set; } = null!;

    public int NoOfRooms { get; set; }

    public decimal MinimumRent { get; set; }

    public decimal MaximunRent { get; set; }

    public long ContactNumber { get; set; }

    public string? OwnerName { get; set; }

    public string? HostalPhotosPath { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
