using System;
using System.Collections.Generic;

namespace PGPAY_DL.Models.DB;

public partial class HostelPhoto
{
    public int Id { get; set; }

    public int HostelId { get; set; }

    public string? Imgpath { get; set; }

    public string? FileName { get; set; }

    public long? FileSize { get; set; }

    public string? PhotosId { get; set; }

    public virtual HostelDetail Hostel { get; set; } = null!;
}
