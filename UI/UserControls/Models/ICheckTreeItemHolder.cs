using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_collection_manager.UI.UserControls.Models
{
    public interface ICheckTreeItemHolder
    {
        bool? Selected { get; set; }
        void SetSelected(bool? selected);
        IEnumerable<TC> GetSelected<TC>() where TC : ICheckTreeItemHolder;
        void CheckSelected();
    }
}