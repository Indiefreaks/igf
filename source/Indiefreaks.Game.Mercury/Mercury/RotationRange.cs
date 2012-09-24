/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

namespace ProjectMercury
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Defines a closed range of rotation on each axis.
    /// </summary>
    [Serializable]
    [TypeConverter("ProjectMercury.Design.RotationRangeTypeConverter, ProjectMercury.Design, Version=4.0.0.0")]
    public struct RotationRange : IEquatable<RotationRange>, IFormattable
    {
        /// <summary>
        /// Gets or sets the yaw component of the rotation range.
        /// </summary>
        public Range Yaw;

        /// <summary>
        /// Gets or sets the pitch component of the rotation range.
        /// </summary>
        public Range Pitch;

        /// <summary>
        /// Gets or sets the roll component of the rotation range.
        /// </summary>
        public Range Roll;

        /// <summary>
        /// Creates a new rotation range by parsing three ISO 31-11 String representions of closed intervals.
        /// </summary>
        /// <param name="value">Input String value.</param>
        /// <returns>A new rotation range value.</returns>
        static public RotationRange Parse(String value)
        {
            return RotationRange.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates a new rotation range by parsing three ISO 31-11 String representations of closed intervals.
        /// </summary>
        /// <param name="value">Input String value.</param>
        /// <param name="format">The format provider.</param>
        /// <returns>A new rotation range value.</returns>
        /// <remarks>Example of a well formed value: <i>"[0,1];[0,1];[0,1]"</i>.</remarks>
        static public RotationRange Parse(String value, IFormatProvider format)
        {
            Check.ArgumentNotNullOrEmpty("value", value);
            Check.ArgumentNotNull("format", format);

            String[] components = value.Split(';');

            if (components.Length != 3)
                goto badformat;

            return new RotationRange
            {
                Yaw = Range.Parse(components[0], format),
                Pitch = Range.Parse(components[1], format),
                Roll = Range.Parse(components[2], format)
            };

        badformat:
            throw new FormatException("Value is not in the correct format.");
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise <c>false</c>.
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            if (obj != null)
                if (obj is RotationRange)
                    return this.Equals((RotationRange)obj);

            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="RotationRange"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="RotationRange"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="RotationRange"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public Boolean Equals(RotationRange value)
        {
            return this.Yaw.Equals(value.Yaw) && 
                   this.Pitch.Equals(value.Pitch) &&                   
                   this.Roll.Equals(value.Roll);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override Int32 GetHashCode()
        {
            return this.Yaw.GetHashCode() ^ 
                   this.Pitch.GetHashCode() ^                   
                   this.Roll.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override String ToString()
        {
            return this.ToString("G", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public String ToString(IFormatProvider formatProvider)
        {
            return this.ToString("G", formatProvider);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public String ToString(String format, IFormatProvider formatProvider)
        {
#if WINDOWS
            return String.Join(";", this.Yaw.ToString(format, formatProvider), 
                                    this.Pitch.ToString(format, formatProvider),                                    
                                    this.Roll.ToString(format, formatProvider));
#else
            return String.Join(";", new String[]
            {
                this.Yaw.ToString(format, formatProvider),
                this.Pitch.ToString(format, formatProvider),
                this.Roll.ToString(format, formatProvider)
            });
#endif
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="x">The lvalue.</param>
        /// <param name="y">The rvalue.</param>
        /// <returns>
        /// 	<c>true</c> if the lvalue <see cref="RotationRange"/> is equal to the rvalue; otherwise, <c>false</c>.
        /// </returns>
        static public Boolean operator ==(RotationRange x, RotationRange y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="x">The lvalue.</param>
        /// <param name="y">The rvalue.</param>
        /// <returns>
        /// 	<c>true</c> if the lvalue <see cref="RotationRange"/> is not equal to the rvalue; otherwise, <c>false</c>.
        /// </returns>
        static public Boolean operator !=(RotationRange x, RotationRange y)
        {
            return !x.Equals(y);
        }

        /// <summary>
        /// Implicit case operator from Vector3 to Range.
        /// </summary>
        /// <param name="value">A vector containing three floating point values.</param>
        /// <returns>A new RotationRange object.</returns>
        static public implicit operator RotationRange(Vector3 value)
        {
            return new RotationRange
            {
                Yaw = (Range)value.X,
                Pitch = (Range)value.Y,
                Roll = (Range)value.Z
            };
        }
    }
}