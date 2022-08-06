using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interface
{
    public interface ICollaboratorBL
    {
        public CollaboratorEntity AddCollab(long noteId, long userId, string email);

        public IEnumerable<CollaboratorEntity> GetCollaboratorByID(long userId, long CollaboratorID);

        public bool DeleteCollaborator(long CollaboratorID);
    }
}
