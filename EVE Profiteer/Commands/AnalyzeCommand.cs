using eZet.Eve.EveProfiteer.ViewModels;
using System;
using System.Windows.Input;

namespace eZet.Eve.EveProfiteer.Commands {
    public class AnalyzeCommand : ICommand {

        bool canExecute;

        public bool CanExecute(object parameter) {
            var vm = parameter as MarketAnalyzerViewModel;
            bool temp = vm != null && vm.SelectedRegion != null;
            if (canExecute != temp)
                CanExecuteChanged(vm, new EventArgs());
            canExecute = temp;
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter) {
            var vm = parameter as MarketAnalyzerViewModel;
        }
    }
}
