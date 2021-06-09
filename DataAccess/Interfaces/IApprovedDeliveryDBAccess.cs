using EzD.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzD_App.DataAccess.Interfaces
{
    public interface IApprovedDeliveryDBAccess
    {
        public List<ApprovedDelivery> GetApprovedDeliveries(string userId);
        public bool ApproveDeliveryProposal(ApprovedDelivery approvedDelivery);
    }
}
