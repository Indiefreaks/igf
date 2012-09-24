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
    using System.Diagnostics;

    /// <summary>
    /// Defines methods for validating arguments passed into methods.
    /// </summary>
    static internal class Check
    {
        /// <summary>
        /// Performs a check against an argument, and throws a <see cref="System.ArgumentNullException"/>
        /// if it is null.
        /// </summary>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        [Conditional("DEBUG")]
        static public void ArgumentNotNull(String parameter, Object argument)
        {
            if (argument == null)
                throw new ArgumentNullException(parameter);
        }

        /// <summary>
        /// Performs a check against a String argument, and throws an <see cref="System.ArgumentNullException"/>
        /// if it is null or empty.
        /// </summary>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        [Conditional("DEBUG")]
        static public void ArgumentNotNullOrEmpty(String parameter, String argument)
        {
            if (String.IsNullOrEmpty(argument))
                throw new ArgumentNullException(parameter);
        }

        /// <summary>
        /// Performs a check against a method argument, and throws an <see cref="System.ArgumentOutOfRangeException"/>
        /// if it is less than the specified threshold.
        /// </summary>
        /// <typeparam name="T">The type of argument being validated.</typeparam>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        /// <param name="threshold">The threshold value that the argument must be equal to or greater than
        /// to pass the test.</param>
        [Conditional("DEBUG")]
        static public void ArgumentNotLessThan<T>(String parameter, T argument, T threshold) where T : IComparable<T>
        {
            if (argument.CompareTo(threshold) < 0)
                throw new ArgumentOutOfRangeException(parameter);
        }

        /// <summary>
        /// Performs a check against a method argument, and throws an <see cref="System.ArgumentOutOfRangeException"/>
        /// if it is not greater than the specified threshold.
        /// </summary>
        /// <typeparam name="T">The type of argument being validated.</typeparam>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        /// <param name="threshold">The threhold value which <paramref name="argument"/> must be greater than.</param>
        [Conditional("DEBUG")]
        static public void ArgumentGreaterThan<T>(String parameter, T argument, T threshold) where T : IComparable<T>
        {
            if (argument.CompareTo(threshold) < 0)
                throw new ArgumentOutOfRangeException(parameter);
        }

        /// <summary>
        /// Performs a check against a method argument, and throws an <see cref="System.ArgumentOutOfRangeException"/>
        /// if it is greater than the specified threshold.
        /// </summary>
        /// <typeparam name="T">The type of argument being validated.</typeparam>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        /// <param name="threshold">The threshold value that the argument must be equal to or less than
        /// to pass the test.</param>
        [Conditional("DEBUG")]
        static public void ArgumentNotGreaterThan<T>(String parameter, T argument, T threshold) where T : IComparable<T>
        {
            if (argument.CompareTo(threshold) > 0)
                throw new ArgumentOutOfRangeException(parameter);
        }

        /// <summary>
        /// Performs a check against a method argument, and throws an <see cref="System.ArgumentOutOfRangeException"/>
        /// if it is greater than the specified maximum value, or less than the specified minimum value.
        /// </summary>
        /// <typeparam name="T">The type of argument being validated.</typeparam>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        /// <param name="min">The minimum allowed value (inclusive).</param>
        /// <param name="max">The maximum allowed value (inclusive).</param>
        [Conditional("DEBUG")]
        static public void ArgumentWithinRange<T>(String parameter, T argument, T min, T max) where T : IComparable<T>
        {
            if ((argument.CompareTo(min) < 0) || (argument.CompareTo(max) > 0))
                throw new ArgumentOutOfRangeException(parameter);
        }

        /// <summary>
        /// Performs a check against a method argument, and throws a <see cref="NotFiniteNumberException"/>
        /// if it is not a finite number eg NaN, PositiveInfinity or NegetiveInfinity.
        /// </summary>
        /// <param name="parameter">The name of the method parameter.</param>
        /// <param name="argument">The value being passed as an argument.</param>
        [Conditional("DEBUG")]
        static public void ArgumentFinite(String parameter, Single argument)
        {
            if (Single.IsNaN(argument))
#if WINDOWS
                throw new NotFiniteNumberException((double)argument);
#elif XBOX || WINDOWS_PHONE
                throw new NotFiniteNumberException();
#endif
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the specified expression is true.
        /// </summary>
        /// <param name="expression">A Booleanean expression.</param>
        /// <param name="message">The error message if the expression is true.</param>
        [Conditional("DEBUG")]
        static public void False(Boolean expression, String message)
        {
            if (expression == true)
                throw new InvalidOperationException(message);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the specified expression is false.
        /// </summary>
        /// <param name="expression">A Booleanean expression.</param>
        /// <param name="message">The error message if the expression is false.</param>
        [Conditional("DEBUG")]
        static public void True(Boolean expression, String message)
        {
            if (expression == false)
                throw new InvalidOperationException(message);
        }
    }
}