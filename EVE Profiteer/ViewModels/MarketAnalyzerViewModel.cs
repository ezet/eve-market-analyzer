using eZet.Eve.EveProfiteer.Commands;
using eZet.Eve.EveProfiteer.Models;
using eZet.Eve.EveProfiteer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace eZet.Eve.EveProfiteer.ViewModels {
    public class MarketAnalyzerViewModel : INotifyPropertyChanged {

        private EveDbService eveDb;

        private EveMarketDataService marketApi;

        public Region SelectedRegion { get; set; }

        public ICollection<Item> SelectedItems { get; private set; }

        public ICollection<MarketGroup> TreeRootNodes { get; private set; }

        public ICollection<MarketAnalyzerResult> MarketAnalyzerResults { get; private set; }

        public ICollection<Region> Regions { get; private set; }

        public ICommand AnalyzeCommand { get; private set; }

        public MarketAnalyzerViewModel(EveDbService eveDb, EveMarketDataService marketApi) {
            this.eveDb = eveDb;
            this.marketApi = marketApi;
            AnalyzeCommand = new RelayCommand<MarketAnalyzerViewModel>(AnalyzeAction, parameter => parameter != null && parameter.SelectedRegion != null && parameter.SelectedItems.Count != 0);
            SelectedItems = new List<Item>();
            TreeRootNodes = eveDb.GetRootMarketGroups().ToList();
            Regions = eveDb.GetRegions().ToList();
            attachChangeHandler(TreeRootNodes);
        }

        public void AnalyzeAction(MarketAnalyzerViewModel vm) {
            //var items = getSelectedItems(TreeRootNodes, new List<Item>());
            //var result = marketApi.GetItemHistory(SelectedRegion, items);
            var result = marketApi.GetItemHistory();
            MarketAnalyzerResults = new List<MarketAnalyzerResult>(result);
            onPropertyChanged("MarketAnalyzerResults");

        }

        private void attachChangeHandler(ICollection<MarketGroup> list) {
            foreach (var child in list) {
                foreach (var item in child.Items) {
                    item.PropertyChanged += treeViewCheckBox_PropertyChanged;
                }
                if (child.SubGroups.Count != 0)
                    attachChangeHandler(child.SubGroups);
            }
        }

        private IList<Item> getSelectedItems(ICollection<MarketGroup> list, List<Item> items) {
            foreach (var child in list) {
                if (child.IsChecked != false) {
                    items.AddRange(from item in items
                                   where item.IsChecked == true
                                   select item);
                    getSelectedItems(child.SubGroups, items);
                }
            }
            return items;
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

        private void onPropertyChanged(string name) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
