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
        public static bool IsApproximatelyEqualTo(this double lhs, double other)
        {
            double diff = lhs - other;
            return diff < double.Epsilon && diff > -double.Epsilon;
        }
    }
}
