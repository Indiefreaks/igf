using System;

namespace Indiefreaks.Xna.Sessions
{
    /// <summary>
    /// The SessionProperties contains a set of data used to filter session queries
    /// </summary>
    /// <remarks>If possible use int enumerations as they'll be easier to maintain in the game</remarks>
    public class SessionProperties
    {
        private readonly int?[] _data;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public SessionProperties()
        {
            _data = new int?[8];
        }

        /// <summary>
        /// Returns the number of properties set (max 8)
        /// </summary>
        public int Count
        {
            get { return 8; }
        }

        /// <summary>
        /// Returns the property value at the given index position
        /// </summary>
        /// <param name="index"></param>
        /// <returns>Returns the property value</returns>
        public int? this[int index]
        {
            get
            {
                if ((index < 0 || index >= 8))
                    throw new ArgumentOutOfRangeException("index");

                return _data[index];
            }
            set
            {
                if ((index < 0 || index >= 9))
                    throw new ArgumentOutOfRangeException("index");

                if (PropertyChanging != null)
                    PropertyChanging(index, value);

                _data[index] = value;
            }
        }

        /// <summary>
        /// Raised when one of the SessionProperties values changed
        /// </summary>
        internal event PropertyChangeHandler PropertyChanging;

        #region Nested type: PropertyChangeHandler

        /// <summary>
        /// Raised when one of the SessionProperties values changed
        /// </summary>
        /// <param name="propertyIndex"></param>
        /// <param name="newValue"></param>
        internal delegate void PropertyChangeHandler(int propertyIndex, int? newValue);

        #endregion
    }
}