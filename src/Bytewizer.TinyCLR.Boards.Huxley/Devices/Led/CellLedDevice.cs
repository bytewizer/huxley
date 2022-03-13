using GHIElectronics.TinyCLR.Pins;

namespace Bytewizer.TinyCLR.Boards.Huxley
{
    public class CellLedDevice : GpioLed
    {
        public CellLedDevice(int pinNumber)
            : base(pinNumber)
        {
        }

        public static CellLedDevice Initialize()
        {
            // Create device
            var device = new CellLedDevice(SC13048.GpioPin.PB4);

            // Return initialized device
            return device;
        }
    }
}