using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class CollaboratorRL : ICollaboratorRL
    {
        private readonly FundooContext fundooContext;


        public CollaboratorRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;

        }


        public CollaboratorEntity AddCollab(long noteId, long userId, string email)
        {
            try
            {
                CollaboratorEntity collaboratorEntity = new CollaboratorEntity();
                collaboratorEntity.Email = email;
                collaboratorEntity.UserId = userId;
                collaboratorEntity.NoteID = noteId;

                fundooContext.CollaboratorTable.Add(collaboratorEntity);
                int result = fundooContext.SaveChanges();

                if (result != null)
                {
                    return collaboratorEntity;
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


        public IEnumerable<CollaboratorEntity> GetCollaboratorByID(long userId, long CollaboratorID)
        {
            try
            {
                var result = fundooContext.CollaboratorTable.Where(e => e.UserId == userId && e.CollaboratorID == CollaboratorID);
                if (result != null)
                {
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

        public bool DeleteCollaborator(long CollaboratorID)
        {
            try
            {

                var result = fundooContext.CollaboratorTable.Where(x => x.CollaboratorID == CollaboratorID).FirstOrDefault();
                if (result != null)
                {
                    fundooContext.CollaboratorTable.Remove(result);
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


    }

}
