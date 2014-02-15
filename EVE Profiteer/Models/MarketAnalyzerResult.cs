using eZet.Eve.MarketDataApi.Dto.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eZet.Eve.EveProfiteer.Models {
    public class MarketAnalyzerResult {

        public List<XmlItemHistory> Data { get; private set; }

        public MarketAnalyzerResult(Region region, Item item) {
            TypeId = item.TypeId;
            ItemName = item.TypeName;
            RegionId = region.RegionId;
            RegionName = region.RegionName;
            Data = new List<XmlItemHistory>();
        }

        public long TypeId { get; private set; }

        public long RegionId { get; private set; }

        public string ItemName { get; private set; }

        public string RegionName { get; private set; }

        public decimal AvgVolume { get; private set; }

        public decimal AvgPrice { get; private set; }

        public decimal AvgBuyPrice { get; private set; }

        public decimal AvgSellPrice { get; private set; }

        public decimal ProfitPerItem { get; private set; }

        public decimal DailyProfit { get; private set; }

        public decimal Margin { get; private set; }

        public void Calculate() {
            AvgVolume = Data.Sum(t => t.Volume) / Data.Count;
            AvgPrice = Data.Sum(t => t.AvgPrice) / Data.Count;
            AvgBuyPrice = Data.Sum(t => t.MinPrice) / Data.Count;
            AvgSellPrice = Data.Sum(t => t.MaxPrice) / Data.Count;
            ProfitPerItem = AvgSellPrice - AvgBuyPrice;
            Margin = ProfitPerItem / AvgBuyPrice;
            DailyProfit = ProfitPerItem * AvgVolume;
        }

 
    }
}
