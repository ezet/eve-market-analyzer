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

        //public MarketAnalyzer GetItemHistory() {
        //    var options = new MarketDataOptions();
        //    options.Items.Add(34);
        //    options.Items.Add(456);
        //    options.Regions.Add(10000002);
        //    var response = api.GetItemHistory(options);
        //    return new MarketAnalyzer(nresponse);
        //}

        public MarketAnalyzer GetMarketAnalyzer(Region region, ICollection<Item> items, IProgress<int> progress) {
            progress.Report(0);
            var options = new MarketDataOptions();
            options.Regions.Add(region.RegionId);
            foreach (var item in items) {
                options.Items.Add(item.TypeId);
            }
            progress.Report(25);
            var response = api.GetItemHistory(options);
            progress.Report(50);
            var result = new MarketAnalyzer(region, items, response);
            progress.Report(75);
            result.Calculate();
            progress.Report(100);
            return result;
        }
    }
}
