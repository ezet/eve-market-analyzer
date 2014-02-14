using eZet.Eve.EveProfiteer.Models;
using eZet.Eve.MarketDataApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eZet.Eve.EveProfiteer.Services {

    public class EveMarketDataService {

        private MarketData api = new MarketData(Format.Xml);

        public ICollection<MarketAnalyzerResult> GetItemHistory() {
            var options = new MarketDataOptions();
            options.Items.Add(34);
            options.Regions.Add(10000002);
            var response = api.GetItemHistory(options);
            var list = new List<MarketAnalyzerResult>();

            foreach (var row in response.Result.RowSet.Row) {
                list.Add(new MarketAnalyzerResult(row));
            }
            return list;
        }

        public ICollection<MarketAnalyzerResult> GetItemHistory(Region region, ICollection<Item> items) {
            var options = new MarketDataOptions();
            options.Regions.Add(region.RegionId);
            foreach (var item in items) {
                options.Items.Add(item.TypeId);
            }
            var response = api.GetItemHistory(options);
            var list = new List<MarketAnalyzerResult>();
            foreach (var row in response.Result.RowSet.Row) {
                list.Add(new MarketAnalyzerResult(row));
            }
            return list;
        }
    }
}
