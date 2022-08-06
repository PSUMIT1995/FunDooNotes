using BussinessLayer.Interface;
using CommonLayer.Modal;
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
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL ilabelBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly FundooContext fundooContext;

        public LabelController(ILabelBL ilabelBL, IMemoryCache memoryCache, IDistributedCache distributedCache, FundooContext fundooContext)
        {
            this.ilabelBL = ilabelBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundooContext = fundooContext;
        }


        [HttpPost]
        [Route("Add")]
        public IActionResult AddLabel(LabelModel label)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = ilabelBL.AddLabel(label, userId);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Label Added Successfully", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed To Add Label", });
                }


            }
            catch (Exception e)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("Get")]
        public IActionResult GetLabel(long LabelID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = ilabelBL.GetLabel(LabelID, userId);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Get Successfully", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed To Get Label", });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        [HttpPut]
        [Route("Rename")]
        public IActionResult RenameLabel(long LabelID, string newLabelName)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = ilabelBL.RenameLabel(LabelID, newLabelName, userId);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Label Edited Successfully", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed To Edit", });
                }


            }
            catch (Exception e)
            {

                throw;
            }

        }



        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteLabel(long LabelID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = ilabelBL.DeleteLabel(LabelID);

                if (result != null )
                {
                    return Ok(new { success = true, message = "Label Deleted" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cannot Delete Label" });
                }


            }
            catch (System.Exception)
            {

                throw;
            }
        }



        [HttpGet("redis")]
        public async Task<IActionResult> GetAllLabelUsingRedisCache()
        {
            long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);

            var cacheKey = "LabelList";
            string serializedLabelList;
            var LabelList = new List<LabelEntity>();
            var redisLabelList = await distributedCache.GetAsync(cacheKey);
            if (redisLabelList != null)
            {
                serializedLabelList = Encoding.UTF8.GetString(redisLabelList);
                LabelList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelList);
            }
            else
            {
                LabelList = fundooContext.LabelTable.ToList();
                serializedLabelList = JsonConvert.SerializeObject(LabelList);
                redisLabelList = Encoding.UTF8.GetBytes(serializedLabelList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisLabelList, options);
            }
            return Ok(LabelList);
        }

    }
}
