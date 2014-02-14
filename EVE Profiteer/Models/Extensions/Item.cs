﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eZet.Eve.EveProfiteer.Models {
    public partial class Item : TreeNode {

        [NotMapped]
        public override TreeNode Parent {
            get {
                return ParentGroup;
            }
            set {
                ParentGroup = value as MarketGroup;
            }
        }
    }
}
