using BussinessLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Service
{
    public class CollaboratorBL : ICollaboratorBL
    {
        private readonly ICollaboratorRL icollaboratorRL;

        public CollaboratorBL(ICollaboratorRL icollaboratorRL)
        {
            this.icollaboratorRL = icollaboratorRL;
        }

        public CollaboratorEntity AddCollab(long noteId, long userId, string email)
        {
            try
            {
                return icollaboratorRL.AddCollab(noteId, userId, email);
            }
            catch (Exception e)
            {

                throw;
            }
        }


        public IEnumerable<CollaboratorEntity> GetCollaboratorByID(long userId, long CollaboratorID)
        {
            try
            {
                return icollaboratorRL.GetCollaboratorByID(CollaboratorID, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool DeleteCollaborator(long CollaboratorId)
        {
            try
            {
                return icollaboratorRL.DeleteCollaborator(CollaboratorId);
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
