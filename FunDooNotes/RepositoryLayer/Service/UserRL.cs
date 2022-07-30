using CommonLayer.Modal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Azure.Management.ContainerInstance.Fluent;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly IConfiguration iconfiguration;
        private readonly FundooContext fundooContext;

        public UserRL(FundooContext fundooContext,IConfiguration iconfiguration)
        {
            this.fundooContext = fundooContext;
            this.iconfiguration = iconfiguration;
        }

        public UserEntity Registration(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = userRegistrationModel.FirstName;
                userEntity.LastName = userRegistrationModel.LastName;
                userEntity.Email = userRegistrationModel.Email;
                userEntity.Password = userRegistrationModel.Password;

                fundooContext.UserTable.Add(userEntity);
                int result = fundooContext.SaveChanges();

                if (result!=0)
                {
                    return userEntity;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public string Login(UserLogin userLoginModel)
        {
            try
            {
                var LoginResult = fundooContext.UserTable.Where(user => user.Email== userLoginModel.Email && user.Password == userLoginModel.Password).FirstOrDefault();

                if (LoginResult != null)
                {
                    var token = GenerateSecurityToken(LoginResult.Email, LoginResult.UserId);

                    return token;

                }

                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        public string GenerateSecurityToken(string email,long userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.iconfiguration[("JWT:key")]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("userID", userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        public string ForgetPassword(string Email)
        {
            try
            {
                var emailCheck = fundooContext.UserTable.FirstOrDefault(x => x.Email == Email);

                if (emailCheck != null)
                {
                    var Token = GenerateSecurityToken(emailCheck.Email, emailCheck.UserId);
                    MSMQmodel mSMQmodel = new MSMQmodel();
                    mSMQmodel.sendData2Queue(Token);
                    return Token.ToString();

                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

       
    }
}
