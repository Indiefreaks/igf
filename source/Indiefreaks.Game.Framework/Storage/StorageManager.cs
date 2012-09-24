using System;
using System.Collections.Generic;
using System.Globalization;
using Indiefreaks.Xna.Core;
using Microsoft.Xna.Framework;

namespace Indiefreaks.Xna.Storage
{
    public class StorageManager : GameComponent
    {
        private Dictionary<PlayerIndex, IAsyncSaveDevice> _playerDevices;
        private IAsyncSaveDevice _sharedDevice;

        public StorageManager(Application application) : base(application)
        {
            if (application == null)
                throw new ArgumentNullException("application", "StorageManager requires a valid application instance");

            if (Application.Storage != null)
                throw new CoreException("There already is an StorageManager instance.");
            application.Components.Add(this);
            application.Services.AddService(typeof (StorageManager), this);

            Strings.Culture = new CultureInfo(CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower());
        }

        public IAsyncSaveDevice SharedDevice
        {
            get { return _sharedDevice; }
        }

        public IAsyncSaveDevice PlayerOne
        {
            get { return _playerDevices[PlayerIndex.One]; }
        }

        public IAsyncSaveDevice PlayerTwo
        {
            get { return _playerDevices[PlayerIndex.Two]; }
        }

        public IAsyncSaveDevice PlayerThree
        {
            get { return _playerDevices[PlayerIndex.Three]; }
        }

        public IAsyncSaveDevice PlayerFour
        {
            get { return _playerDevices[PlayerIndex.Four]; }
        }

        public override void Initialize()
        {
            base.Initialize();

#if WINDOWS || XBOX
            var shared = new SharedSaveDevice();

            shared.DeviceSelectorCanceled += (s, e) => e.Response = SaveDeviceEventResponse.Force;
            shared.DeviceDisconnected += (s, e) => e.Response = SaveDeviceEventResponse.Force;

            _sharedDevice = shared;
#else
            _sharedDevice = new IsolatedStorageSaveDevice();
#endif

            _playerDevices = new Dictionary<PlayerIndex, IAsyncSaveDevice>();


            for (int i = 0; i < 4; i++)
            {
                var playerIndex = (PlayerIndex) Enum.ToObject(typeof (PlayerIndex), i);

#if WINDOWS || XBOX
                var playerDevice = new PlayerSaveDevice(playerIndex);
                
                playerDevice.DeviceSelectorCanceled += (s, e) => e.Response = SaveDeviceEventResponse.Force;
                playerDevice.DeviceDisconnected += (s, e) => e.Response = SaveDeviceEventResponse.Force;
#else
                var playerDevice = new IsolatedStorageSaveDevice();
#endif

                _playerDevices.Add(playerIndex, playerDevice);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_sharedDevice is IUpdate)
                ((IUpdate) _sharedDevice).Update(gameTime);

            foreach (var playerDevice in _playerDevices.Values)
            {
                if(playerDevice is IUpdate)
                    ((IUpdate)playerDevice).Update(gameTime);
            }
        }

        public void PrepareSharedDevice()
        {
#if WINDOWS || XBOX
            ((SharedSaveDevice) _sharedDevice).PromptForDevice();
#endif
        }

        public IAsyncSaveDevice PreparePlayerDevice(PlayerIndex playerIndex)
        {
            IAsyncSaveDevice playerDevice = _playerDevices[playerIndex];

#if WINDOWS || XBOX
            ((PlayerSaveDevice) playerDevice).PromptForDevice();
#endif

            return playerDevice;
        }
    }
}