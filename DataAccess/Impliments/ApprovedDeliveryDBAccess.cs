using EzD.Model;
using EzD_App.Data;
using EzD_App.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzD_App.DataAccess.Impliments
{
    public class ApprovedDeliveryDBAccess : IApprovedDeliveryDBAccess
    {
        ApplicationDbContext _dbContext;
        public ApprovedDeliveryDBAccess(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<ApprovedDelivery> GetApprovedDeliveries(string userId)
        {
            int deliveryGuyID = _dbContext.User.Include(u => u.IdentityUser).Include(u=> u.DeliveryGuy)
                .SingleOrDefault(u => u.IdentityUser.Id.Equals(userId)).DeliveryGuy.DeliveryGuyID;

            return _dbContext.ApprovedDelivery.Include(ad => ad.ChosenDeliveryGuy).Include(ad => ad.Package).
                Include(ad => ad.Package.FromAddress).Include(ad => ad.Package.ToAddress).
                Where(ad => ad.ChosenDeliveryGuy.DeliveryGuyID == deliveryGuyID && ad.Package.Status == Status.InProgress).ToList();
        }

        public bool ApproveDeliveryProposal(ApprovedDelivery approvedDelivery)
        {
           Package package = _dbContext.Packages.Include(p => p.Owner).Include(p => p.FromAddress).
                Include(p => p.ToAddress).SingleOrDefault(p => p.PackageID == approvedDelivery.Package.PackageID);
           DeliveryGuy deliveryGuy = _dbContext.DeliveryGuy.
               SingleOrDefault(d => d.DeliveryGuyID == approvedDelivery.ChosenDeliveryGuy.DeliveryGuyID);

            if (package == null || deliveryGuy == null)
            {
                return false;
            }

            package.Status = Status.InProgress;

            approvedDelivery.Package = package;
            approvedDelivery.ChosenDeliveryGuy = deliveryGuy;

            _dbContext.ApprovedDelivery.Add(approvedDelivery);
            _dbContext.SaveChanges();

            return true;
        }

    }
}
