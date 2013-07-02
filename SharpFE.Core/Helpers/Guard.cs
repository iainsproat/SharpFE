//-----------------------------------------------------------------------
// <copyright file="Guard.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Returns a boolean value based on internal statements to the delegate.
    /// </summary>
    /// <returns>true if the internal statements of the delegate wish it so.</returns>
    internal delegate bool ArgumentComparator();
    
    /// <summary>
    /// Guard provides a number of methods which check for bad variable states.
    /// This may include null parameters etc..
    /// All methods of Guard will throw an exception on finding a bad variable.
    /// </summary>
    internal class Guard
    {
        /// <summary>
        /// Throws an exception if the parameter is null.
        /// </summary>
        /// <param name="parameter">The object to check is not null.</param>
        /// <param name="parameterName">The name of the parameter within the calling method.</param>
        /// <exception cref="ArgumentNullException">Thrown if the parameter is null.</exception>
        public static void AgainstNullArgument(object parameter, string parameterName)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
        
        /// <summary>
        /// Throws an exception if the parameter is null.
        /// </summary>
        /// <param name="parameter">The object to check is not null.</param>
        /// <param name="parameterName">The name of the parameter within the calling method.</param>
        /// <param name="customErrorMessage">A custom error message.</param>
        /// <exception cref="ArgumentNullException">Thrown if the parameter is null.</exception>
        public static void AgainstNullArgument(object parameter, string parameterName, string customErrorMessage)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName, customErrorMessage);
            }
        }
        
        /// <summary>
        /// Throws an exception if the testForFailure is true.
        /// </summary>
        /// <param name="testForFailure">A comparator which returns true if this method is to throw an error</param>
        /// <param name="failureMessage">A custom message to provide with the exception.</param>
        /// <param name="parameterName">The name of the parameter within the calling method.</param>
        public static void AgainstBadArgument(string parameterName, ArgumentComparator testForFailure, string failureMessage, params object[] failureMessageFormatItems)
        {
            if (testForFailure())
            {
                throw new ArgumentException(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    failureMessage,
                    failureMessageFormatItems),
                                            parameterName);
            }
        }
        
        public static void AgainstInvalidState(ArgumentComparator testForFailure, string failureMessage, params object[] failureMessageFormatItems)
        {
            if (testForFailure())
            {
                throw new InvalidOperationException(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    failureMessage,
                    failureMessageFormatItems));
            }
        }
    }
}
