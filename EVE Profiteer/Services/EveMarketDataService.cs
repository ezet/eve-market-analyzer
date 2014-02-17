using eZet.Eve.EveProfiteer.Entities;
using eZet.Eve.EveProfiteer.Models;
using eZet.Eve.MarketDataApi;
using System;
using System.Collections.Generic;

namespace eZet.Eve.EveProfiteer.Services {

    public class EveMarketService {

        private readonly MarketData api = new MarketData(Format.Xml);

        //public MarketAnalyzer GetItemHistory() {
        //    var options = new MarketDataOptions();
        //    options.Items.Add(34);
        //    options.Items.Add(456);
        //    options.Regions.Add(10000002);
        //    var response = api.GetItemHistory(options);
        //    return new MarketAnalyzer(nresponse);
        //}

        public MarketAnalyzer GetMarketAnalyzer(Region region, ICollection<Item> items, IProgress<ProgressType> progress) {
            progress.Report(new ProgressType(0, "Configuring query..."));
            var options = new MarketDataOptions();
            options.Regions.Add(region.RegionId);
            foreach (var item in items) {
                options.Items.Add(item.TypeId);
            }
            progress.Report(new ProgressType(25, "Fetching history data..."));
            var response = api.GetItemHistory(options);
            progress.Report(new ProgressType(50, "Initializing analysis..."));
            var result = new MarketAnalyzer(region, items, response);
            progress.Report(new ProgressType(75, "Analyzing..."));
            result.Calculate();
            progress.Report(new ProgressType(100, "Finished."));
            return result;
        }
    }
}
