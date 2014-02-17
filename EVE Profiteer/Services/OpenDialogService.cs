using eZet.Eve.EveProfiteer.ViewModels;
using eZet.Eve.EveProfiteer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace eZet.Eve.EveProfiteer.Services {
    public class DialogService {

        public Dictionary<ViewModel, Window> Dialogs {get; private set;}

        public DialogService() {
            Dialogs = new Dictionary<ViewModel, Window>();
        }

        public void ShowDialog(ViewModel vm) {
            var view = new AnalyzerProgressView();
            view.DataContext = vm;
            Dialogs.Add(vm, view);
            var result = view.ShowDialog();
        }

        public void CloseDialog(ViewModel vm) {
            Window window;
            if (Dialogs.TryGetValue(vm, out window)) {
                window.Dispatcher.BeginInvoke((Action)(() => window.Close()));
            }
        }
    }
}
