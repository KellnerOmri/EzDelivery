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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EzD_App.Controllers
{
    [Route("api/ApprovedDelivery")]
    [ApiController]
    [Authorize]
    public class ApprovedDeliveryController : ControllerBase
    {
        IApprovedDeliveryManager _ApprovedDeliveryManager;
        UserManager<IdentityUser> _userManager;

        public ApprovedDeliveryController(IApprovedDeliveryManager ApprovedDeliveryManager, UserManager<IdentityUser> userManager)
        {
            _ApprovedDeliveryManager = ApprovedDeliveryManager;
            _userManager = userManager;
        }

        // GET: api/<ApprovedDeliveryController>
        [HttpGet]
        [Route("/api/ApprovedDelivery/GetApprovedDeliveries")]
        public string GetApprovedDeliveries()
        {
            string userId = _userManager.GetUserId(User);
            return JsonConvert.SerializeObject(_ApprovedDeliveryManager.GetApprovedDeliveries(userId));
        }

        [HttpPost]
        [Route("/api/ApprovedDelivery/ApproveDeliveryProposal")]
        public string ApproveDeliveryProposal(DeliveryProposalDto deliveryProposalDto)
        {
            return JsonConvert.SerializeObject(_ApprovedDeliveryManager.ApproveDeliveryProposal(deliveryProposalDto));
        }

        // GET api/<ApprovedDeliveryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ApprovedDeliveryController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ApprovedDeliveryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ApprovedDeliveryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
