using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using osu_collection_manager.Annotations;

namespace osu_collection_manager.UI.UserControls.Models
{
    public class BaseCheckTreeItemHolder<P, C> : ICheckTreeItemHolder, INotifyPropertyChanged
        where P : ICheckTreeItemHolder where C : ICheckTreeItemHolder
    {
        public virtual P Parent { get; set; }
        public virtual BindingList<C> Children { get; set; }

        private bool? _selected;
        public virtual bool? Selected
        {
            get { return _selected; }
            set
            {
                SetSelected(value);
                Parent?.CheckSelected();
            }
        }

        public virtual void SetSelected(bool? selected)
        {
            _selected = selected;
            if (Children != null && selected != null)
                foreach (var child in Children)
                {
                    child.SetSelected((bool)selected);
                }
            OnPropertyChanged(nameof(Selected));
        }

        public IEnumerable<TC> GetSelected<TC>() where TC : ICheckTreeItemHolder
        {
            throw new NotImplementedException();
        }

        IEnumerable<C> GetSelected()
        {
            return Children.Where(child => child.Selected == true).ToList();
        }

        public void CheckSelected()
        {
            if (Children.Count == 0) return;
            _selected = Children[0].Selected;
            for (var i = 1; i < Children.Count; i++)
            {
                if (_selected == Children[i].Selected) continue;
                _selected = null;
                break;
            }
            OnPropertyChanged(nameof(Selected));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
