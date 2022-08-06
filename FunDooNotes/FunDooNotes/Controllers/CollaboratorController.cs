using BussinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBL icollaboratorBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly FundooContext fundooContext;

        public CollaboratorController(ICollaboratorBL icollaboratorBL, IMemoryCache memoryCache, IDistributedCache distributedCache, FundooContext fundooContext)
        {
            this.icollaboratorBL = icollaboratorBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundooContext = fundooContext;
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult AddCollab(long noteId, string email)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = icollaboratorBL.AddCollab(noteId, userId, email);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Added Successfully", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed To Add", });
                }


            }
            catch (Exception e)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("Get")]
        public IEnumerable<CollaboratorEntity> GetCollaboratorByID(long CollaboratorID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                return icollaboratorBL.GetCollaboratorByID(CollaboratorID, userId);

            }
            catch (System.Exception)
            {

                throw;
            }
        }



        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteCollaborator(long CollaboratorID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = icollaboratorBL.DeleteCollaborator(CollaboratorID);

                if (result != false)
                {
                    return Ok(new { success = true, message = "Deleted Successfully" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cannot Delete" });
                }


            }
            catch (System.Exception)
            {

                throw;
            }
        }



        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCollaboratorUsingRedisCache()
        {
            long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);

            var cacheKey = "CollaboratorList";
            string serializedCollaboratorList;
            var CollaboratorList = new List<CollaboratorEntity>();
            var redisCollaboratorList = await distributedCache.GetAsync(cacheKey);
            if (redisCollaboratorList != null)
            {
                serializedCollaboratorList = Encoding.UTF8.GetString(redisCollaboratorList);
                CollaboratorList = JsonConvert.DeserializeObject<List<CollaboratorEntity>>(serializedCollaboratorList);
            }
            else
            {
                CollaboratorList = fundooContext.CollaboratorTable.ToList();
                serializedCollaboratorList = JsonConvert.SerializeObject(CollaboratorList);
                redisCollaboratorList = Encoding.UTF8.GetBytes(serializedCollaboratorList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCollaboratorList, options);
            }
            return Ok(CollaboratorList);
        }
    }
}
