/*
 * Copyright Iain Sproat, 2013
 * User: ispro
 * Date: 03/07/2013
 * 
 */
using System;

namespace SharpFE
{
    /// <summary>
    /// Extension methods for System.Double
    /// </summary>
    public static class DoubleExtension
    {
        /// <summary>
        /// Equality comparison using double.epsilon.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="other"></param>
        /// <returns>True if the two values are within double.epsilon of each other.</returns>
        public static bool IsApproximatelyEqualTo(this double lhs, double other)
        {
            return lhs.IsApproximatelyEqualTo(other, double.Epsilon);
        }
        
        /// <summary>
        /// Equality comparison using a tolerance.
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="other"></param>
        /// <returns>True if the two values are within tolerance of each other.</returns>
        public static bool IsApproximatelyEqualTo(this double lhs, double other, double tolerance)
        {
            //tolerance must always be zero or positive
            if (tolerance < 0)
            {
                tolerance *= -1.0;
            }
            
            double diff = lhs - other;
            return diff < tolerance && diff > -tolerance;
        }
    }
}
