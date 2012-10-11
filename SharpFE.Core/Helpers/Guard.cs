//-----------------------------------------------------------------------
// <copyright file="FiniteElement.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SharpFE
{
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
        public static void AgainstBadArgument(ArgumentComparator testForFailure, string failureMessage, string parameterName)
        {
            if (testForFailure())
            {
                throw new ArgumentException(failureMessage, parameterName);
            }
        }
    }
}
