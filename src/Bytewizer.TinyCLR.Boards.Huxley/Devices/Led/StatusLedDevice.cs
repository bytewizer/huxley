using GHIElectronics.TinyCLR.Pins;

namespace Bytewizer.TinyCLR.Boards.Huxley
{
    public class StatusLedDevice : GpioLed
    {
        public StatusLedDevice(int pinNumber)
            : base(pinNumber)
        {
        }

        public static StatusLedDevice Initialize()
        {
            // Create device
            var device = new StatusLedDevice(SC13048.GpioPin.PB5);

            // Return initialized device
            return device;
        }
    }
}