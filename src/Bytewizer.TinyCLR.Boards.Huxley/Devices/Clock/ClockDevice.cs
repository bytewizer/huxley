using GHIElectronics.TinyCLR.Native;
using GHIElectronics.TinyCLR.Devices.Rtc;

namespace Bytewizer.TinyCLR.Boards.Huxley
{
    public class ClockDevice : DisposableObject
    {
        private readonly RtcController _controller;
        
        public ClockDevice()
        {
            _controller = RtcController.GetDefault();
            _controller.SetChargeMode(BatteryChargeMode.Fast);

            if (_controller.IsValid)
            {
                SystemTime.SetTime(_controller.Now);
            }    
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
        }

        /// <summary>
        /// Creates an initialized instance of the device configured with default settings.
        /// </summary>
        public static ClockDevice Initialize()
        {
            var device = new ClockDevice();

            return device;
        }
    }
}