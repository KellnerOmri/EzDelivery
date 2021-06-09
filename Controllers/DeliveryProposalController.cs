using EzD_App.DTO;
using EzD_App.Logic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzD_App.Controllers
{
    [Route("api/DeliveryProposal")]
    [ApiController]
    [Authorize]
    public class DeliveryProposalController : ControllerBase
    {
        IDeliveryProposalManager _deliveryProposalManager;
        UserManager<IdentityUser> _userManager;

        public DeliveryProposalController(IDeliveryProposalManager deliveryProposalManager, UserManager<IdentityUser> userManager)
        {
            _deliveryProposalManager = deliveryProposalManager;
            _userManager = userManager;
        }

       
        [HttpPost]
        [Route("/api/DeliveryProposal/AddDeliveryProposal")]
        public string AddDeliveryProposal(DeliveryProposalDto deliveryProposalDto)
        {
            string userId = _userManager.GetUserId(User);
            _deliveryProposalManager.AddDeliveryProposal(userId, deliveryProposalDto);

            return JsonConvert.SerializeObject(userId);                
        }

        [HttpPost]
        [Route("/api/DeliveryProposal/getAllDeliveryProposal")]
        public string getAllDeliveryProposal()
        {
            var deliveryGuyId = _userManager.GetUserId(User);
            
            List<DeliveryProposalDto> DP = _deliveryProposalManager.getAllDeliveryProposal(deliveryGuyId);
            return JsonConvert.SerializeObject(DP);
        }


        // GET: api/<DeliveryProposalController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<DeliveryProposalController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DeliveryProposalController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DeliveryProposalController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        public void AddProposal()
        {
            return;
        }


        // DELETE api/<DeliveryProposalController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("/api/DeliveryProposal/GetDeliveryGuysByPackageId")]
        public string GetDeliveryGuysByPackageId([FromBody] string packageId)
        {
            List<DeliveryProposalDto> DeliveryProposalList = _deliveryProposalManager.GetDeliveryGuysByPackageId(packageId);
            return JsonConvert.SerializeObject(DeliveryProposalList);
        }

        [HttpDelete]
        [Route("/api/DeliveryProposal/DeleteDeliveryProposal")]
        public string DeleteDeliveryProposal([FromBody] int deliveryPropsalID)
        {
            _deliveryProposalManager.DeleteDeliveryProposal(deliveryPropsalID);
            return JsonConvert.SerializeObject("Ok");
        }
    }
}
