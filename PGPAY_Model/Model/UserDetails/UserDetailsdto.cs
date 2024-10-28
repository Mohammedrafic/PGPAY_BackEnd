using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_Model.Model.UserDetails
{
    public class UserDetailsdto
    {
        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string UserRole { get; set; } = null!;

        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

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

        public HostelDetails? HostelDetails { get; set; }
    }

    public class HostelDetails
    {
        public string HostelName { get; set; } = null!;

        public string HostalAddress { get; set; } = null!;

        public int NoOfRooms { get; set; }

        public decimal MinimumRent { get; set; }

        public decimal MaximunRent { get; set; }

        public long ContactNumber { get; set; }

        public string? OwnerName { get; set; }

        public List<IFormFile> HostalPhotosPath { get; set; } = new List<IFormFile>();
    }

}
