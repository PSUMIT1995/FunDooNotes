using BussinessLayer.Interface;
using CommonLayer.Modal;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Service
{
    public class NotesBL : INotesBL
    {
        private readonly INotesRL inotesRL;

        public NotesBL(INotesRL inotesRL)
        {
            this.inotesRL = inotesRL;
        }


        public NotesEntity CreateNotes(NotesModel notesModel, long userId)
        {
            try
            {
                return inotesRL.CreateNotes(notesModel, userId);
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
                return inotesRL.ReadNotes(userId);
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
                return inotesRL.DeleteNotes(userId, noteId);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public NotesEntity UpdateNote(NotesModel notesModel, long noteId, long userId)
        {
            try
            {
                return inotesRL.UpdateNote(notesModel, noteId, userId);
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
                return inotesRL.PinToTop(noteId, userId);
            }
            catch (Exception e)
            {

                throw;
            }
        }



        public bool Archive(long noteId, long userId)
        {
            try
            {
                return inotesRL.Archive(noteId, userId);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public bool Trash(long noteId, long userId)
        {
            try
            {
                return inotesRL.Trash(noteId, userId);
            }
            catch (Exception e)
            {

                throw;
            }
        }



        public NotesEntity Color(long noteId, string color)
        {
            try
            {
                return inotesRL.Color(noteId, color);
            }
            catch (Exception e)
            {

                throw;
            }
        }


        public string Image(IFormFile image, long NoteID, long userID)
        {
            try
            {
                return inotesRL.Image(image, NoteID, userID);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    
}
