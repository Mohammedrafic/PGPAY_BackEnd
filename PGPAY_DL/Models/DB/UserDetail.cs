using System;
using System.Collections.Generic;

namespace PGPAY_DL.Context;

public partial class UserDetail
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public int? Age { get; set; }

    public long PhoneNo { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? MaritalStatus { get; set; }

    public string? State { get; set; }

    public string? Address { get; set; }

    public long? AltPhoneNo { get; set; }

    public string? ProofPath { get; set; }

    public string? Sex { get; set; }

    public string? CreateBy { get; set; }

    public string? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
