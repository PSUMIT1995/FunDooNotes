using CommonLayer.Modal;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class LabelRL : ILabelRL
    {
        private readonly FundooContext fundooContext;


        public LabelRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;

        }


        public LabelEntity AddLabel(LabelModel label, long userId)
        {
            try
            {
                var result = fundooContext.LabelTable.Where(e => e.NoteID == label.NoteID);
                if (result != null)
                {
                    LabelEntity labelEntity = new LabelEntity();
                    labelEntity.LabelName = label.LabelName;
                    labelEntity.NoteID = label.NoteID;
                    labelEntity.UserId = userId;
                    fundooContext.LabelTable.Add(labelEntity);
                    fundooContext.SaveChanges();
                    return labelEntity;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }


        public IEnumerable<LabelEntity> GetLabel(long LabelID,long userId)
        {
            try
            {
                var result = fundooContext.LabelTable.Where(e => e.LabelID == LabelID && e.UserId == userId);
                if (result != null)
                {
                    return result;

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public LabelEntity RenameLabel(long LabelID, string newLabelName, long userId)
        {
            try
            {
                var result = fundooContext.LabelTable.Where(e => e.LabelID == LabelID && e.UserId == userId).FirstOrDefault();
                if (result != null)
                {
                    result.LabelName = newLabelName;
                    fundooContext.LabelTable.Update(result);
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


        public bool DeleteLabel(long LabelID)
        {
            try
            {
                var result = fundooContext.LabelTable.Where(e => e.LabelID == LabelID).FirstOrDefault();
                if (result != null)
                {
                    fundooContext.LabelTable.Remove(result);
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
