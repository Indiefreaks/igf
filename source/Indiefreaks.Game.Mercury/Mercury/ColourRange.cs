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
    /// Defines a closed range of colour.
    /// </summary>
    [Serializable]
    [TypeConverter("ProjectMercury.Design.ColourRangeTypeConverter, ProjectMercury.Design, Version=4.0.0.0")]
    public struct ColourRange : IEquatable<ColourRange>, IFormattable
    {
        /// <summary>
        /// Gets or sets the red component of the colour range.
        /// </summary>
        public Range Red;

        /// <summary>
        /// Gets or sets the green component of the colour range.
        /// </summary>
        public Range Green;

        /// <summary>
        /// Gets or sets the green component of the colour range.
        /// </summary>
        public Range Blue;

        /// <summary>
        /// Creates a new colour range by parsing three ISO 31-11 String representions of closed intervals.
        /// </summary>
        /// <param name="value">Input String value.</param>
        /// <returns>A new colour range value.</returns>
        static public ColourRange Parse(String value)
        {
            return ColourRange.Parse(value, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates a new colour range by parsing three ISO 31-11 String representations of closed intervals.
        /// </summary>
        /// <param name="value">Input String value.</param>
        /// <param name="format">The format provider.</param>
        /// <returns>A new colour range value.</returns>
        /// <remarks>Example of a well formed value: <i>"[0,1];[0,1];[0,1]"</i>.</remarks>
        static public ColourRange Parse(String value, IFormatProvider format)
        {
            Check.ArgumentNotNullOrEmpty("value", value);
            Check.ArgumentNotNull("format", format);

            String[] components = value.Split(';');

            if (components.Length != 3)
                goto badformat;

            return new ColourRange
            {
                Red = Range.Parse(components[0], format),
                Green = Range.Parse(components[1], format),
                Blue = Range.Parse(components[2], format)
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
                if (obj is ColourRange)
                    return this.Equals((ColourRange)obj);

            return false;
        }

        /// <summary>
        /// Determines whether the specified <see cref="ColourRange"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="ColourRange"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="ColourRange"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public Boolean Equals(ColourRange value)
        {
            return this.Red.Equals(value.Red) &&
                   this.Green.Equals(value.Green) &&
                   this.Blue.Equals(value.Blue);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override Int32 GetHashCode()
        {
            return this.Red.GetHashCode() ^
                   this.Green.GetHashCode() ^
                   this.Blue.GetHashCode();
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
            return String.Join(";", new String[]
            {
                this.Red.ToString(format, formatProvider),
                this.Green.ToString(format, formatProvider),
                this.Blue.ToString(format, formatProvider)
            });
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="x">The lvalue.</param>
        /// <param name="y">The rvalue.</param>
        /// <returns>
        /// 	<c>true</c> if the lvalue <see cref="ColourRange"/> is equal to the rvalue; otherwise, <c>false</c>.
        /// </returns>
        static public Boolean operator ==(ColourRange x, ColourRange y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="x">The lvalue.</param>
        /// <param name="y">The rvalue.</param>
        /// <returns>
        /// 	<c>true</c> if the lvalue <see cref="ColourRange"/> is not equal to the rvalue; otherwise, <c>false</c>.
        /// </returns>
        static public Boolean operator !=(ColourRange x, ColourRange y)
        {
            return !x.Equals(y);
        }

        /// <summary>
        /// Implicit cast operator from Vector3 to ColourRange.
        /// </summary>
        /// <param name="value">A vector containing three floating point values.</param>
        /// <returns>A new ColourRange object.</returns>
        static public implicit operator ColourRange(Vector3 value)
        {
            return new ColourRange
            {
                Red   = (Range)value.X,
                Green = (Range)value.Y,
                Blue  = (Range)value.Z
            };
        }
    }
}