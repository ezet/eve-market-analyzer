using eZet.Eve.EveProfiteer.Commands;
using eZet.Eve.EveProfiteer.Models;
using eZet.Eve.EveProfiteer.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace eZet.Eve.EveProfiteer.ViewModels {
    public class MarketAnalyzerViewModel {

        private EveDbService eveDb;

        private EveMarketDataService marketApi;

        public Region SelectedRegion { get; set; }

        public List<Item> SelectedItems { get; private set; }

        public ObservableCollection<MarketGroup> TreeRootNodes { get; private set; }

        public ObservableCollection<MarketAnalyzerResult> MarketAnalyzerResults { get; private set; }

        public List<Region> Regions { get; private set; }


        public static ICommand AnalyzeCommand { get; private set; }

        public MarketAnalyzerViewModel(EveDbService eveDb, EveMarketDataService marketApi) {
            AnalyzeCommand = new RelayCommand<MarketAnalyzerViewModel>(AnalyzeAction, parameter => parameter != null && parameter.SelectedRegion != null );
            SelectedItems = new List<Item>();
            MarketAnalyzerResults = new ObservableCollection<MarketAnalyzerResult>();
            this.marketApi = marketApi;
            this.eveDb = eveDb;
            TreeRootNodes = new ObservableCollection<MarketGroup>(eveDb.GetRootMarketGroups());
            Regions = new List<Region>(eveDb.GetRegions());
        }

        public void AnalyzeAction(MarketAnalyzerViewModel vm) {
            MarketAnalyzerResults.Clear();
            //MarketAnalyzerResults = marketApi.GetItemHistory(SelectedRegion, SelectedItems);
            foreach (var item in marketApi.GetItemHistory()){
                MarketAnalyzerResults.Add(item);
            }
        }

    }
}
