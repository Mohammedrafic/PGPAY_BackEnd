using System;
using System.Collections.Generic;

namespace PGPAY_DL.Models.DB;

public partial class MenuItem
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsOpen { get; set; }

    public string? AdminPath { get; set; }

    public string? ImgPath { get; set; }

    public string? UserPath { get; set; }

    public bool? IsAdmin { get; set; }

    public bool? IsUser { get; set; }

    public virtual ICollection<SubMenuItem> SubMenuItems { get; set; } = new List<SubMenuItem>();
}
