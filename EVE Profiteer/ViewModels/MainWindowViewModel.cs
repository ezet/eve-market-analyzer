using eZet.Eve.EveProfiteer.Models;
using eZet.Eve.EveProfiteer.Services;
using eZet.Eve.MarketDataApi;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace eZet.Eve.EveProfiteer.ViewModels {
    public class MarketAnalyzerViewModel {

        private EveDbService eveDb;

        private EveMarketDataService marketApi;

        public ObservableCollection<MarketGroup> TreeRootNodes { get; private set; }

        public ObservableCollection<MarketAnalyzerResult> MarketAnalyzerResults { get; private set; }

        public MarketAnalyzerViewModel(EveDbService eveDb, EveMarketDataService marketApi) {
            this.marketApi = marketApi;
            this.eveDb = eveDb;
            TreeRootNodes = new ObservableCollection<MarketGroup>(eveDb.GetRootMarketGroups());
            MarketAnalyzerResults = marketApi.GetItemHistory();
        }

        public void getResult() {
            var list = marketApi.GetItemHistory();
        }

    }
}
