using System.ComponentModel;

namespace eZet.Eve.EveProfiteer.ViewModels {
    public abstract class ViewModel : INotifyPropertyChanged {

        protected virtual void onPropertyChanged(string name) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

