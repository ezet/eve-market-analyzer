using eZet.Eve.EveProfiteer.Services;
using eZet.Eve.EveProfiteer.ViewModels;
using System.Windows;

namespace eZet.Eve.EveProfiteer {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private MarketAnalyzerViewModel vm;

        public MainWindow() {
            InitializeComponent();
            vm = new MarketAnalyzerViewModel(new EveDataService(), new EveMarketService(), new DialogService());
            DataContext = vm;
            Splitter.DragDelta += SplitterNameDragDelta;
        }

        private void SplitterNameDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e) {
            MainGrid.ColumnDefinitions[0].Width = new GridLength(MainGrid.ColumnDefinitions[0].ActualWidth + e.HorizontalChange);
        }
    }

}
