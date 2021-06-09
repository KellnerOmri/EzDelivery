using EzD.Model;
using EzD_App.DTO;
using EzD_App.Logic.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EzD_App.Controllers
{
    [Route("api/Package")]
    [ApiController]
    [Authorize]
    public class PackageController : ControllerBase
    {
        IPackageManager _packageManager;
        UserManager<IdentityUser> _userManager;

        public PackageController(IPackageManager packageManager, UserManager<IdentityUser> userManager)
        {
            _packageManager = packageManager;
            _userManager = userManager;
        }

        // GET: api/<PackageController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PackageController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        
        // POST api/<PackageController>/4
        [HttpPost("{id}")]
        [Route("/api/Package/GetAllPendingPackages")]
        public string GetAllPendingPackages()
        {
            //Will get all the packages in the DB to show for the deliveryGuy.
            List<PackageDto> packagesList= _packageManager.GetAllPendingPackages();
            return JsonConvert.SerializeObject(packagesList);
        } 
        
        
        // POST api/<PackageController>/4
        [HttpPost("{id}")]
        [Route("/api/Package/GetAllPendingPackagesWithOutMyPackage")]
        public string GetAllPendingPackagesWithOutMyPackage()
        { 
            string userid = _userManager.GetUserId(User);
            //Will get all the packages in the DB to show for the deliveryGuy.
            List<PackageDto> packagesList= _packageManager.GetAllPendingPackagesWithOutMyPackage(userid);
            return JsonConvert.SerializeObject(packagesList);
        } 
        
        // POST api/<PackageController>
        [HttpPost("{id}")]
        [Route("/api/Package/GetAllPackagesDeliveryGuyDid")]
        public string GetAllPackagesDeliveryGuyDid()
        { 
            string userid = _userManager.GetUserId(User);
            //Will get all the packages in the DB to show for the deliveryGuy.
            List<PackageDto> packagesList= _packageManager.GetAllPackagesDeliveryGuyDid(userid);
            return JsonConvert.SerializeObject(packagesList);
        }

        





        // PUT api/<PackageController>/5
        [HttpPut("{id}")]
        public string AddPackage([FromBody] PackageDto package)
        {
            string userId = _userManager.GetUserId(User);
            _packageManager.AddPackage(package, userId);

            return JsonConvert.SerializeObject("Ok"); //Ajax waiting for a json to return back to the client
            //we will return the package ID
        }

        // PUT api/<PackageController>/ChoosePackage/5
        //[HttpPut]
        //[Route("/api/Package/ChoosePackage")]
        //public string ChoosePackage([FromBody] PackageDto package)
        //{
        //    var deliveryGuyId = _userManager.GetUserId(User);
        //    _packageManager.AddDeliveryGuyProposal(package, int.Parse(deliveryGuyId));
        //    return JsonConvert.SerializeObject("Ok"); //Ajax waiting for a json to return back to the client
        //}

        // DELETE api/<PackageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        [HttpPost]
        [Route("/api/Package/GetPackagesByUserId")]
      public string GetPackagesByUserId()
      {
          var id = _userManager.GetUserId(User);
          List<PackageDto> packagesList = _packageManager.GetPackagesByUserId(id);

            foreach (var package in packagesList)
            {
                package.PickUpDate = package.PickUpDate.Date;
            }
      
          return JsonConvert.SerializeObject(packagesList);
      }

      [HttpPut]
      [Route("/api/Package/UpdatePackage")]
      public string UpdatePackage([FromBody] PackageDto package)
      {
          string userId = _userManager.GetUserId(User);
          _packageManager.UpdatePackage(package, userId);
           return JsonConvert.SerializeObject("Ok");
       }

        [HttpDelete]
        [Route("/api/Package/DeletePackage/{PackageId}")]
        public string DeletePackage(int PackageId)
        {
            string userId = _userManager.GetUserId(User);
            _packageManager.DeletePackage(PackageId);
            return JsonConvert.SerializeObject("Ok");
        }

        [HttpPost]
        [Route("/api/Package/EndDeliveryProcess")]
        public string EndDeliveryProcess([FromBody]int packageId)
        {
            return JsonConvert.SerializeObject(_packageManager.EndDeliveryProcess(packageId));
        }
    }
}