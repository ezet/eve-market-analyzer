using eZet.Eve.MarketDataApi.Dto.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eZet.Eve.EveProfiteer.Models {
    public class MarketAnalyzerResult {
        public long TypeId { get; set; }

        public long RegionId { get; set; }

        public string Date { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public decimal AvgPrice { get; set; }

        public long Volume { get; set; }

        public long Orders { get; set; }

        public MarketAnalyzerResult(XmlItemHistory row) {
            TypeId = row.TypeId;
            RegionId = row.RegionId;
            Date = row.Date;
            MinPrice = row.MinPrice;
            MaxPrice = row.MaxPrice;
            AvgPrice = row.AvgPrice;
            Volume = row.Volume;
            Orders = row.Orders;
        }
    }
}
