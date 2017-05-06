using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using osu_collection_manager.Annotations;
using osu_collection_manager.Models;
using osu_collection_manager.UI.UserControls.Models;
using UserControl = System.Windows.Controls.UserControl;

namespace osu_collection_manager.UI.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CollectionsTree : UserControl, INotifyPropertyChanged, ICheckTreeItemHolder
    {
        private string _title = "Untitled collection pack";

        private bool? _selected = true;
        public bool? Selected
        {
            get { return _selected; }
            set { SetSelected(value); }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public BindingList<CollectionHolder> Children { get; set; }

        public CollectionsTree()
        {
            InitializeComponent();
            DataContext = this;
            Children = new BindingList<CollectionHolder>();
            TreeViewCollections.ItemsSource = Children;
        }

/*        public void SetSelected(bool? selected)
        {
            _selected = selected;
            if (Collections != null && selected != null)
            {
                Debug.WriteLine("Selectuing all");
                foreach (var collection in Collections)
                {
                    collection.SetSelected((bool) selected);
                }
            }
            OnPropertyChanged(nameof(Selected));
        }

        public List<Collection> GetSelected()
        {
            var ret = new List<Collection>();
            foreach (var collectionHolder in Collections)
            {
                if (collectionHolder.Selected == true)
                {
                    ret.Add(collectionHolder.Data);
                }
                else if (collectionHolder.Selected == null)
                {
                    var col = collectionHolder.Data;
                    col.MapSets.Clear();
                    foreach (var mapsetHolder in collectionHolder.Mapsets)
                    {
                        if (mapsetHolder.Selected)
                            col.MapSets.Add(mapsetHolder.Data);
                    }
                    ret.Add(col);
                }
            }
            return ret;
        }

        public void CheckSelected()
        {
            if (Collections.Count == 0) return;
            _selected = Collections[0].Selected;
            for (var i = 1; i < Collections.Count; i++)
            {
                if (_selected == Collections[i].Selected) continue;
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
        }*/
        public void SetSelected(bool? selected)
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

        public List<Collection> GetSelected(bool apply = false)
        {
            var ret = new List<Collection>();
            foreach (var child in Children)
            {
                if (child.Selected == true)
                {
                    ret.Add(child.GetData(apply));
                }
                else if (child.Selected == null)
                {
                    var col = child.GetData(apply);
                    col.Mapsets.Clear();
                    foreach (var subChild in child.Children)
                    {
                        if (subChild.Selected == true)
                            col.Mapsets.Add(subChild.Data);
                    }
                    ret.Add(col);
                }
            }
            return ret;
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
