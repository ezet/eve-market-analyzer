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

    public abstract class TreeNode : INotifyPropertyChanged {

        protected bool? _isChecked = false;

        public virtual TreeNode Parent { get; set; }

        public virtual ICollection<TreeNode> Children { get; set; }

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

            if (updateChildren && _isChecked.HasValue && Children != null)
                UpdateChildren(value);

            if (updateParent && Parent != null)
                UpdateParent();

            OnPropertyChanged("IsChecked");
        }

        public void UpdateParent() {
            bool? state = Parent.Children.First().IsChecked;
            foreach (var child in Parent.Children) {
                if (state != child.IsChecked) {
                    state = null;
                    break;
                }
            }
            Parent.setIsChecked(state, false, true);
        }

        public void UpdateChildren(bool? value) {
            foreach (var child in Children) {
                child.setIsChecked(value, true, false);
            }
        }

        public void OnPropertyChanged(string name) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}