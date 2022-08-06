using CommonLayer.Modal;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interface
{
    public interface ILabelBL
    {
        public LabelEntity AddLabel(LabelModel label, long userId);

        public IEnumerable<LabelEntity> GetLabel(long LabelID, long userId);

        public LabelEntity RenameLabel(long LabelID, string newLabelName, long userId);

        public bool DeleteLabel(long LabelID);
    }
}
