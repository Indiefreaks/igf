/*
 * Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)
 * 
 * This program is licensed under the Microsoft Permissive License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://mpe.codeplex.com/license.
 */

#if WINDOWS_PHONE || XBOX
namespace System.ComponentModel
{
    /// <summary>
    /// The exception thrown when using invalid arguments that are enumerators.
    /// </summary>
    public sealed class InvalidEnumArgumentException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.InvalidEnumArgumentException
        /// class without a message.
        /// </summary>
        public InvalidEnumArgumentException()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.InvalidEnumArgumentException
        /// class with the specified message.
        /// </summary>
        /// <param name="message">The message to display with this exception. </param>
        public InvalidEnumArgumentException(String message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.InvalidEnumArgumentException
        /// class with a message generated from the argument, the invalid value, and an enumeration
        /// class.
        /// </summary>
        /// <param name="paramName">The name of the argument that caused the exception. </param>
        /// <param name="value">The value of the argument that failed.</param>
        /// <param name="enumType">A System.Type that represents the enumeration class with the
        /// valid values. </param>
        public InvalidEnumArgumentException(String paramName, Int32 value, Type enumType) : base()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.InvalidEnumArgumentException
        /// class with the specified detailed description and the specified exception.
        /// </summary>
        /// <param name="message">A detailed description of the error.</param>
        /// <param name="innerException">A reference to the inner exception that is the cause of
        /// this exception.</param>
        public InvalidEnumArgumentException(String message, Exception innerException)
        {
            throw new NotImplementedException();
        }
    }
}
#endif