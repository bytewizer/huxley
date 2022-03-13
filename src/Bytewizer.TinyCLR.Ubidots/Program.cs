using System.Diagnostics;
using System.Threading;

using Bytewizer.TinyCLR.Boards.Huxley;

namespace Bytewizer.TinyCLR.Ubidots
{
    internal class Program
    {
        private static Timer _timerStatus;
        private static HuxleyBoard _mainboard;
        
        static void Main()
        {
            _mainboard = new HuxleyBoard("com.bytewizer.trice:huxley");
            
            // Blink device for 5 seconds to show device is on
            _mainboard.StatusLed.Blink(200, 200, 5000);

            _timerStatus = new Timer(OnTimer, null, 0, 1000);

            Thread.Sleep(Timeout.Infinite);
        }

        private static void OnTimer(object sender)
        {
            if (_mainboard.Notecard.IsCellActive())
            {
                _mainboard.CellularLed.On();
            }
            else
            {
                _mainboard.CellularLed.Off();
            }

            if (_mainboard.Notecard.IsGpsActive())
            {
                _mainboard.GpsLed.Off();
            }
            else
            {
                _mainboard.GpsLed.Blink(200, 200);
            }

            var connection = _mainboard.Notecard.CardWireless();
            var location = _mainboard.Notecard.CardLocation();
            Debug.WriteLine($"{ location.status } / { location.lat } / { location.lon } / { location.time} / {connection.status}");
        }
    }
}