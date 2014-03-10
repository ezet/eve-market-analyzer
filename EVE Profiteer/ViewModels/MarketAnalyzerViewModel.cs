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
using eZet.Eve.EveProfiteer.Entities;
using System.Windows.Data;
using eZet.Eve.EveProfiteer.Views;

namespace eZet.Eve.EveProfiteer.ViewModels {
    public class MarketAnalyzerViewModel : ViewModel {
        private EveDataService dataService { get; set; }

        private EveMarketService marketService { get; set; }

        private DialogService dialogService { get; set; }

        public ICommand AnalyzeCommand { get; private set; }

        public ICommand ScannerLinkCommand { get; private set; }

        public Region SelectedRegion { get; set; }

        public ICollection<Item> SelectedItems { get; private set; }

        public ICollection<MarketGroup> TreeRootNodes { get; private set; }
        public ICollection<Region> Regions { get; private set; }

        private ICollectionView _marketAnalyzerResults;

        public ICollectionView MarketAnalyzerResults {
            get { return _marketAnalyzerResults; }
            private set {
                _marketAnalyzerResults = value;
                _marketAnalyzerResults.Filter = filterResults; onPropertyChanged("MarketAnalyzerResults");
            }
        }

        private int _analyzeProgress;

        public int AnalyzeProgress {
            get { return _analyzeProgress; }
            private set { _analyzeProgress = value; onPropertyChanged("AnalyzeProgress"); }
        }

        private int _dayLimit = 10;

        public int DayLimit {
            get { return _dayLimit; }
            private set { _dayLimit = value; onPropertyChanged("DayLimit"); }
        }

        private string _searchFilter;
        public string SearchFilter {
            get { return _searchFilter; }
            set { _searchFilter = value; onPropertyChanged("SearchFilter"); }
        }

        private bool _profitFilterCheckBox = true;

        public bool ProfitFilterCheckBox {
            get { return _profitFilterCheckBox; }
            set { _profitFilterCheckBox = value; onPropertyChanged("ProfitFilterCheckBox"); }
        }

        private decimal _profitFilterValue = 5000000;

        public decimal ProfitFilterValue {
            get { return _profitFilterValue; }
            set { _profitFilterValue = value; onPropertyChanged("ProfitFilterValue"); }
        }

        public MarketAnalyzerViewModel(EveDataService dataService, EveMarketService marketService, DialogService dialogService) {
            CollectionViewSource.GetDefaultView(MarketAnalyzerResults);
            this.dataService = dataService;
            this.marketService = marketService;
            this.dialogService = dialogService;
            PropertyChanged += gridFilter_PropertyChanged;
            SelectedItems = new List<Item>();
            AnalyzeCommand = new RelayCommand<MarketAnalyzerViewModel>(AnalyzeAction, parameter => parameter != null && parameter.SelectedRegion != null && parameter.SelectedItems.Count != 0);
            ScannerLinkCommand = new RelayCommand<MarketAnalyzerViewModel>(ScannerLinkAction, f => f != null && f.MarketAnalyzerResults != null);
            TreeRootNodes = buildTree();
            Regions = dataService.GetRegions().ToList();
            SelectedRegion = Regions.Single(f => f.RegionId == 10000002);
        }

        private bool filterResults(object obj) {
            var item = obj as MarketAnalyzerResult;
            if (item == null)
                return false;
            if (ProfitFilterCheckBox && item.DailyProfit < ProfitFilterValue)
                return false;
            return string.IsNullOrWhiteSpace(SearchFilter) || item.ItemName.ToLower().Contains(SearchFilter.ToLower());
        }

        public void AnalyzeAction(MarketAnalyzerViewModel vm) {
            var cts = new CancellationTokenSource();
            var progressVm = new AnalyzerProgressViewModel(cts);
            var progress = progressVm.GetProgressReporter();
            Task.Factory.StartNew(() => getResult(progress), cts.Token)
                .ContinueWith(task => { MarketAnalyzerResults = new ListCollectionView(task.Result.Items.Values.ToList()); dialogService.CloseDialog(progressVm); }, cts.Token);
            dialogService.ShowDialog(progressVm, new AnalyzerProgressView());
        }

        public void ScannerLinkAction(MarketAnalyzerViewModel vm) {
            var list = MarketAnalyzerResults.Cast<MarketAnalyzerResult>().Where(result => result.IsChecked).Select(f => f.TypeId);
            var uri = marketService.GetScannerLink(list.ToList());
            var scannerVm = new ScannerLinkViewModel(uri);
            dialogService.ShowDialog(scannerVm, new ScannerLinkView());
        }

        private MarketAnalyzer getResult(IProgress<ProgressType> progress) {
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

        private void gridFilter_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            var vm = sender as MarketAnalyzerViewModel;
            if (vm.MarketAnalyzerResults != null && (e.PropertyName == "ProfitFilterValue" || e.PropertyName == "ProfitFilterCheckBox" || e.PropertyName == "SearchFilter"))
                vm.MarketAnalyzerResults.Refresh();
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
