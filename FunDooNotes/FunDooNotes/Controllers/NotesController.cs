using BussinessLayer.Interface;
using BussinessLayer.Service;
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
    public class NotesController : ControllerBase
    {
        private readonly INotesBL inotesBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly FundooContext fundooContext;

        public NotesController(INotesBL inotesBL, IMemoryCache memoryCache, IDistributedCache distributedCache, FundooContext fundooContext)
        {
            this.inotesBL = inotesBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundooContext = fundooContext;
        }



        [HttpPost]
        [Route("Create")]
        public IActionResult CreateNotes(NotesModel notesModel)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var notes = inotesBL.CreateNotes(notesModel, userId);

                if (notes != null)
                {
                    return Ok(new { success = true, message = "Notes Created Successfully", data = notes });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed To Create Notes", });
                }


            }
            catch (Exception e)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("Get")]
        public IActionResult ReadNotes()
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = inotesBL.ReadNotes(userId);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Notes Get Successfully", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed To Get Notes"});
                }


            }
            catch (System.Exception)
            {

                throw;
            }
        }


        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteNotes(long NoteID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = inotesBL.DeleteNotes(userId, NoteID);

                if (result != false)
                {
                    return Ok(new { success = true, message = "Note Deleted"});
                }
                else
                {
                    return BadRequest(new { success = false, message = "Cannot Delete Note"});
                }


            }
            catch (System.Exception)
            {

                throw;
            }
        }



        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateNotes(NotesModel notesModel,long NoteID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = inotesBL.UpdateNote(notesModel, NoteID, userId);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Note Updated Successfully", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed To Update Note", });
                }


            }
            catch (Exception e)
            {

                throw;
            }
        }



        [HttpPut]
        [Route("Pin")]
        public IActionResult PinToTop(long NoteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = inotesBL.PinToTop(NoteID,userID);

                if (result == true)
                {
                    return Ok(new { success = true, message = "Note Pinned Successfully"});
                }
                else if (result == false)
                {
                    return Ok(new { success = true, message = "Note Unpinned Successfully"});
                }

                return BadRequest(new { success = false, message = "Operation Failed"});


            }
            catch (Exception e)
            {

                throw;
            }
        }


        [HttpPut]
        [Route("Archive")]
        public IActionResult Archive(long NoteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = inotesBL.Archive(NoteID, userID);

                if (result == true)
                {
                    return Ok(new { success = true, message = "Note Archived"});
                }
                else if (result == false)
                {
                    return Ok(new { success = true, message = "Note Unarchived"});
                }

                return BadRequest(new { success = false, message = "Operation Failed"});


            }
            catch (Exception e)
            {

                throw;
            }
        }


        [HttpPut]
        [Route("Trash")]
        public IActionResult Trash(long NoteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = inotesBL.Trash(NoteID, userID);

                if (result == true)
                {
                    return Ok(new { success = true, message = "Note Trashed" });
                }
                else if (result == false)
                {
                    return Ok(new { success = true, message = "Note Restored" });
                }

                return BadRequest(new { success = false, message = "Operation Failed" });


            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpPut]
        [Route("Color")]
        public IActionResult Color(long NoteID, string color)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = inotesBL.Color(NoteID, color);

                if (result != null)
                {
                    return Ok(new { success = true, message = "Color Changed Successfully" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Color Not Changed" });

                }
            }
            catch (Exception e)
            {

                throw;
            }
        }


        [HttpPut]
        [Route("Image")]
        public IActionResult Image(IFormFile image, long NoteID)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = inotesBL.Image(image, NoteID, userID);
                if (result != null)
                {
                    return Ok(new { success = true, message = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Failed To Upload." });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        [HttpGet("redis")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
             
            var cacheKey = "NotesList";
            string serializedNotesList;
            var NotesList = new List<NotesEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNotesList);
            }
            else
            {
                NotesList = fundooContext.NotesTable.ToList();
                serializedNotesList = JsonConvert.SerializeObject(NotesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(NotesList);
        }
    }
}
