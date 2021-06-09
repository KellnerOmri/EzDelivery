using EzD.Model;
using EzD_App.Data;
using EzD_App.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzD_App.DataAccess.Impliments
{
    public class DeliveryGuyDBAccess : IDeliveryGuyDBAccess
    {
        ApplicationDbContext _dbContext;
        public DeliveryGuyDBAccess(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DeliveryGuy GetDeliveryGuyByID(int deliveryGuyId)
        {
            return _dbContext.User.SingleOrDefault(deliveryGuy => deliveryGuy.UserID == deliveryGuyId).DeliveryGuy;
        }
    }

    

    
}
