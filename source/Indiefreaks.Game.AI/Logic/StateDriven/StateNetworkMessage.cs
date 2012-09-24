using System;
using Indiefreaks.Xna.Core;

namespace Indiefreaks.Xna.Logic.StateDriven
{
    /// <summary>
    /// Custom structure used to transfer State status changes over the network
    /// </summary>
    /// <typeparam name="T">A custom Enum inheriting from ushort used to differentiate states</typeparam>
    public struct StateNetworkMessage<T> where T : struct, IConvertible
    {
        private T _currentState;
        private bool _isActive;
        private bool _isTerminating;

        public StateNetworkMessage(bool isActive, bool isTerminating, T currentState)
        {
            _isActive = isActive;
            _isTerminating = isTerminating;
            _currentState = currentState;
        }

        public StateNetworkMessage(byte[] bytes)
        {
            if (bytes.Length == 0 || bytes.Length > 4)
                throw new CoreException("StateNetworkMessage too long");

            _isActive = BitConverter.ToBoolean(bytes, 0);
            _isTerminating = BitConverter.ToBoolean(bytes, 1);
            _currentState = (T) Enum.ToObject(typeof (T), BitConverter.ToUInt16(bytes, 2));
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public bool IsTerminating
        {
            get { return _isTerminating; }
            set { _isTerminating = value; }
        }

        public T CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[4];

            bytes[0] = BitConverter.GetBytes(_isActive)[0];
            bytes[1] = BitConverter.GetBytes(_isTerminating)[0];
            byte[] stateBytes = BitConverter.GetBytes((ushort) (object) _currentState);
            bytes[2] = stateBytes[0];
            bytes[3] = stateBytes[1];

            return bytes;
        }
    }
}