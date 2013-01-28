//-----------------------------------------------------------------------
// <copyright file="DisplacementVector.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Double;

    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Vector is more specific and is used instead of Collection")]
    public class DisplacementVector : DenseVector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplacementVector" /> class.
        /// </summary>
        /// <param name="locationNode">The node to which this displacement occurs.</param>
        public DisplacementVector(IFiniteElementNode locationNode)
            : this(locationNode, 0, 0)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplacementVector" /> class.
        /// </summary>
        /// <param name="locationNode">The node to which this displacement occurs.</param>
        /// <param name="valueInXComponent">The component of the displacement along the global x-axis</param>
        /// <param name="valueInYComponent">The component of the displacement along the global y-axis</param>
        public DisplacementVector(IFiniteElementNode locationNode, double valueInXComponent, double valueInYComponent)
            : base(6)
        {
            this.Location = locationNode;
            this.At(0, valueInXComponent);
            this.At(1, valueInYComponent);
        }
        
        /// <summary>
        /// Gets the node to which this displacement occurs.
        /// </summary>
        public IFiniteElementNode Location
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the component of translation along the global X-axis.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X", Justification = "X is the common nomenclature for translation along the x-axis")]
        public double X
        {
            get { return this.GetValue(DegreeOfFreedom.X); }
        }
        
        /// <summary>
        /// Gets the component of translation along the global Y-axis.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y", Justification = "Y is the common nomenclature for translation along the y-axis")]
        public double Y
        {
            get { return this.GetValue(DegreeOfFreedom.Y); }
        }
        
        /// <summary>
        /// Gets the component of translation along the global Z-axis.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Z", Justification = "Z is the common nomenclature for translation along the z-axis")]
        public double Z
        {
            get { return this.GetValue(DegreeOfFreedom.Z); }
        }
        
        /// <summary>
        /// Gets the component of translation around the XX-axis.
        /// Units are radians.
        /// </summary>
        public double XX
        {
            get { return this.GetValue(DegreeOfFreedom.XX); }
        }
        
        /// <summary>
        /// Gets the component of translation around the XX-axis.
        /// Units are radians.
        /// </summary>
        public double YY
        {
            get { return this.GetValue(DegreeOfFreedom.YY); }
        }
        
        /// <summary>
        /// Gets the component of translation around the XX-axis.
        /// Units are radians.
        /// </summary>
        public double ZZ
        {
            get { return this.GetValue(DegreeOfFreedom.ZZ); }
        }
        
        /// <summary>
        /// Gets the component of displacement in the given degree of freedom.
        /// </summary>
        /// <param name="degreeOfFreedom">The degree of freedom for the component we wish to retrieve.</param>
        /// <returns>A double representing the value of the component of displacement in the given degree of freedom.</returns>
        /// <remarks>The callee is responsible for ensuring the degreeOfFreedom makes sense in their particular context.</remarks>
        public double GetValue(DegreeOfFreedom degreeOfFreedom)
        {
            return this.At((int)degreeOfFreedom);
        }
        
        /// <summary>
        /// Sets the component of displacement in the given degree of freedom.
        /// </summary>
        /// <param name="degreeOfFreedom">The degree of freedom indicating the component to add the value to.</param>
        /// <param name="value">The value in one component of the vector</param>
        /// <remarks>The callee is responsible for ensuring the degreeOfFreedom makes sense in their particular context.</remarks>
        public void SetValue(DegreeOfFreedom degreeOfFreedom, double value)
        {
            this.At((int)degreeOfFreedom, value);
        }
    }
}
