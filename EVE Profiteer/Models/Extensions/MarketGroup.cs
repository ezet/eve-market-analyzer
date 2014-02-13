using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace eZet.Eve.EveProfiteer.Models {
    public partial class MarketGroup : TreeViewNode {

        private IList<TreeViewNode> _children;

        [NotMapped]
        public IList<TreeViewNode> Children {
            get {
                if (_children == null)
                    LoadChildren();
                return _children;
            }
            private set {
                _children = value;
            }
        }

        public void LoadChildren() {
            _children = new ObservableCollection<TreeViewNode>(SubGroups);
            foreach (var item in Items) {
                _children.Add(item);
            }
        }

        public override void UpdateChildren(bool? value) {
            foreach (var child in Children) {
                child.setIsChecked(value, true, false);
            }
        }

        public override void UpdateParent() {
            bool? state = ParentGroup.Children.First().IsChecked;
            foreach (var child in ParentGroup.Children) {
                if (state != child.IsChecked) {
                    state = null;
                    break;
                }
            }
            ParentGroup.setIsChecked(state, false, true);
        }
    }

    public partial class Item : TreeViewNode {

    }

    public abstract class TreeViewNode : INotifyPropertyChanged {

        protected bool? _isChecked = false;

        public MarketGroup ParentGroup;

        public bool? IsChecked {
            get {
                return _isChecked;
            }
            set {
                setIsChecked(value, true, true);
            }
        }

        internal void setIsChecked(bool? value, bool updateChildren, bool updateParent) {
            if (value == _isChecked)
                return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue)
                UpdateChildren(value);

            if (updateParent && ParentGroup != null)
                UpdateParent();

            OnPropertyChanged("IsChecked");
        }

        public virtual void UpdateParent() {

        }

        public virtual void UpdateChildren(bool? value) {

        }

        public void OnPropertyChanged(string s) {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}