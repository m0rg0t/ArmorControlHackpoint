using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.Speech.Synthesis;
using GalaSoft.MvvmLight;

namespace BluetoothClientWP8.Model
{
    public class HitItem: ViewModelBase
    {
        private double _hitValue;
        /// <summary>
        /// Find hit value
        /// </summary>
        public double HitValue
        {
            get { return _hitValue; }
            set
            {
                _hitValue = value;
                RaisePropertyChanged("HitItem");
            }
        }

        private int _eventsCount = 0;
        /// <summary>
        /// Количество событий
        /// </summary>
        public int EventsCount
        {
            get { return _eventsCount; }
            set
            {
                _eventsCount = value;
                RaisePropertyChanged("EventsCount");
            }
        }
        

        private DateTime _hitDateTime = DateTime.Now;
        /// <summary>
        /// Date when hit was found
        /// </summary>
        public DateTime HitDateTime
        {
            get { return _hitDateTime; }
            set
            {
                _hitDateTime = value;
                RaisePropertyChanged("HitDateTime");
            }
        }

        private string _hitAxis;
        /// <summary>
        /// Axis at which we found hit
        /// </summary>
        public string HitAxis
        {
            get { return _hitAxis; }
            set
            {
                _hitAxis = value;
                RaisePropertyChanged("HitAxis");
            }
        }
        
    }
}
