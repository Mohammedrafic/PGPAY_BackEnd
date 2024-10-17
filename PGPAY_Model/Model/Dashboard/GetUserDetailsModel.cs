using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_Model.Model.Dashboard
{
    public class GetUserDetailsModel
    {
        public int UserId { get; set; }

        public string Name { get; set; } = null!;

        public int? Age { get; set; }

        public long PhoneNo { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? MaritalStatus { get; set; }

        public string? State { get; set; }

        public string? Address { get; set; }

        public long? AltPhoneNo { get; set; }

        public string? Sex { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
