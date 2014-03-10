using eZet.Eve.EveProfiteer.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace eZet.Eve.EveProfiteer.Services {
    public class DialogService {

        public Dictionary<ViewModel, Window> Dialogs {get; private set;}

        public DialogService() {
            Dialogs = new Dictionary<ViewModel, Window>();
        }

        public void ShowDialog(ViewModel vm, Window view) {
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
