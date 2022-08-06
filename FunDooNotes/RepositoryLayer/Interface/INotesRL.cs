using CommonLayer.Modal;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface INotesRL
    {
        public NotesEntity CreateNotes(NotesModel notesModel, long userId);

        public IEnumerable<NotesEntity> ReadNotes(long userId);

        public bool DeleteNotes(long userId, long noteId);

        public NotesEntity UpdateNote(NotesModel notesModel, long noteId, long userId);

        public bool PinToTop(long noteId, long userId);

        public bool Archive(long noteId, long userId);

        public bool Trash(long noteId, long userId);

        public NotesEntity Color(long noteId, string color);

        public string Image(IFormFile image, long NoteID, long userID);
    }
}
