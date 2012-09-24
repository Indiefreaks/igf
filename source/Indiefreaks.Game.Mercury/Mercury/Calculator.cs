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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Encapsulates common mathematical functions.
    /// </summary>
    static public class Calculator
    {
        /// <summary>
        /// Represents the value of pi.
        /// </summary>
        public const Single Pi = 3.141593f;

        /// <summary>
        /// Represents the value of pi multiplied by two.
        /// </summary>
        public const Single TwoPi = 6.283185f;

        /// <summary>
        /// Represents the value of pi divided by two.
        /// </summary>
        public const Single PiOver2 = 1.570796f;

        /// <summary>
        /// Represents the value of pi divided by four.
        /// </summary>
        public const Single PiOver4 = 0.7853982f;

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="min">The minimum value. If <paramref name="value"/> is less than <paramref name="min"/>, <paramref name="min"/> will be returned.</param>
        /// <param name="max">The maximum value. If <paramref name="value"/> is greater than <paramref name="max"/>, <paramref name="max"/> will be returned.</param>
        /// <returns>The clamped value.</returns>
        static public Single Clamp(Single value, Single min, Single max)
        {
            value = (value < min) ? min : value;
            value = (value > max) ? max : value;

            return value;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to be clamped.</param>
        /// <param name="min">The minimum value. If <paramref name="value"/> is less than <paramref name="min"/>, <paramref name="min"/> will be returned.</param>
        /// <param name="max">The maximum value. If <paramref name="value"/> is greater than <paramref name="max"/>, <paramref name="max"/> will be returned.</param>
        /// <param name="output">The output value.</param>
        static public void Clamp(Single value, Single min, Single max, out Single output)
        {
            output = (value > max) ? max : value;
            output = (value < min) ? min : value;
        }
#if UNSAFE
        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">A pointer to the value to be clamped.</param>
        /// <param name="min">The minimum value. If <paramref name="value"/> is less than <paramref name="min"/>, <paramref name="min"/> will be returned.</param>
        /// <param name="max">The maximum value. If <paramref name="value"/> is greater than <paramref name="max"/>, <paramref name="max"/> will be returned.</param>
        static public unsafe void Clamp(Single* value, Single min, Single max)
        {
            *value = (*value > max) ? max : *value;
            *value = (*value < min) ? min : *value;
        }
#endif
        /// <summary>
        /// Wraps a value to within a specified range.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The wrapped value.</returns>
        static public Single Wrap(Single value, Single min, Single max)
        {
            return ((value - min) % (max - min)) + min;
        }

        /// <summary>
        /// Wraps a value to within a specified range.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="output">The output value.</param>
        static public void Wrap(Single value, Single min, Single max, out Single output)
        {
            output = ((value - min) % (max - min)) + min;
        }
#if UNSAFE
        /// <summary>
        /// Wraps a value to be within a specified range.
        /// </summary>
        /// <param name="value">A pointer to the value to be wrapped.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        static public unsafe void Wrap(Single* value, Single min, Single max)
        {
            *value = ((*value - min) % (max - min)) + min;
        }
#endif
        /// <summary>
        /// Interpolates between two values using a linear curve.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">A value between zero and one indicating the weight of <paramref name="value2"/></param>
        /// <returns>The Interpolated value.</returns>
        static public Single LinearInterpolate(Single value1, Single value2, Single amount)
        {
            return value1 + ((value2 - value1) * amount);
        }

        /// <summary>
        /// Interpolates between two values using a linear curve.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">A value between zero and one indicating the weight of <paramref name="value2"/></param>
        /// <param name="output">The output value.</param>
        static public void LinearInterpolate(Single value1, Single value2, Single amount, out Single output)
        {
            output = value1 + ((value2 - value1) * amount);
        }
#if UNSAFE
        /// <summary>
        /// Interpolates between two values using a linear curve.
        /// </summary>
        /// <param name="value">A pointer to a value which will receive the result of the interpolation.</param>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">A value between zero and one indicating the weight of <paramref name="value2"/></param>
        static public unsafe void LinearInterpolate(Single* value, Single value1, Single value2, Single amount)
        {
            *value = value1 + ((value2 - value1) * amount);
        }
#endif
        /// <summary>
        /// Interpolates between three values using a linear curve, where the position of the middle source value is variable.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value2Position">The position of the second source value between zero and one.</param>
        /// <param name="value3">Source value.</param>
        /// <param name="amount">A value between zero and one indicating the position on the curve to evaluate.</param>
        /// <remarks>The Interpolated value.</remarks>
        static public Single LinearInterpolate(Single value1, Single value2, Single value2Position, Single value3, Single amount)
        {
            if (amount < value2Position)
                return Calculator.LinearInterpolate(value1, value2, amount / value2Position);

            else
                return Calculator.LinearInterpolate(value2, value3, (amount - value2Position) / (1f - value2Position));
        }

        /// <summary>
        /// Interpolates between three values using a linear curve, where the position of the middle source value is variable.
        /// </summary>
        /// <param name="output">The output parameter.</param>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value2Position">The position of the second source value between zero and one.</param>
        /// <param name="value3">Source value.</param>
        /// <param name="amount">A value between zero and one indicating the position on the curve to evaluate.</param>
        static public void LinearInterpolate(Single value1, Single value2, Single value2Position, Single value3, Single amount, out Single output)
        {
            if (amount < value2Position)
                output = Calculator.LinearInterpolate(value1, value2, amount / value2Position);

            else
                output = Calculator.LinearInterpolate(value2, value3, (amount - value2Position) / (1f - value2Position));
        }
#if UNSAFE
        /// <summary>
        /// Interpolates between three values using a linear curve, where the position of the middle source value is variable.
        /// </summary>
        /// <param name="value">A pointer to a value which will receive the result of the interpolation.</param>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value2Position">The position of the second source value between zero and one.</param>
        /// <param name="value3">Source value.</param>
        /// <param name="amount">A value between zero and one indicating the position on the curve to evaluate.</param>
        static public unsafe void LinearInterpolate(Single* value, Single value1, Single value2, Single value2Position, Single value3, Single amount)
        {
            if (amount < value2Position)
                Calculator.LinearInterpolate(value, value1, value2, amount / value2Position);

            else
                Calculator.LinearInterpolate(value, value2, value3, (amount - value2Position) / (1f - value2Position));
        }
#endif
        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">A value between zero and one indicating the weight of <paramref name="value2"/></param>
        /// <returns>The Interpolated value.</returns>
        static public Single CubicInterpolate(Single value1, Single value2, Single amount)
        {
#if UNSAFE
            unsafe { Calculator.Clamp(&amount, 0f, 1f); }
#else
            amount = Calculator.Clamp(amount, 0f, 1f);
#endif
            return Calculator.LinearInterpolate(value1, value2, (amount * amount) * (3f - (2f * amount)));
        }

        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">A value between zero and one indicating the weight of <paramref name="value2"/></param>
        /// <param name="output">The result of the calculation</param>
        static public void CubicInterpolate(Single value1, Single value2, Single amount, out Single output)
        {
#if UNSAFE
            unsafe { Calculator.Clamp(&amount, 0f, 1f); }
#else
            amount = Calculator.Clamp(amount, 0f, 1f);
#endif
            output = Calculator.LinearInterpolate(value1, value2, (amount * amount) * (3f - (2f * amount)));
        }
#if UNSAFE
        /// <summary>
        /// Interpolates between two values using a cubic equation.
        /// </summary>
        /// <param name="value">A pointer to a value which will receive the result of the interpolation.</param>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="amount">A value between zero and one indicating the weight of <paramref name="value2"/></param>
        static public unsafe void CubicInterpolate(Single* value, Single value1, Single value2, Single amount)
        {
            Calculator.Clamp(&amount, 0f, 1f);

            Calculator.LinearInterpolate(value, value1, value2, (amount * amount) * (3f - (2f * amount)));
        }
#endif
        /// <summary>
        /// Returns the greater of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The greater value.</returns>
        static public Single Max(Single value1, Single value2)
        {
            return value1 >= value2 ? value1 : value2;
        }

        /// <summary>
        /// Returns the greater of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="output">The output value.</param>
        static public void Max(Single value1, Single value2, out Single output)
        {
            output = value1 >= value2 ? value1 : value2;
        }

        /// <summary>
        /// Returns the greater of three values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value3">Source value.</param>
        /// <returns>The greater value.</returns>
        static public Single Max(Single value1, Single value2, Single value3)
        {
            return value2 >= value3 ? (value1 >= value2 ? value1 : value2) : (value1 >= value3 ? value1 : value3);
        }

        /// <summary>
        /// Returns the greater of three values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value3">Source value.</param>
        /// <param name="output">The output value.</param>
        static public void Max(Single value1, Single value2, Single value3, out Single output)
        {
            output = value2 >= value3 ? (value1 >= value2 ? value1 : value2) : (value1 >= value3 ? value1 : value3);
        }

        /// <summary>
        /// Returns the lesser of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <returns>The lesser value.</returns>
        static public Single Min(Single value1, Single value2)
        {
            return value1 <= value2 ? value1 : value2;
        }

        /// <summary>
        /// Returns the lesser of two values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="output">The output value.</param>
        static public void Min(Single value1, Single value2, out Single output)
        {
            output = value1 <= value2 ? value1 : value2;
        }

        /// <summary>
        /// Returns the lesser of three values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value3">Source value.</param>
        /// <returns>The lesser value.</returns>
        static public Single Min(Single value1, Single value2, Single value3)
        {
            return value2 <= value3 ? (value1 <= value2 ? value1 : value2) : (value1 <= value3 ? value1 : value3);
        }

        /// <summary>
        /// Returns the lesser of three values.
        /// </summary>
        /// <param name="value1">Source value.</param>
        /// <param name="value2">Source value.</param>
        /// <param name="value3">Source value.</param>
        /// <param name="output">The output value.</param>
        static public void Min(Single value1, Single value2, Single value3, out Single output)
        {
            output = value2 <= value3 ? (value1 <= value2 ? value1 : value2) : (value1 <= value3 ? value1 : value3);
        }

        /// <summary>
        /// Returns the absolute value of a single precision Single point value.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <returns>The absolute value of <paramref name="value"/></returns>
        static public Single Abs(Single value)
        {
            return value >= 0f ? value : -value;
        }

        /// <summary>
        /// Returns the absolute value of a single precision Single point value.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <param name="output">Output value.</param>
        static public void Abs(Single value, out Single output)
        {
            output = value >= 0f ? value : -value;
        }
#if UNSAFE
        /// <summary>
        /// Sets a pointer to a single precision floating point value to be the absolute value of itself.
        /// </summary>
        /// <param name="value">A pointer to a single precision floating point value.</param>
        static public unsafe void Abs(Single* value)
        {
            *value = *value >= 0f ? *value : -*value;
        }
#endif
        /// <summary>
        /// Returns the angle whose cosine is the specified value.
        /// </summary>
        /// <param name="value">A number representing a cosine.</param>
        /// <returns>The angle whose cosine is the specified value.</returns>
        static public Single Acos(Single value)
        {
            return (Single)Math.Acos((double)value);
        }

        /// <summary>
        /// Returns the angle whose sine is the specified value.
        /// </summary>
        /// <param name="value">A number representing a sine.</param>
        /// <returns>The angle whose sine is the specified value.</returns>
        static public Single Asin(Single value)
        {
            return (Single)Math.Asin((double)value);
        }

        /// <summary>
        /// Returns the angle whos tangent is the speicified number.
        /// </summary>
        /// <param name="value">A number representing a tangent.</param>
        /// <returns>The angle whos tangent is the speicified number.</returns>
        static public Single Atan(Single value)
        {
            return (Single)Math.Atan((double)value);
        }

        /// <summary>
        /// Returns the angle whose tangent is the quotient of the two specified numbers.
        /// </summary>
        /// <param name="y">The y coordinate of a point.</param>
        /// <param name="x">The x coordinate of a point.</param>
        /// <returns>The angle whose tangent is the quotient of the two specified numbers.</returns>
        static public Single Atan2(Single y, Single x)
        {
            return (Single)Math.Atan2((double)y, (double)x);
        }

        /// <summary>
        /// Returns the sine of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The sine of the specified angle.</returns>
        static public Single Sin(Single value)
        {
            return (Single)Math.Sin((double)value);
        }

        /// <summary>
        /// Returns the hyperbolic sine of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The hyperbolic sine of the specified angle.</returns>
        static public Single Sinh(Single value)
        {
            return (Single)Math.Sinh((double)value);
        }

        /// <summary>
        /// Returns the cosine of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The cosine of the specified angle.</returns>
        static public Single Cos(Single value)
        {
            return (Single)Math.Cos((double)value);
        }

        /// <summary>
        /// Returns the hyperbolic cosine of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The hyperbolic cosine of the specified angle.</returns>
        static public Single Cosh(Single value)
        {
            return (Single)Math.Cosh((double)value);
        }

        /// <summary>
        /// Returns the tangent of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The tangent of the specified angle.</returns>
        static public Single Tan(Single value)
        {
            return (Single)Math.Tan((double)value);
        }

        /// <summary>
        /// Returns the hyperbolic tangent of the specified angle.
        /// </summary>
        /// <param name="value">An angle specified in radians.</param>
        /// <returns>The hyperbolic tangent of the specified angle.</returns>
        static public Single Tanh(Single value)
        {
            return (Single)Math.Tanh((double)value);
        }

        /// <summary>
        /// Returns the natural (base e) logarithm of the specified value.
        /// </summary>
        /// <param name="value">A number whose logarithm is to be found.</param>
        /// <returns>The natural (base e) logarithm of the specified value.</returns>
        static public Single Log(Single value)
        {
            return (Single)Math.Log((double)value);
        }

        /// <summary>
        /// Returns the specified value raised to the specified power.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <param name="power">A single precision floating point number that specifies a power.</param>
        /// <returns>The specified value raised to the specified power.</returns>
        static public Single Pow(Single value, Single power)
        {
            return (Single)Math.Pow((double)value, (double)power);
        }

        /// <summary>
        /// Returns the square root of the specified value.
        /// </summary>
        /// <param name="value">Source value.</param>
        /// <returns>The square root of the specified value.</returns>
        static public Single Sqrt(Single value)
        {
            return (Single)Math.Sqrt((double)value);
        }
    }
}