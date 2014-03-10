using System;

namespace eZet.Eve.EveProfiteer.ViewModels {
    public class ScannerLinkViewModel : ViewModel {

        public Uri Uri { get; private set; }

        public ScannerLinkViewModel(Uri uri) {
            Uri = uri;

        }



    }
}
