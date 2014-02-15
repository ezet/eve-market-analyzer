using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eZet.Eve.EveProfiteer.ViewModels {
    public class AnalyzerProgressViewModel : ViewModel {

        private int _progress;

        public int Progress {
            get {
                return _progress;
            }
            set {
                _progress = value;
                onPropertyChanged("Progress");
            }
        }

        public IProgress<int> GetProgressReporter() {
            return new Progress<int>(t => Progress = t);
        }

    }
}
