using eZet.Eve.EveProfiteer.Commands;
using eZet.Eve.EveProfiteer.Models;
using eZet.Eve.EveProfiteer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using eZet.Eve.EveProfiteer.Views;

namespace eZet.Eve.EveProfiteer.ViewModels {
    public class MarketAnalyzerViewModel : ViewModel {

        private EveDbService dataService { get; set; }

        private EveMarketDataService marketService { get; set; }

        private DialogService dialogService { get; set; }

        public Region SelectedRegion { get; set; }

        public ICollection<Item> SelectedItems { get; private set; }

        public ICollection<MarketGroup> TreeRootNodes { get; private set; }

        private List<MarketAnalyzerResult> _marketAnalyzerResults;

        public List<MarketAnalyzerResult> MarketAnalyzerResults {
            get {
                return _marketAnalyzerResults;
            }
            private set {
                _marketAnalyzerResults = value;
                onPropertyChanged("MarketAnalyzerResults");
            }
        }


        public ICollection<Region> Regions { get; private set; }

        public ICommand AnalyzeCommand { get; private set; }

        private int _analyzeProgress;

        public int AnalyzeProgress {
            get {
                return _analyzeProgress;
            }
            private set {
                _analyzeProgress = value; onPropertyChanged("AnalyzeProgress");
            }
        }

        private int _dayLimit = 10;

        public int DayLimit {
            get {
                return _dayLimit;
            }
            private set {
                _dayLimit = value;
                onPropertyChanged("DayLimit");
            }
        }

        private AnalyzerProgressView AnalyzerProgressDialog { get; set; }

        public MarketAnalyzerViewModel(EveDbService dataService, EveMarketDataService marketService, DialogService dialogService) {
            this.dataService = dataService;
            this.marketService = marketService;
            this.dialogService = dialogService;
            SelectedItems = new List<Item>();
            AnalyzeCommand = new RelayCommand<MarketAnalyzerViewModel>(AnalyzeAction, parameter => parameter != null && parameter.SelectedRegion != null && parameter.SelectedItems.Count != 0);
            TreeRootNodes = buildTree();
            Regions = dataService.GetRegions().ToList();
        }

        public void AnalyzeAction(MarketAnalyzerViewModel vm) {
            var cts = new CancellationTokenSource();
            var progressVm = new AnalyzerProgressViewModel();
            var progress = progressVm.GetProgressReporter();
            Task.Factory.StartNew<MarketAnalyzer>(() => getResult(progress), cts.Token).ContinueWith(task => MarketAnalyzerResults = task.Result.Items.Values.ToList());
            dialogService.ShowDialog(progressVm);
        }

        private MarketAnalyzer getResult(IProgress<int> progress) {
            return marketService.GetMarketAnalyzer(SelectedRegion, SelectedItems, progress);
        }

        private ICollection<MarketGroup> buildTree() {
            var rootList = new List<MarketGroup>();
            dataService.SetLazyLoad(false);
            var items = dataService.GetItems().Where(p => p.MarketGroupId.HasValue).ToList();
            var groupList = dataService.GetMarketGroups().ToList();
            var groups = groupList.ToDictionary(t => t.MarketGroupId);

            foreach (var item in items) {
                MarketGroup group;
                var id = item.MarketGroupId ?? default(int);
                groups.TryGetValue(id, out group);
                group.Children.Add(item);
                item.PropertyChanged += treeViewCheckBox_PropertyChanged;
            }
            foreach (var key in groups) {
                if (key.Value.ParentGroupId.HasValue) {
                    MarketGroup group;
                    var id = key.Value.ParentGroupId ?? default(int);
                    groups.TryGetValue(id, out group);
                    group.Children.Add(key.Value);
                } else {
                    rootList.Add(key.Value);
                }
            }
            dataService.SetLazyLoad(true);
            return rootList;
        }

        private void treeViewCheckBox_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            var item = sender as Item;
            if (e.PropertyName == "IsChecked") {
                if (item.IsChecked == true) {
                    SelectedItems.Add(item);
                } else if (item.IsChecked == false)
                    SelectedItems.Remove(item);
                else {
                    throw new NotImplementedException();
                }
            }
        }


    }
}
