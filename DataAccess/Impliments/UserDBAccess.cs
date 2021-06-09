using EzD.Model;
using EzD_App.Data;
using EzD_App.DataAccess.Interfaces;
using EzD_App.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EzD_App.DataAccess.Impliments
{
    public class UserDBAccess : IUserDBAccess
    {
        ApplicationDbContext _dbContext;

        public UserDBAccess(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddIdentityUser(User user)
        {
            _dbContext.User.Add(user);
            _dbContext.SaveChanges();
        }

        public bool UpdateUserPersonal(User user, string userId)
        {
            try
            {
                var userDb = _dbContext.User.Include(u => u.IdentityUser).Single(u => u.IdentityUser.Id.Equals(userId));
                userDb.FirstName = user.FirstName;
                userDb.LastName = user.LastName;
                _dbContext.SaveChanges();
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }

        public UserDTO LoadPersonalInfo(string userId)
        {
            var userDb = _dbContext.User.Include(u => u.IdentityUser).Single(u => u.IdentityUser.Id.Equals(userId));
            UserDTO userDto = new UserDTO();
            userDto.FirstName = userDb.FirstName;
            userDto.LastName = userDb.LastName;

            return userDto;
        }

        public void CreateDeliveryGuy(string userId)
        {
            var userDb = _dbContext.User.Include(u => u.IdentityUser).Include(x => x.DeliveryGuy).Single(u => u.IdentityUser.Id.Equals(userId));
            if(userDb.DeliveryGuy == null)//in case the DeliveryGuy was defined, the object doesn't been saved in the database but the DeliveryGuyId.
            {
                userDb.DeliveryGuy = new DeliveryGuy();
                userDb.DeliveryGuy.Active = true;
                _dbContext.SaveChanges();
            }
            else
            {
                userDb.DeliveryGuy.Active = true;
                _dbContext.SaveChanges();
            }
        }

        public void DeleteDeliveryGuy(string userId)
        {
            var userDb = _dbContext.User.Include(u => u.IdentityUser).Include(x => x.DeliveryGuy).Single(u => u.IdentityUser.Id.Equals(userId));
            if (userDb.DeliveryGuy != null)//in case the DeliveryGuy was defined, the object doesn't been saved in the database but the DeliveryGuyId.
            {
                userDb.DeliveryGuy.Active = false;
                _dbContext.SaveChanges();
            }
        }


        public DeliveryGuy LoadDeliveryGuyStatus(string userId)
        {
            var userDb = _dbContext.User.Include(u => u.IdentityUser).Include(d => d.DeliveryGuy).Single(u => u.IdentityUser.Id.Equals(userId));

            return userDb.DeliveryGuy;
        }


        public bool NavbarState(string userId)
        {
            var userDb = _dbContext.User.Include(u => u.IdentityUser).Include(d => d.DeliveryGuy).Single(u => u.IdentityUser.Id.Equals(userId));
            return userDb.DeliveryGuy != null ? userDb.DeliveryGuy.Active : false;
        }

    }
}
