namespace Bytewizer.TinyCLR.Boards.Huxley
{
    /// <summary>
    /// Notepath hardware board.
    /// </summary>
    /// <remarks>
    /// Encapsulates common initialization and access to all components.
    /// </remarks>
    public sealed class HuxleyBoard : DisposableObject
    {
        #region Lifetime

        /// <summary>
        /// Initializes a new instance of the <see cref="HuxleyBoard"/> class.
        /// </summary>
        /// <param name="productUID">Identifier is used by Notehub to associate your Notecard to your project.</param>
        public HuxleyBoard(string productUID)
        {
            ProductUID = productUID;

            try
            {
                // Create board devices
                _powerDevice = PowerDevice.Initialize();
                _clockDevice = ClockDevice.Initialize();
                _notecardDevice = NotecardDevice.Initialize();

                // Create status led devices
                _gpsDevice = GpsLedDevice.Initialize();
                _cellDevice = CellLedDevice.Initialize();
                _statusDevice = StatusLedDevice.Initialize();

                // Register device if not allready registered
                NotehubRegister();
            }
            catch
            {
                // Close devices in case partially initialized
                _gpsDevice?.Dispose();
                _cellDevice?.Dispose();
                _clockDevice?.Dispose();
                _powerDevice?.Dispose();
                _statusDevice?.Dispose();
                _notecardDevice?.Dispose();

                // Continue error
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            _gpsDevice?.Dispose();
            _cellDevice?.Dispose();
            _clockDevice?.Dispose();
            _powerDevice?.Dispose();
            _statusDevice?.Dispose();
            _notecardDevice?.Dispose();
        }

        #endregion

        #region Private Fields

        private readonly NotecardDevice _notecardDevice;

        private readonly GpsLedDevice _gpsDevice;

        private readonly CellLedDevice _cellDevice;

        private readonly StatusLedDevice _statusDevice;

        private readonly ClockDevice _clockDevice;

        private readonly PowerDevice _powerDevice;

        #endregion

        #region Private Methods   

        public bool NotehubRegister()
        {            
            if (!_notecardDevice.IsRegistered(ProductUID))
            {                  
                return _notecardDevice.Register(ProductUID);
            }

            return false;
        }

        #endregion

        #region Public Properties

        public string ProductUID { get; private set; }

        public NotecardDevice Notecard => _notecardDevice;

        public GpsLedDevice GpsLed => _gpsDevice;

        public CellLedDevice CellularLed => _cellDevice;

        public StatusLedDevice StatusLed => _statusDevice;

        public ClockDevice Clock => _clockDevice;

        #endregion
    }
}
