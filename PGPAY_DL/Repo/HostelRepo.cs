﻿using Microsoft.EntityFrameworkCore;
using PGPAY_DL.Context;
using PGPAY_DL.dto;
using PGPAY_DL.IRepo;
using PGPAY_DL.Models.DB;
using PGPAY_Model.Model.Response;
using PGPAY_Model.Model.UserDetails;

namespace PGPAY_DL.Repo
{
    public class HostelRepo : IHostelRepo
    {
        private readonly ResponseModel response = new();
        private readonly PGPAYContext _context;
        public HostelRepo(PGPAYContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel> GetAllHostelDetails()
        {
            try
            {
                var HostelDetails = await (from HDetails in _context.HostelDetails.AsNoTracking()
                                           join Ratings in _context.Ratings on HDetails.HostelId equals Ratings.HostelId
                                           join Discnt in _context.Discounts on HDetails.HostelId equals Discnt.HostelId
                                           join Photos in _context.HostelPhotos on HDetails.HostelId equals Photos.HostelId into hostelPhotosGroup
                                           select new
                                           {
                                               HostelDetails = HDetails,
                                               Ratings = Ratings,
                                               Discounts = Discnt,
                                               Photos = GetPhotos(hostelPhotosGroup.ToList()),
                                               HostelFacility = _context.HostelFacilities.ToList(),
                                           }).ToListAsync();
                HostelDetails.ForEach(x =>
                {
                    int offerPercentage = int.Parse(x.Discounts.Offerper.Replace("% off", ""));
                    x.HostelDetails.Rent = (x.HostelDetails.MinimumRent * offerPercentage) / 100;
                });
                if (HostelDetails.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.Content = HostelDetails;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Error!!!!";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
        private static List<string> GetPhotos(List<HostelPhoto> hostelPhotos)
        {
            var photoUrls = new List<string>();
            foreach (var photo in hostelPhotos)
            {
                var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", photo.FileName);

                if (System.IO.File.Exists(photoPath))
                {
                    var bytes = System.IO.File.ReadAllBytes(photoPath);
                    var base64Image = Convert.ToBase64String(bytes);
                    var imageUrl = $"data:image/jpeg;base64,{base64Image}";
                    photoUrls.Add(imageUrl);
                }
                else
                {
                    photoUrls.Add("assets\\images\\beds-hostel-affordable-housing-36997317.webp");
                }
            }
            return photoUrls;
        }

        public async Task<ResponseModel> GetHostelRequestById(int UserID)
        {
            try
            {
                var hostelIds = await _context.HostelDetails
                                           .Where(x => x.UserId == UserID)
                                           .Select(x => x.HostelId)
                                           .ToListAsync();
                var request = await (from hr in _context.HostelRequests
                                     join hd in _context.HostelDetails on hr.UserId equals hd.UserId into hdGroup
                                     from hd in hdGroup.DefaultIfEmpty()
                                     join ud in _context.UserDetails on hd.UserId equals ud.UserId into udGroup
                                     from ud in udGroup.DefaultIfEmpty()
                                     join p in _context.Payments on hd.UserId equals p.UserId into pGroup
                                     from p in pGroup.DefaultIfEmpty()
                                     where hostelIds.Contains(hr.HostelId)
                                     select new HostelRequestDto
                                     {
                                         RequestId = hr.RequestId,
                                         HostelName = hd.HostelName,
                                         RequestType = hr.RequestType,
                                         UserName = ud.Name,
                                         RequestDate = hr.RequestDate,
                                         Status = hr.Status,
                                         Description = hr.Description,
                                         AssignedTo = hr.AssignedTo,
                                         Response = hr.Response,
                                         ContactDetails = ud.PhoneNo,
                                         LastUpdated = hr.LastUpdated,
                                         Amount = p.Amount,
                                         PaymentMethod = p.PaymentMethod,
                                         TransactionId = p.TransactionId,
                                         IsAdvancedPayment = p.IsAdvancePayment,
                                         PaymentStatus = p.PaymentStatus,
                                         AdvanceAmount = p.AdvanceAmount,
                                         RemainingAmount = p.RemainingBalance,
                                         Remarks = p.Remarks,
                                         UpdatedAt = p.UpdatedAt
                                     })
                                     .GroupBy(x => x.RequestId)
                                     .Select(g => g.First())
                                     .ToListAsync();
                if (request.Count() > 0)
                {
                    response.IsSuccess = true;
                    response.Content = request;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Error!!!!";
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return response;
        }
    }
}
