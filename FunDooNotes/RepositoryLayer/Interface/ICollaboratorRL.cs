using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICollaboratorRL
    {
        public CollaboratorEntity AddCollab(long noteId, long userId, string email);

        public IEnumerable<CollaboratorEntity> GetCollaboratorByID(long userId, long CollaboratorID);

        public bool DeleteCollaborator(long CollaboratorID);
    }
}
