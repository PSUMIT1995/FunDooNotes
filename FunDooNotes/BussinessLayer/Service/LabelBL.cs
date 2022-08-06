using BussinessLayer.Interface;
using CommonLayer.Modal;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Service
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL ilabelRL;

        public LabelBL(ILabelRL ilabelRL)
        {
            this.ilabelRL = ilabelRL;
        }

        public LabelEntity AddLabel(LabelModel label, long userId)
        {
            try
            {
                return ilabelRL.AddLabel(label, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public IEnumerable<LabelEntity> GetLabel(long LabelID, long userId)
        {
            try
            {
                return ilabelRL.GetLabel(LabelID, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public LabelEntity RenameLabel(long LabelID, string newLabelName, long userId)
        {
            try
            {
                return ilabelRL.RenameLabel(LabelID, newLabelName, userId);
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
                return ilabelRL.DeleteLabel(LabelID);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
