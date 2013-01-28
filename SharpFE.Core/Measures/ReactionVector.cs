//-----------------------------------------------------------------------
// <copyright file="ReactionVector.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Double;

    /// <summary>
    /// ReactionVector are the forces at a node which are calculated in an analysis.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Vector is more specific and will be used instead of Collection")]
    public class ReactionVector : ForceVector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactionVector" /> class.
        /// </summary>
        /// <param name="locationNode">The node at which these reaction forces occur.</param>
        public ReactionVector(IFiniteElementNode locationNode)
            : this(locationNode, 0, 0)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactionVector" /> class.
        /// </summary>
        /// <param name="locationNode">The node at which these reaction forces occur.</param>
        /// <param name="valueOfXComponent">The component of translational force along the global x-axis.</param>
        /// <param name="valueOfYComponent">The component of translational force along the global y-axis.</param>
        public ReactionVector(IFiniteElementNode locationNode, double valueOfXComponent, double valueOfYComponent)
            : base(valueOfXComponent, valueOfYComponent)
        {
            this.Location = locationNode;
        }
        
        /// <summary>
        /// Gets the node at which these reactions occur.
        /// </summary>
        public IFiniteElementNode Location
        {
            get;
            private set;
        }
    }
}
