//-----------------------------------------------------------------------
// <copyright file="VectorExtensions.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Double;

    /// <summary>
    /// Extends the MathNet.Numerics.LinearAlgebra.Double.DenseVector class.
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Computes the cross product of two 3 dimensional vectors using the 'right-hand' rule.
        /// </summary>
        /// <param name="leftHandSide">The vector on the right hand side of the cross product statement</param>
        /// <param name="rightHandSide">The vector on the left hand side of the cross product statement</param>
        /// <returns>A vector representing the cross product of the left and right hand side.</returns>
        public static Vector CrossProduct(this Vector leftHandSide, Vector rightHandSide)
        {
            Guard.AgainstNullArgument(rightHandSide, "rightHandSide");
            Guard.AgainstBadArgument(
                () => { return !(leftHandSide.Count == 3 || leftHandSide.Count == 2); },
                "Cross product can only be carried out with a 2 or 3 dimensional vector",
                "leftHandSide");
            Guard.AgainstBadArgument(
                () => { return !(rightHandSide.Count == 2 || rightHandSide.Count == 3); },
                "Cross product can only be carried out with a 2 or 3 dimensional vector",
                "rightHandSide");
            
            Vector result = new DenseVector(3);
            result[0] = (leftHandSide[1] * rightHandSide[2]) - (leftHandSide[2] * rightHandSide[1]);
            result[1] = (leftHandSide[2] * rightHandSide[0]) - (leftHandSide[0] * rightHandSide[2]);
            result[2] = (leftHandSide[0] * rightHandSide[1]) - (leftHandSide[1] * rightHandSide[0]);
            return result;
        }
    }
}
