using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Modal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class NotesRL : INotesRL
    {

        private readonly FundooContext fundooContext;
        private readonly IConfiguration cloudinaryEntity;

        public NotesRL(FundooContext fundooContext, IConfiguration cloudinaryEntity)
        {
            this.fundooContext = fundooContext;
            this.cloudinaryEntity = cloudinaryEntity;
        }

        public NotesEntity CreateNotes(NotesModel notesModel, long userId)
        {
            try
            {
                var ValidateUser = fundooContext.UserTable.Where(e => e.UserId == userId);
                if (ValidateUser != null)
                {
                    NotesEntity notesEntity = new NotesEntity();
                    notesEntity.Title = notesModel.Title;
                    notesEntity.Description = notesModel.Description;
                    notesEntity.Reminder = notesModel.Reminder;
                    notesEntity.Color = notesModel.Color;
                    notesEntity.Image = notesModel.Image;
                    notesEntity.Archive = notesModel.Archive;
                    notesEntity.Pin = notesModel.Pin;
                    notesEntity.Trash = notesModel.Trash;
                    notesEntity.Created = notesModel.Created;
                    notesEntity.Edited = notesModel.Edited;
                    notesEntity.UserId = userId;

                    fundooContext.NotesTable.Add(notesEntity);
                    int result = fundooContext.SaveChanges();
                    return notesEntity;
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

        public IEnumerable<NotesEntity> ReadNotes(long userId)
        {
            try
            {
                var result = fundooContext.NotesTable.Where(id => id.UserId == userId);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool DeleteNotes(long userId, long noteId)
        {
            try
            {

                var result = fundooContext.NotesTable.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();
                if (result != null)
                {
                    fundooContext.NotesTable.Remove(result);
                    this.fundooContext.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }


            }
            catch (Exception)
            {

                throw;
            }
        }



        public NotesEntity UpdateNote(NotesModel notesModel, long noteId, long userId)
        {
            try
            {
                var result = fundooContext.NotesTable.Where(note => note.UserId == userId && note.NoteID == noteId).FirstOrDefault();
                if (result != null )
                {
                    result.Title = notesModel.Title;
                    result.Description = notesModel.Description;
                    result.Reminder = notesModel.Reminder;
                    result.Edited = DateTime.Now;
                    result.Color = notesModel.Color;
                    result.Image = notesModel.Image;

                    this.fundooContext.SaveChanges();
                    return result;
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


        public bool PinToTop(long noteId, long userId)
        {
            try
            {

                var result = fundooContext.NotesTable.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();
                if (result.Pin == true)
                {
                    result.Pin = false;
                    fundooContext.SaveChanges();
                    return false;
                }
                else
                {
                    result.Pin = true;
                    fundooContext.SaveChanges();
                    return true;

                }

            }
            catch (Exception)
            {

                throw;
            }
        }



        public bool Archive(long noteId, long userId)
        {
            try
            {

                var result = fundooContext.NotesTable.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();
                if (result.Archive == false)
                {
                    result.Archive = true;
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    result.Archive = false;
                    fundooContext.SaveChanges();
                    return false;

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Trash(long noteId, long userId)
        {
            try
            {

                var result = fundooContext.NotesTable.Where(x => x.UserId == userId && x.NoteID == noteId).FirstOrDefault();
                if (result.Trash == false)
                {
                    result.Trash = true;
                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    result.Trash = false;
                    fundooContext.SaveChanges();
                    return false;

                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity Color(long noteId, string color)
        {
            try
            {
                var result = fundooContext.NotesTable.First(x => x.NoteID == noteId);
                if (result != null)
                {
                    result.Color = color;
                    fundooContext.SaveChanges();
                    return result;
                    
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



        public string Image(IFormFile image, long NoteID, long userID)
        {
            try
            {
                var result = fundooContext.NotesTable.Where(x => x.UserId == userID && x.NoteID == NoteID).FirstOrDefault();
                if (result != null)
                {
                    Account cldaccount = new Account(
                        cloudinaryEntity["CloudinarySettings:CloudName"],
                        cloudinaryEntity["CloudinarySettings:ApiKey"],
                        cloudinaryEntity["CloudinarySettings:ApiSecret"]
                        );
                    Cloudinary cloudinary = new Cloudinary(cldaccount);
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(image.FileName, image.OpenReadStream()),
                    };
                    var uploadResult = cloudinary.Upload(uploadParams);
                    string imagePath = uploadResult.Url.ToString();
                    result.Image = imagePath;
                    fundooContext.SaveChanges();
                    return "Image Uploaded Successfully";
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
