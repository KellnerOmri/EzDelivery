using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EzD.Model;
using EzD_App.Data;
using EzD_App.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EzD_App.DataAccess
{
    public class PackageDBAccess : IPackageDBAccess
    {
        ApplicationDbContext _dbContext;
        public PackageDBAccess(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddPackage(Package package, string userId)
        {
            var user = _dbContext.User.Include(u => u.IdentityUser).Single(u => u.IdentityUser.Id.Equals(userId));
            package.Owner = user;
            package.Status = Status.Pending;
            _dbContext.Packages.Add(package);
            _dbContext.SaveChanges();
        }

        public List<Package> GetPackagesByStartingCity(string city)
        {
            //return _dbContext.Packages.Where(x => x.FromAddress.City == city).ToList();
            throw new NotImplementedException();
        }

        public List<Package> GetAllPendingPackages()
        {

            return _dbContext.Packages.Include(r => r.FromAddress).Include(r => r.ToAddress).Include(r=>r.Owner).Where(s => s.Status == Status.Pending).ToList<Package>();
        }


        public List<Package> GetAllPendingPackagesWithOutMyPackage(string userid)
        {   
            return _dbContext.Packages.Include(r => r.FromAddress).Include(r => r.ToAddress).Include(r => r.Owner.IdentityUser).Where(s => s.Status == Status.Pending && s.Owner.IdentityUser.Id != userid).ToList<Package>();
        }
         public List<Package> GetAllPackagesDeliveryGuyDid(string userid)
        {
            User deliveryGuy = _dbContext.User.Include(d=>d.DeliveryGuy).SingleOrDefault(u => u.IdentityUser.Id.Equals(userid));
            List<ApprovedDelivery> ApprovedDeliveryList = _dbContext.ApprovedDelivery.Include(p=>p.Package).Where(AD => AD.ChosenDeliveryGuy.DeliveryGuyID == deliveryGuy.DeliveryGuy.DeliveryGuyID).ToList<ApprovedDelivery>();
            List<Package> package = new List<Package>();
            foreach (ApprovedDelivery item in ApprovedDeliveryList)
            {
               var singlePackage = _dbContext.Packages.Include(r => r.FromAddress).Include(r => r.ToAddress).Include(r => r.Owner.IdentityUser)
                    .SingleOrDefault(s => s.Status == Status.Done && s.PackageID == item.Package.PackageID);
                if (singlePackage != null)
                {
                    package.Add(singlePackage);
                }           
            }
            return package;
        }

        public Package GetPackageByID(int packageID)
        {
            return _dbContext.Packages.Include(r => r.FromAddress).Include(r => r.ToAddress).SingleOrDefault(package => package.PackageID == packageID);
        }

        public void saveDbChanges()
        {
            _dbContext.SaveChanges();
        }

        public void AddProposal(Package package, string deliveryGuyId)
        {

        }

        public List<Package> GetPackagesByUserId(string userId)
        {
            var user = _dbContext.User.Include(u => u.IdentityUser).Single(u => u.IdentityUser.Id.Equals(userId));//.UserID
            return _dbContext.Packages.Include(r => r.FromAddress).Include(r => r.ToAddress).Where(x => x.Owner.UserID == user.UserID).ToList<Package>();
        }

        public void UpdatePackage(Package package, string id)
        {
            var user = _dbContext.User.Include(u => u.IdentityUser).Single(u => u.IdentityUser.Id.Equals(id));//.UserID
            package.Owner = user;
            Package packageToEdit = _dbContext.Packages.SingleOrDefault(x => x.PackageID == package.PackageID);
            packageToEdit.SenderPhone = package.SenderPhone;
            packageToEdit.ContactPhone = package.ContactPhone;
            packageToEdit.Weight = package.Weight;
            packageToEdit.Description = package.Description;
            packageToEdit.PickUpDate = package.PickUpDate;
            packageToEdit.DeadLineDate = package.DeadLineDate;
            packageToEdit.FromAddress = package.FromAddress;
            packageToEdit.ToAddress = package.ToAddress;
            packageToEdit.Price = package.Price;
            packageToEdit.SenderIsReceiver = package.SenderIsReceiver;
           
            _dbContext.SaveChanges();
        }
        
        //Get & delete all delivery proposal with the same packageID
        public void DeletePackage(int packageId)
        {
            Package p = _dbContext.Packages.Include(r => r.FromAddress).Include(r => r.ToAddress).Single(x => x.PackageID == packageId);          
            List <DeliveryProposal> DP = _dbContext.DeliveryProposals.Include(dp => dp.Package).Where(x => x.Package.PackageID == p.PackageID).ToList();
            foreach (DeliveryProposal item in DP)
            {
                _dbContext.DeliveryProposals.Remove(item);
            }
            
            _dbContext.Packages.Remove(p);
            _dbContext.SaveChanges();
        }

        public bool EndDeliveryProcess(int packageId)
        {
            Package package = _dbContext.Packages.SingleOrDefault(p => p.PackageID == packageId);

            if (package == null)
            {
                return false;
            }

            package.Status = Status.Done;
            _dbContext.SaveChanges();

            return true;
        }
    }
}
