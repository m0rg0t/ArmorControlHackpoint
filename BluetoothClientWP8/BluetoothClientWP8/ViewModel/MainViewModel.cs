using System.Collections.ObjectModel;
using BluetoothClientWP8.Model;
using GalaSoft.MvvmLight;

namespace BluetoothClientWP8.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private bool _loading;
        /// <summary>
        /// Loading progress
        /// </summary>
        public bool Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                RaisePropertyChanged("Loading");
            }
        }

        private ObservableCollection<AccelerationItem> _items = new ObservableCollection<AccelerationItem>();
        /// <summary>
        /// AccelerationItems full list
        /// </summary>
        public ObservableCollection<AccelerationItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged("Items");
            }
        }
        
        
    }
}