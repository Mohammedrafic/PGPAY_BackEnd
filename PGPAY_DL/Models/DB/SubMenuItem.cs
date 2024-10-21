using System;
using System.Collections.Generic;

namespace PGPAY_DL.Models.DB;

public partial class SubMenuItem
{
    public int Id { get; set; }

    public int MenuItemId { get; set; }

    public string Name { get; set; } = null!;

    public string? AdminPath { get; set; }

    public string? ImgPath { get; set; }

    public string? UserPath { get; set; }

    public virtual MenuItem MenuItem { get; set; } = null!;
}
