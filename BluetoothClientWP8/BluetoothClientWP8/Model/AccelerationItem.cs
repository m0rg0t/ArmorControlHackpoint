using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace BluetoothClientWP8.Model
{
    public class AccelerationItem: ViewModelBase
    {
        private string _message;
        /// <summary>
        /// raw message from arduino
        /// </summary>
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                try
                {
                    string[] messageArray = _message.Split(' ');
                    // Parse the complete frame before publishing any axis changes.
                    // Keep the existing numeric format/culture contract.
                    double x = Double.Parse(messageArray[0]);
                    double y = Double.Parse(messageArray[1]);
                    double z = Double.Parse(messageArray[2]);
                    // NaN/infinity are parseable doubles, but not usable sensor samples.
                    if (!Double.IsNaN(x) && !Double.IsInfinity(x) &&
                        !Double.IsNaN(y) && !Double.IsInfinity(y) &&
                        !Double.IsNaN(z) && !Double.IsInfinity(z))
                    {
                        X = x;
                        Y = y;
                        Z = z;
                    }
                }
                catch
                {
                }
                
                RaisePropertyChanged("Message");
            }
        }

        private double _x;
        /// <summary>
        /// 
        /// </summary>
        public double X
        {
            get { return _x; }
            set
            {
                _x = value;
                RaisePropertyChanged("X");
            }
        }

        private double _y;
        /// <summary>
        /// 
        /// </summary>
        public double Y
        {
            get { return _y; }
            set
            {
                _y = value;
                RaisePropertyChanged("Y");
            }
        }

        private double _z;
        /// <summary>
        /// 
        /// </summary>
        public double Z
        {
            get { return _z; }
            set
            {
                _z = value; 
                RaisePropertyChanged("Z");
            }
        }

        private DateTime _recivedDateTime = DateTime.Now;
        /// <summary>
        /// Datetime when acceleration record was recived
        /// </summary>
        public DateTime RecivedDateTime
        {
            get { return _recivedDateTime; }
            set
            {
                _recivedDateTime = value;
                RaisePropertyChanged("RecivedDateTime");
            }
        }
        
        
        
    }
}
