using eZet.Eve.EveProfiteer.Models;
using eZet.Eve.EveProfiteer.Services;
using eZet.Eve.MarketDataApi;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace eZet.Eve.EveProfiteer.ViewModels {
    public class MarketAnalyzerViewModel {

        private EveDbService eveDb;

        private EveMarketDataService marketApi;

        public long SelectedRegion { get; set; }

        public ObservableCollection<MarketGroup> TreeRootNodes { get; private set; }

        public ObservableCollection<MarketAnalyzerResult> MarketAnalyzerResults { get; private set; }

        public List<Region> Regions { get; private set; }

        public MarketAnalyzerViewModel(EveDbService eveDb, EveMarketDataService marketApi) {
            this.marketApi = marketApi;
            this.eveDb = eveDb;
            TreeRootNodes = new ObservableCollection<MarketGroup>(eveDb.GetRootMarketGroups());
            MarketAnalyzerResults = marketApi.GetItemHistory();
            Regions = new List<Region>(eveDb.GetRegions());
        }

        public void getResult() {
            var list = marketApi.GetItemHistory();
        }

    }
}
