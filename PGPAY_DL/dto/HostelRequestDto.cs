﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGPAY_DL.dto
{
    public class HostelRequestDto
    {
        public int UserID { get; set; }
        public int? RequestId { get; set; }  
        public string? HostelName { get; set; } 
        public string? RequestType { get; set; } 
        public string? UserName { get; set; } 
        public DateTime? RequestDate { get; set; }  
        public string? Status { get; set; } 
        public string? Description { get; set; }  
        public string? AssignedTo { get; set; } 
        public string? Response { get; set; } 
        public long? ContactDetails { get; set; } 
        public DateTime? LastUpdated { get; set; } 
        public decimal? Amount { get; set; } 
        public string? PaymentMethod { get; set; } 
        public string? TransactionId { get; set; } 
        public bool? IsAdvancedPayment { get; set; }  
        public string? PaymentStatus { get; set; } 
        public decimal? AdvanceAmount { get; set; } 
        public decimal? RemainingAmount { get; set; } 
        public string? Remarks { get; set; } 
        public DateTime? UpdatedAt { get; set; }
        public int? HostelId { get; set; }
    }

    public class HostelFilter
    {
        public int UserId { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortOrder { get; set;}
        public int priceRange { get; set; }
        public int minRating { get; set; }
    }
}
