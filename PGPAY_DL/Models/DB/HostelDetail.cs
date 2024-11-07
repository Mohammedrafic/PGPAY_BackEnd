using System;
using System.Collections.Generic;

namespace PGPAY_DL.Models.DB;

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

    public int? RatingId { get; set; }

    public string? DiscountCode { get; set; }

    public int? DiscountId { get; set; }

    public decimal? Rent { get; set; }

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public virtual ICollection<HostelPhoto> HostelPhotos { get; set; } = new List<HostelPhoto>();

    public virtual ICollection<HostelRequest> HostelRequests { get; set; } = new List<HostelRequest>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
