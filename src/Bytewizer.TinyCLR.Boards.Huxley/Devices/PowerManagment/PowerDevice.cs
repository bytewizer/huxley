using System.Diagnostics;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Native;
using GHIElectronics.TinyCLR.Devices.Gpio;

namespace Bytewizer.TinyCLR.Boards.Huxley
{
    public class PowerDevice : DisposableObject
    {
        private readonly GpioPin _attnPin;
        private readonly GpioController _controller;
        
        public PowerDevice(int pinNumber)
        {
            _controller = GpioController.GetDefault();
            _attnPin = _controller.OpenPin(pinNumber);
            _attnPin.SetDriveMode(GpioPinDriveMode.InputPullDown);
            _attnPin.ValueChanged += AttnPin_ValueChanged;
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

            _controller?.Dispose();
            _attnPin?.Dispose();
        }

        /// <summary>
        /// Creates an initialized instance of the device configured with default settings.
        /// </summary>
        public static PowerDevice Initialize()
        {
            var device = new PowerDevice(SC13048.GpioPin.PH3);

            return device;
        }

        public void Sleep()
        {
            Power.Sleep();
        }

        private static void AttnPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e) 
        {
            Debug.WriteLine(sender.ToString());
        }
    }
}