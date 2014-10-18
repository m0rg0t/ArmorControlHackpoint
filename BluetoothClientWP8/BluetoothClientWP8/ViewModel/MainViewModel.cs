using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Phone.Speech.Synthesis;
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

        private ObservableCollection<HitItem> _hitItems = new ObservableCollection<HitItem>();
        /// <summary>
        /// List with hit items
        /// </summary>
        public ObservableCollection<HitItem> HitItems
        {
            get { return _hitItems; }
            set
            {
                _hitItems = value;
                RaisePropertyChanged("HitItems");
            }
        }

        /// <summary>
        /// return max hit value for hitItems collection
        /// </summary>
        public double MaxHitValue
        {
            get
            {
                double max = 0;
                foreach (var hit in HitItems)
                {
                    if (max < hit.HitValue)
                    {
                        max = hit.HitValue;
                    }
                }
                return max;
            }
        }

        private HitItem _currentHitItem = null;
        /// <summary>
        /// Current hit item
        /// </summary>
        public HitItem CurrentHitItem
        {
            get { return _currentHitItem; }
            set
            {
                _currentHitItem = value;
                RaisePropertyChanged("CurrentHitItem");
            }
        }
        

        public const int hitLevel = 3000;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task CheckHit()
        {
            ///fix this later - not simpe 3 if, but something better
            //foreach (var item in Items)
            //{
            var item = Items.Last();
                HitItem hit = null;

                if (checkAxisForHit(item.X))
                {
                    hit = new HitItem();
                    hit.HitAxis = "X";
                    hit.HitValue = item.X;
                }

                if (checkAxisForHit(item.Y))
                {
                    hit = new HitItem();
                    hit.HitAxis = "Y";
                    hit.HitValue = item.Y;
                }

                if (checkAxisForHit(item.Z, 4000))
                {
                    hit = new HitItem();
                    hit.HitAxis = "Z";
                    hit.HitValue = item.Z;
                }

            if (hit != null)
            {
                if (CurrentHitItem != null)
                {
                    if (CurrentHitItem.HitValue < hit.HitValue)
                    {
                        hit.EventsCount = CurrentHitItem.EventsCount;
                        CurrentHitItem = hit;
                    }
                }
                else
                {
                    CurrentHitItem = hit;
                    CurrentHitItem.EventsCount = 1;
                }
            }
            else
            {
                if (CurrentHitItem != null)
                {
                    HitItems.Add(CurrentHitItem);
                    SpeakText("Зафиксирован удар по оси " + CurrentHitItem.HitAxis + " со значением " + CurrentHitItem.HitValue);
                    CurrentHitItem = null;
                }
            }
            //}
        }

        private async void SpeakText(string text)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            await synth.SpeakTextAsync(text);
        }

        /// <summary>
        /// Check for axis value for reaching hit level
        /// </summary>
        /// <param name="axisValue"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        private bool checkAxisForHit(double axisValue, double normal = 0)
        {
            if ((Math.Abs(axisValue - normal) - hitLevel) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}