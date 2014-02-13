using eZet.Eve.EveProfiteer.Models;
using eZet.Eve.EveProfiteer.Services;
using System.Collections.ObjectModel;

namespace eZet.Eve.EveProfiteer.ViewModels {
    public class MainWindowViewModel {

        private EveDbService eveDb;

        public ObservableCollection<MarketGroup> TreeRootNodes { get; private set; }

        public ObservableCollection<MarketAnalyzerResult> MarketAnalyzerResults { get; private set; }

        public MainWindowViewModel(EveDbService eveDb) {
            this.eveDb = eveDb;
            var query = eveDb.GetRootMarketGroups();
            var list = new ObservableCollection<MarketGroup>(query);
            TreeRootNodes = list;
        }

    }
}
