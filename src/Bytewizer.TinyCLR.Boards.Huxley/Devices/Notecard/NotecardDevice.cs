using System;
using System.Text;
using System.Threading;

using GHIElectronics.TinyCLR.Pins;
using GHIElectronics.TinyCLR.Native;
using GHIElectronics.TinyCLR.Data.Json;
using GHIElectronics.TinyCLR.Devices.I2c;

using Bytewizer.TinyCLR.Drivers.Blues.Notecard;
using Bytewizer.TinyCLR.Boards.Huxley.Models;

namespace Bytewizer.TinyCLR.Boards.Huxley
{
    /// <summary>
    /// The Blues Wireless Notecard cellular data pump device. 
    /// </summary>
    public class NotecardDevice : DisposableObject
    {

        private readonly NotecardController _notecard;

        #region Constants

        /// <summary>
        /// Time to wait for the device to startup in milliseconds.
        /// </summary>
        public const int StartupDelay = 200;

        /// <summary>
        /// Time to wait for the <see cref="Restart"/> command to complete in millisecond.
        /// </summary>
        public const int RestartDelay = 5000;

        #endregion

        #region Lifetime

        /// <summary>
        /// Creates an instance using the specified controller.
        /// </summary>
        /// <param name="controller">The I2C controller.</param>
        public NotecardDevice(I2cController controller)
        {
            // Initialize hardware

            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            _notecard = new NotecardController(controller);
        }

        /// <summary>
        /// Creates an initialized instance of the device configured with default settings.
        /// </summary>
        public static NotecardDevice Initialize()
        {
            var controller = I2cController.FromName(SC13048.I2cBus.I2c1);
            var device = new NotecardDevice(controller);

            return device;
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

            _notecard?.Dispose();
        }

        #endregion

        #region Public Properties


        #endregion

        #region Public Methods

        public bool IsRegistered(string productUID)
        {
            var request = new JsonRequest("hub.get");

            var results = _notecard.Request(request);
            if (results.IsSuccess &&
                results.Response.Contains(productUID))
            {
                return true;
            }

            return false;
        }

        public bool IsConnected()
        {
            var request = new JsonRequest("card.status");

            var results = _notecard.Request(request);
            if (results.IsSuccess &&
                results.Response.Contains("{normal}"))
            {
                return true;
            }

            return false;
        }

        public bool IsCellActive()
        {
            var request = new JsonRequest("card.wireless");

            var results = _notecard.Request(request);
            if (results.IsSuccess &&
                results.Response.Contains("{network-up}"))
            {
                return true;
            }

            return false;
        }

        public bool IsGpsActive()
        {
            var request = new JsonRequest("card.location");

            var results = _notecard.Request(request);
            if (results.IsSuccess
                && results.Response.Contains("{gps-inactive} {gps}"))
            {
                return true;
            }

            return false;
        }

        public bool Restart()
        {
            var request = new JsonRequest("card.restart");

            if (_notecard.Request(request).IsSuccess)
            {
                Thread.Sleep(RestartDelay);
                return true;
            }

            return false;
        }

        public bool Restore()
        {
            var request = new JsonRequest("card.restore");
            request.Add("delete", true);
            request.Add("connected", true);

            if (_notecard.Request(request).IsSuccess)
            {
                Thread.Sleep(RestartDelay * 2);
                return true;
            }

            return false;
        }

        public bool Register(string productUID)
        {
            JsonRequest request;
            
            try
            {
                var sn = BitConverter.ToString(DeviceInformation.GetUniqueId());

                request = new JsonRequest("hub.set");
                request.Add("product", productUID);
                request.Add("sn", sn);
                request.Add("mode", "periodic");
                request.Add("voutbound", "usb:30;high:180;normal:360;low:1440;dead:0");
                request.Add("vinbound", "usb:30;high:360;normal:720;low:1440;dead:0");

                _notecard.Request(request).EnsureSuccess();

                // Set battery type
                request = new JsonRequest("card.voltage");
                request.Add("mode", "lipo");

                _notecard.Request(request).EnsureSuccess();

                // Sample GPS location
                request = new JsonRequest("card.location.mode");
                request.Add("mode", "periodic");
                request.Add("seconds", 60);
                //request.Add("vseconds", "usb:1800;high:3600;normal:7200;low:43200;dead:0");

                _notecard.Request(request).EnsureSuccess();

                // Start gathering tracking events
                request = new JsonRequest("card.location.track");
                request.Add("start", true);
                request.Add("heartbeat", true);
                request.Add("hours", 12);
                request.Add("file", "_track.qo");
                request.Add("sync", true);

                _notecard.Request(request).EnsureSuccess();

                // Set motion attn interrupt
                request = new JsonRequest("card.attn");
                request.Add("mode", "arm,motion");

                _notecard.Request(request).EnsureSuccess();

                return true;
            }
            catch 
            { 
                return false; 
            }
        }

        public bool Sync()
        {
            var request = new JsonRequest("hub.sync");

            return _notecard.Request(request).IsSuccess;
        }

        public bool EnableSleep()
        {
            var request = new JsonRequest("card.attn");
            request.Add("mode", "sleep");
            request.Add("seconds", 30);

            return _notecard.Request(request).IsSuccess;
        }

        public bool EnableTrace()
        {
            var request = new JsonRequest("card.trace");
            request.Add("mode", "on");

            return _notecard.Request(request).IsSuccess;
        }
        public bool DisableTrace()
        {
            var request = new JsonRequest("card.trace");
            request.Add("mode", "off");

            return _notecard.Request(request).IsSuccess;
        }

        public bool EnableMotion()
        {
            var request = new JsonRequest("card.motion.mode");
            request.Add("start", true);

            return _notecard.Request(request).IsSuccess;
        }

        public bool DisableMotion()
        {
            var request = new JsonRequest("card.motion.mode");
            request.Add("stop", true);

            return _notecard.Request(request).IsSuccess;
        }

        public bool ArmMotion()
        {
            var request = new JsonRequest("card.attn");
            request.Add("mode", "arm,motion");

            return _notecard.Request(request).IsSuccess;
        }

        public CardTimeModel CardTime()
        {
            var request = new JsonRequest("card.time");
            return (CardTimeModel)_notecard.Request(request, typeof(CardTimeModel));
        }

        public CardLocationModel CardLocation()
        {
            var request = new JsonRequest("card.location");
            return (CardLocationModel)_notecard.Request(request, typeof(CardLocationModel));
        }

        public CardWirelessModel CardWireless()
        {
            var request = new JsonRequest("card.wireless");
            return (CardWirelessModel)_notecard.Request(request, typeof(CardWirelessModel));
        }

        #endregion
    }
}