using GHIElectronics.TinyCLR.Pins;

namespace Bytewizer.TinyCLR.Boards.Huxley
{
    public class GpsLedDevice : GpioLed
    {
        public GpsLedDevice(int pinNumber)
            : base(pinNumber)
        {
        }

        public static GpsLedDevice Initialize()
        {
            // Create device
            var device = new GpsLedDevice(SC13048.GpioPin.PB3);

            // Return initialized device
            return device;
        }
    }
}