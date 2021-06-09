using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzD.Model
{
    // This model is for packages that have a chosen delivery guy by the sender
    public class ApprovedDelivery
    {
        public int DeliveryID { get; set; }
        public DeliveryGuy ChosenDeliveryGuy { get; set; }
        public Package Package { get; set; }
        public float Price { get; set; }
    }
}