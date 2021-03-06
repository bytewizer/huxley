using System;
using System.Threading;

using GHIElectronics.TinyCLR.Devices.Gpio;

namespace Bytewizer.TinyCLR.Boards.Huxley
{
    public abstract class GpioLed : DisposableObject
    {
        private int _timeOn;
        private int _timeOff;

        private Timer _timer;
        private Thread _thread;
        private readonly GpioPin _ledPin;

        protected GpioLed(int pinNumber)
        {
            var gpio = GpioController.GetDefault();

            _ledPin = gpio.OpenPin(pinNumber);
            _ledPin.SetDriveMode(GpioPinDriveMode.Output);
            _ledPin.Write(GpioPinValue.Low);
        }


        /// <summary>
        /// <see cref="DisposableObject.Dispose(bool)"/>.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _ledPin?.Dispose();
        }

        public bool Active { get; private set; } = false;

        public GpioPinValue State
        {
            get { return _ledPin.Read(); }
            set { _ledPin.Write(value); }
        }

        public void On()
        {
            _ledPin.Write(GpioPinValue.High);
        }

        public void Off()
        {
            StopBlinking();

            _ledPin.Write(GpioPinValue.Low);
        }

        public void Blink(int timeOn, int timeOff, int interval)
        {
            if (interval > 0)
            {
                _timer = new Timer(sender =>
                {
                    StopBlinking();
                }, null, interval, -1);
            }

            Blink(timeOn, timeOff);
        }

        public void Blink(int timeOn, int timeOff)
        {
            _timeOff = timeOff;
            _timeOn = timeOn;

            if (Active)
            {
                return;
            }

            Active = true;

            _thread = new Thread(() =>
            {
                while (Active)
                {
                    var ret = _ledPin.Read();
                    if (ret == GpioPinValue.High)
                    {
                        _ledPin.Write(GpioPinValue.Low);
                        Thread.Sleep(_timeOff);
                    }
                    else
                    {
                        _ledPin.Write(GpioPinValue.High);
                        Thread.Sleep(_timeOn);
                    }
                }
            });

            _thread.Start();
        }

        private void StopBlinking()
        {
            if (!Active)
            {
                return;
            }

            Active = false;

            if (_thread != null)
            {
                _thread = null;
            }

            _ledPin.Write(GpioPinValue.Low);
        }
    }
}