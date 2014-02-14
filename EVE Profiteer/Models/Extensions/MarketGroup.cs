using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eZet.Eve.EveProfiteer.Models {
    public partial class MarketGroup : TreeNode {

        [NotMapped]
        private ICollection<TreeNode> _children;

        [NotMapped]
        public override TreeNode Parent {
            get {
                return ParentGroup;
            }
            set {
                ParentGroup = value as MarketGroup;
            }
        }

        [NotMapped]
        public override ICollection<TreeNode> Children {
            get {
                if (_children == null)
                    LoadChildren();
                return _children;
            }
            set {
                _children = value;
            }
        }

        public void LoadChildren() {
            _children = new List<TreeNode>(SubGroups);
            foreach (var item in Items) {
                _children.Add(item);
            }
        }
    }
}
