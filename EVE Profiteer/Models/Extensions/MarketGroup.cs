using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace eZet.Eve.EveProfiteer.Models {
    public partial class MarketGroup : TreeViewNode {

        [NotMapped]
        public ObservableCollection<TreeViewNode> Children {
            get {
                var children = new ObservableCollection<TreeViewNode>();
                foreach (var group in SubGroups) {
                    children.Add(group);
                }
                foreach (var item in Items) {
                    children.Add(item);
                }
                return children as ObservableCollection<TreeViewNode>;
            }
        }
    }

    public partial class Item : TreeViewNode {

    }

    public class TreeViewNode : FrameworkContentElement {

        public bool Check { get; set; }


    }
}