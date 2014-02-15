using eZet.Eve.MarketDataApi.Dto.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eZet.Eve.EveProfiteer.Models {
    public class MarketAnalyzer {

        public Dictionary<long, MarketAnalyzerResult> Items { get; set; }

        public MarketAnalyzer(Region region, ICollection<Item> items, XmlResponse<XmlItemHistory> history) {
            Items = new Dictionary<long, MarketAnalyzerResult>();
            var historyData = history.Result.RowSet.Row.ToList();
            //var currentData = current.Result.RowSet.Row.ToList();
            foreach (var item in items) {
                Items.Add(item.TypeId, new MarketAnalyzerResult(region, item));
            }
            foreach (var item in historyData) {
                Items[item.TypeId].Data.Add(item);
            }
        }

        public void Calculate() {
            foreach (var item in Items.Values) {
                if (item.Data.Count != 0)
                    item.Calculate();
            }
        }

    }
}
