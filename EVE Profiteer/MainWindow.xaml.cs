using eZet.Eve.EveProfiteer.Models;
using eZet.Eve.EveProfiteer.Services;
using eZet.Eve.EveProfiteer.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace eZet.Eve.EveProfiteer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public MarketAnalyzerViewModel ViewModel {
            set {
                DataContext = value;
            }
        }

        public MainWindow() {
            InitializeComponent();
            DataContext = new MarketAnalyzerViewModel(new EveDbService(), new EveMarketDataService());
        }

    }

}
