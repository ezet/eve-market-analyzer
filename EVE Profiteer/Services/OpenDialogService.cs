using eZet.Eve.EveProfiteer.ViewModels;
using eZet.Eve.EveProfiteer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eZet.Eve.EveProfiteer.Services {
    public class DialogService {

        public void ShowDialog(AnalyzerProgressViewModel vm) {
            var view = new AnalyzerProgressView();
            view.DataContext = vm;
            var result = view.ShowDialog();
        }
    }
}
