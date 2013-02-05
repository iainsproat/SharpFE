//-----------------------------------------------------------------------
// <copyright file="ForceVector.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "Vector is a more specific name and will be used instead of Collection")]
    public class ForceVector : KeyedVector<DegreeOfFreedom>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ForceVector" /> class.
        /// </summary>
        /// <param name="valueOfXComponent">The component of translational force along the global x-axis.</param>
        public ForceVector(double valueOfXComponent)
            : this(valueOfXComponent, 0)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ForceVector" /> class.
        /// </summary>
        /// <param name="valueOfXComponent">The component of translational force along the global x-axis.</param>
        /// <param name="valueOfYComponent">The component of translational force along the global y-axis.</param>
        public ForceVector(double valueOfXComponent, double valueOfYComponent)
            : this(valueOfXComponent, valueOfYComponent, 0)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ForceVector" /> class.
        /// </summary>
        /// <param name="valueOfXComponent">The component of translational force along the global x-axis.</param>
        /// <param name="valueOfYComponent">The component of translational force along the global y-axis.</param>
        /// <param name="valueOfZComponent">The component of translational force along the global z-axis.</param>
        public ForceVector(double valueOfXComponent, double valueOfYComponent, double valueOfZComponent)
            : this(valueOfXComponent, valueOfYComponent, valueOfZComponent, 0, 0, 0)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ForceVector" /> class.
        /// </summary>
        /// <param name="valueOfXComponent">The component of translational force along the global x-axis.</param>
        /// <param name="valueOfYComponent">The component of translational force along the global y-axis.</param>
        /// <param name="valueOfZComponent">The component of translational force along the global z-axis.</param>
        /// <param name="valueOfRotationalComponentAroundXXAxis">The component of rotational force around the global x-axis.</param>
        /// <param name="valueOfRotationalComponentAroundYYAxis">The component of rotational force around the global y-axis.</param>
        /// <param name="valueOfRotationalComponentAroundZZAxis">The component of rotational force around the global z-axis.</param>
        public ForceVector(double valueOfXComponent, double valueOfYComponent, double valueOfZComponent, double valueOfRotationalComponentAroundXXAxis, double valueOfRotationalComponentAroundYYAxis, double valueOfRotationalComponentAroundZZAxis)
            : base(new List<DegreeOfFreedom>(6){ DegreeOfFreedom.X, DegreeOfFreedom.Y, DegreeOfFreedom.Z, DegreeOfFreedom.XX, DegreeOfFreedom.YY, DegreeOfFreedom.ZZ})
        {
            this.Id = Guid.NewGuid();
            this.SetValue(DegreeOfFreedom.X, valueOfXComponent);
            this.SetValue(DegreeOfFreedom.Y, valueOfYComponent);
            this.SetValue(DegreeOfFreedom.Z, valueOfZComponent);
            this.SetValue(DegreeOfFreedom.XX, valueOfRotationalComponentAroundXXAxis);
            this.SetValue(DegreeOfFreedom.YY, valueOfRotationalComponentAroundYYAxis);
            this.SetValue(DegreeOfFreedom.ZZ, valueOfRotationalComponentAroundZZAxis);
        }
        
        public ForceVector(KeyedVector<DegreeOfFreedom> vectorToClone)
            : base(vectorToClone.Keys)
        {
            this.Id = Guid.NewGuid();
            foreach( KeyValuePair<DegreeOfFreedom, double> kvp in vectorToClone)
            {
                this.SetValue(kvp.Key, kvp.Value);
            }
        }
        #endregion
        
        /// <summary>
        /// Gets a new instance of the <see cref="ForceVector" /> class, with all component values set to zero.
        /// </summary>
        public static ForceVector Zero
        {
            get
            {
                return new ForceVector(0, 0, 0, 0, 0, 0);
            }
        }
        
        /// <summary>
        /// Gets the unique Id of this force vector.
        /// </summary>
        public Guid Id
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the component of translational force along the global x-axis.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "X", Justification = "X is a common and clear name for translation in the x-axis when used in the context of a vector")]
        public double X
        {
            get
            {
                return this[DegreeOfFreedom.X];
            }
        }
        
        /// <summary>
        /// Gets the component of translational force along the global y-axis.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Y", Justification = "Y is a common and clear name for translation in the y-axis when used in the context of a vector")]
        public double Y
        {
            get
            {
                return this[DegreeOfFreedom.Y];
            }
        }
        
        /// <summary>
        /// Gets the component of translational force along the global z-axis.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Z", Justification = "Z is a common and clear name for translation in the z-axis when used in the context of a vector")]
        public double Z
        {
            get
            {
                return this[DegreeOfFreedom.Z];
            }
        }
        
        /// <summary>
        /// Gets the component of rotational force around the global x-axis.
        /// </summary>
        public double XX
        {
            get
            {
                return this[DegreeOfFreedom.XX];
            }
        }
        
        /// <summary>
        /// Gets the component of rotational force around the global y-axis.
        /// </summary>
        public double YY
        {
            get
            {
                return this[DegreeOfFreedom.YY];
            }
        }
        
        /// <summary>
        /// Gets the component of rotational force around the global z-axis.
        /// </summary>
        public double ZZ
        {
            get
            {
                return this[DegreeOfFreedom.ZZ];
            }
        }
        
        /// <summary>
        /// Sets the value of the component of force in the given global degree of freedom.
        /// </summary>
        /// <param name="degreeOfFreedom">The degree of freedom for which the component of force should be sought.</param>
        /// <param name="value">The value of the component for the given degree of freedom</param>
        /// <remarks>The callee is responsible for ensuring the degreeOfFreedom makes sense in their particular context.</remarks>
        internal void SetValue(DegreeOfFreedom degreeOfFreedom, double value)
        {
            this[degreeOfFreedom] = value;
        }
    }
}
