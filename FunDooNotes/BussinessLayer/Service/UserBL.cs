using BussinessLayer.Interface;
using CommonLayer.Modal;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL iuserRL;

        public UserBL(IUserRL iuserRL)
        {
            this.iuserRL = iuserRL;
        }
        
        public UserEntity Registration(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                return iuserRL.Registration(userRegistrationModel);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string Login(UserLogin userLoginModel)
        {
            try
            {
                return iuserRL.Login(userLoginModel);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public string ForgetPassword(string Email)
        {
            try
            {
                return iuserRL.ForgetPassword(Email);
            }
            catch (Exception e)
            {

                throw;
            }
        }


        public bool ResetLink(string email, string password, string confirmPassword)
        {
            try
            {
                return iuserRL.ResetLink(email, password, confirmPassword);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
