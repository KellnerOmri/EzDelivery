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
    public class DeliveryProposalDBAccess : IDeliveryProposalDBAccess
    {
        ApplicationDbContext _dbContext;
        public DeliveryProposalDBAccess(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddDeliveryProposal(string userID, DeliveryProposal deliveryProposal,int packageID)
        {
            DeliveryGuy deliveryGuy = _dbContext.User.Include(u => u.IdentityUser).Include(u => u.DeliveryGuy).
                SingleOrDefault(u => u.IdentityUser.Id.Equals(userID)).DeliveryGuy;
            
            deliveryProposal.IntrestedDeliveryGuy = deliveryGuy;
         
            Package package = _dbContext.Packages.Include(r => r.FromAddress).Include(r => r.ToAddress).Include(r => r.Owner).
                SingleOrDefault(package => package.PackageID == packageID);    
            
            deliveryProposal.Package = package;

            _dbContext.DeliveryProposals.Add(deliveryProposal);
            _dbContext.SaveChanges();           
        }

        public List<DeliveryProposal> GetProposalsByPackageId(string packageId)
        {
            int id = Int32.Parse(packageId);

            return _dbContext.DeliveryProposals.Include(r => r.IntrestedDeliveryGuy).Include(r => r.Package).
                Include(a => a.Package.FromAddress).Include(a => a.Package.ToAddress).
                Include(dp => dp.Package.Owner).Where(s => s.Package.PackageID == id).ToList();
        }

        public Dictionary<int, User> GetUserDictionary(List<DeliveryProposal> deliveryProposals)
        {
            Dictionary<int, User> userDictionary = new Dictionary<int, User>();

            foreach (DeliveryProposal d in deliveryProposals)
            {
                User u = _dbContext.User.Include(r => r.DeliveryGuy).Include(r => r.IdentityUser).
                    SingleOrDefault(x => x.DeliveryGuy.DeliveryGuyID == d.IntrestedDeliveryGuy.DeliveryGuyID);

                if (u != null && u.DeliveryGuy.Active)
                {
                    userDictionary.Add(u.DeliveryGuy.DeliveryGuyID, u);
                }
            }

            return userDictionary;
        }
        public void DeleteDeliveryProposal(int deliveryProposalId)
        {
            DeliveryProposal DP = new DeliveryProposal();
             DP = _dbContext.DeliveryProposals.Include(r => r.IntrestedDeliveryGuy).Include(r => r.Package).
                Include(a => a.Package.FromAddress).Include(a => a.Package.ToAddress).SingleOrDefault(dp => dp.ProposalID.Equals(deliveryProposalId));
            if (DP == null)
                return;
            _dbContext.DeliveryProposals.Remove(DP);         
            _dbContext.SaveChanges();
        }

        //return all proposal of identity deliveryGuy(that conecting to the system right now)
        public List<DeliveryProposal> getAllDeliveryProposal(string IdentityDelivery)
        {
            int deliveryID = _dbContext.User.Include(u => u.IdentityUser).Include(x => x.DeliveryGuy)
                .SingleOrDefault(d => d.IdentityUser.Id.Equals(IdentityDelivery)).DeliveryGuy.DeliveryGuyID;

            return _dbContext.DeliveryProposals.Include(r => r.IntrestedDeliveryGuy).Include(r => r.Package).
                Include(a =>a.Package.FromAddress).Include(a => a.Package.ToAddress)
                .Where(s => s.IntrestedDeliveryGuy.DeliveryGuyID == deliveryID && s.Package.Status == Status.Pending).ToList();
        }
    }
}
