using EzD_App.DTO;
using EzD_App.Logic.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzD_App.Controllers
{
    public class UserController : ControllerBase
    {
        UserManager<IdentityUser> _userManager;
        IUserManager _userMng;

        public UserController(IUserManager userMng, UserManager<IdentityUser> userManager)
        {
            _userMng = userMng;
            _userManager = userManager;
        }

        //This controller is used to get the FirstName & LastName of the user from the website.
        //We will update the data in the DB accordingly.
        // PUT api/<UserController>/GetPersonalInfo
        [HttpPut]
        [Route("/api/User/GetPersonalInfo")]
        public string GetPersonalInfo([FromBody] UserDTO user)
        {
            var userId = _userManager.GetUserId(User);
            bool flag = _userMng.UpdateUserPersonal(user, userId);
            if (flag)
                return JsonConvert.SerializeObject(flag);
            return JsonConvert.SerializeObject(flag);
        }


        // This controller will get the User information and will send it to the website.
        // Therefore, we will be able to put this data as the page loads.
        // POST api/<UserController>/LoadPersonalInfo
        [HttpPost]
        [Route("/api/User/LoadPersonalInfo")]
        public string LoadPersonalInfo()
        {
            var userId = _userManager.GetUserId(User);
            UserDTO userDto = _userMng.LoadPersonalInfo(userId);

            return JsonConvert.SerializeObject(userDto);

        }

        [HttpPut]
        [Route("/api/User/CreateDeliveryGuy")]
        public string CreateDeliveryGuy()
        {
            var userId = _userManager.GetUserId(User);
            _userMng.CreateDeliveryGuy(userId);

            return JsonConvert.SerializeObject("Ok");
        }

        [HttpDelete]
        [Route("/api/User/DeleteDeliveryGuy")]
        public string DeleteDeliveryGuy()
        {
            string userId = _userManager.GetUserId(User);
            _userMng.DeleteDeliveryGuy(userId);
            return JsonConvert.SerializeObject("Ok");
        }


        // This controller will get the Delivery Guy Status and will send it to the website.
        // Therefore, we will be able to put this data as the page loads.
        // POST api/<UserController>/LoadPersonalInfo
        [HttpPost]
        [Route("/api/User/LoadDeliveryGuyStatus")]
        public string LoadDeliveryGuyStatus()
        {
            var userId = _userManager.GetUserId(User);
            DeliveryGuyDto deliveryGuyrDto = _userMng.LoadDeliveryGuyStatus(userId);

            return JsonConvert.SerializeObject(deliveryGuyrDto);
        }


        // This controller will get the UserID and will send it to the website.
        // Therefore, we will change the state of the navbar according to the DeliveryGuy/Or not.
        // POST api/<UserController>/LoadPersonalInfo
        [HttpPost]
        [Route("/api/User/NavbarState")]
        public string NavbarState()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return JsonConvert.SerializeObject(false);

            bool navbarState= _userMng.NavbarState(userId);

            return JsonConvert.SerializeObject(navbarState);
        }

    }
}
