﻿using eZet.Eve.EveProfiteer.Entities;
using eZet.Eve.EveProfiteer.Models;
using System;
using System.Collections.Generic;
using eZet.EveLib.EveMarketData;

namespace eZet.Eve.EveProfiteer.Services {

    public class EveMarketService {

        private readonly EveMarketData api = new EveMarketData(Format.Json);

        public MarketAnalyzer GetMarketAnalyzer(Region region, ICollection<Item> items, IProgress<ProgressType> progress) {
            progress.Report(new ProgressType(0, "Configuring query..."));
            var options = new EveMarketDataOptions();
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

        public Uri GetScannerLink(ICollection<Int64> items) {
            var options = new EveMarketDataOptions {Items = items};
            return api.GetScannerUri(options);
        }
    }
}
