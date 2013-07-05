//-----------------------------------------------------------------------
// <copyright file="ElementStiffnessMatrixBuilder.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012 - 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using SharpFE;
    using SharpFE.Cache;
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ElementStiffnessMatrixBuilder<T> : IElementStiffnessCalculator
        where T : FiniteElement
    {
        protected Cache<T, StiffnessMatrix>  globalStiffnessMatrixCache = new Cache<T, StiffnessMatrix>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementStiffnessMatrixBuilder">ElementStiffnessMatrixBuilder</see> class.
        /// </summary>
        /// <param name="finiteElement"></param>
        protected ElementStiffnessMatrixBuilder(T finiteElement)
        {
            Guard.AgainstNullArgument(finiteElement, "finiteElement");
            this.Element = finiteElement;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public T Element
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// Gets the stiffness matrix of this element as rotated to global coordinates.
        /// </summary>
        public StiffnessMatrix StiffnessMatrixInGlobalCoordinates
        {
            get
            {
                return this.globalStiffnessMatrixCache.GetOrCreateAndSave(this.Element, this.Element.GetHashCode(), () => {
                                                                              return this.BuildGlobalStiffnessMatrix();
                                                                          });
            }
        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public abstract KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> ShapeFunctionVector(IFiniteElementNode locationInLocalCoordinates);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> StrainDisplacementMatrix(IFiniteElementNode location);
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract StiffnessMatrix LocalStiffnessMatrix();
        
        /// <summary>
        /// Gets the exact stiffness value for a given node and degree of freedom combinations.
        /// </summary>
        /// <param name="rowNode">The node defining the row (force equations)</param>
        /// <param name="rowDegreeOfFreedom">The degree of freedom defining the row (force equations)</param>
        /// <param name="columnNode">The node defining the column (displacement equations)</param>
        /// <param name="columnDegreeOfFreedom">the degree of freedom defining the column (displacement equations)</param>
        /// <returns>A value representing the stiffness at the given locations</returns>
        /// <exception cref="ArgumentException">Thrown if either of the nodes is not part of this element, or either of the degrees of freedom are not supported by this element.</exception>
        public double GetStiffnessInGlobalCoordinatesAt(IFiniteElementNode rowNode, DegreeOfFreedom rowDegreeOfFreedom, IFiniteElementNode columnNode, DegreeOfFreedom columnDegreeOfFreedom)
        {
            if (rowNode == null)
            {
                throw new ArgumentNullException("rowNode");
            }
            
            if (columnNode == null)
            {
                throw new ArgumentNullException("columnNode");
            }
            
            return this.StiffnessMatrixInGlobalCoordinates.At(rowNode, rowDegreeOfFreedom, columnNode, columnDegreeOfFreedom);
        }

        

        
        /// <summary>
        /// Prepares and generates the stiffness matrix.
        /// It creates an entirely new matrix from the current set of nodes and the supported degrees of freedom of this element.
        /// It calls the GenerateStiffnessMatrix method which inheriting classes are expected to implement.
        /// It sets the stiffnessMatrixHasBeenGenerated flag to true.
        /// </summary>
        protected StiffnessMatrix BuildGlobalStiffnessMatrix()
        {
            StiffnessMatrix k = this.LocalStiffnessMatrix();
            Guard.AgainstInvalidState(() => { return !k.Determinant().IsApproximatelyEqualTo(0.0); }, ///TODO calculating the determinant is computationally intensive.  We should use another method of model verification to speed this up.
                                      "The stiffness matrix for an individual element should be singular and non-invertible. i.e. it should have a zero determinant.  This is not the case for element {0} of type {1}",
                                      this.Element,
                                      this.Element.GetType());
            
            KeyedSquareMatrix<NodalDegreeOfFreedom> t = this.BuildStiffnessRotationMatrixFromLocalToGlobalCoordinates();
            
            ////FIXME these multiplications assume the keys of both matrices are ordered and identical
            KeyedSquareMatrix<NodalDegreeOfFreedom> kt = k.Multiply(t); // K*T
            KeyedSquareMatrix<NodalDegreeOfFreedom> ttransposed = t.Transpose(); // T^
            KeyedSquareMatrix<NodalDegreeOfFreedom> ttransposedkt = ttransposed.Multiply(kt); // (T^)*K*T
            StiffnessMatrix globStiffMat = new StiffnessMatrix(ttransposedkt);
            
            Guard.AgainstInvalidState(() => { return !globStiffMat.Determinant().IsApproximatelyEqualTo(0.0); }, //TODO calculating the determinant is computationally intensive.  We should use another method of model verification to speed this up.
                                      "The global stiffness matrix for an individual element should be singular and non-invertible. i.e. it should have a zero determinant.  This is not the case for element {0} of type {1}",
                                      this.Element,
                                      this.Element.GetType());
            
            return globStiffMat;
        }
        
        /// <summary>
        /// Builds the rotational matrix from local coordinates to global coordinates.
        /// </summary>
        /// <returns></returns>
        protected KeyedSquareMatrix<NodalDegreeOfFreedom> BuildStiffnessRotationMatrixFromLocalToGlobalCoordinates()
        {
            KeyedSquareMatrix<DegreeOfFreedom> rotationMatrix = this.Element.CalculateElementRotationMatrix();
            
            KeyedSquareMatrix<NodalDegreeOfFreedom> elementRotationMatrixFromLocalToGlobalCoordinates = new KeyedSquareMatrix<NodalDegreeOfFreedom>(this.Element.SupportedNodalDegreeOfFreedoms);

            foreach (IFiniteElementNode node in this.Element.Nodes)
            {
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.X), new NodalDegreeOfFreedom(node, DegreeOfFreedom.X), rotationMatrix.At(DegreeOfFreedom.X, DegreeOfFreedom.X));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.X), new NodalDegreeOfFreedom(node, DegreeOfFreedom.Y), rotationMatrix.At(DegreeOfFreedom.X, DegreeOfFreedom.Y));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.X), new NodalDegreeOfFreedom(node, DegreeOfFreedom.Z), rotationMatrix.At(DegreeOfFreedom.X, DegreeOfFreedom.Z));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.Y), new NodalDegreeOfFreedom(node, DegreeOfFreedom.X), rotationMatrix.At(DegreeOfFreedom.Y, DegreeOfFreedom.X));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.Y), new NodalDegreeOfFreedom(node, DegreeOfFreedom.Y), rotationMatrix.At(DegreeOfFreedom.Y, DegreeOfFreedom.Y));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.Y), new NodalDegreeOfFreedom(node, DegreeOfFreedom.Z), rotationMatrix.At(DegreeOfFreedom.Y, DegreeOfFreedom.Z));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.Z), new NodalDegreeOfFreedom(node, DegreeOfFreedom.X), rotationMatrix.At(DegreeOfFreedom.Z, DegreeOfFreedom.X));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.Z), new NodalDegreeOfFreedom(node, DegreeOfFreedom.Y), rotationMatrix.At(DegreeOfFreedom.Z, DegreeOfFreedom.Y));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.Z), new NodalDegreeOfFreedom(node, DegreeOfFreedom.Z), rotationMatrix.At(DegreeOfFreedom.Z, DegreeOfFreedom.Z));
                
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.XX), new NodalDegreeOfFreedom(node, DegreeOfFreedom.XX), rotationMatrix.At(DegreeOfFreedom.X, DegreeOfFreedom.X));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.XX), new NodalDegreeOfFreedom(node, DegreeOfFreedom.YY), rotationMatrix.At(DegreeOfFreedom.X, DegreeOfFreedom.Y));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.XX), new NodalDegreeOfFreedom(node, DegreeOfFreedom.ZZ), rotationMatrix.At(DegreeOfFreedom.X, DegreeOfFreedom.Z));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.YY), new NodalDegreeOfFreedom(node, DegreeOfFreedom.XX), rotationMatrix.At(DegreeOfFreedom.Y, DegreeOfFreedom.X));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.YY), new NodalDegreeOfFreedom(node, DegreeOfFreedom.YY), rotationMatrix.At(DegreeOfFreedom.Y, DegreeOfFreedom.Y));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.YY), new NodalDegreeOfFreedom(node, DegreeOfFreedom.ZZ), rotationMatrix.At(DegreeOfFreedom.Y, DegreeOfFreedom.Z));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.ZZ), new NodalDegreeOfFreedom(node, DegreeOfFreedom.XX), rotationMatrix.At(DegreeOfFreedom.Z, DegreeOfFreedom.X));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.ZZ), new NodalDegreeOfFreedom(node, DegreeOfFreedom.YY), rotationMatrix.At(DegreeOfFreedom.Z, DegreeOfFreedom.Y));
                elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, DegreeOfFreedom.ZZ), new NodalDegreeOfFreedom(node, DegreeOfFreedom.ZZ), rotationMatrix.At(DegreeOfFreedom.Z, DegreeOfFreedom.Z));
            }
            
            return elementRotationMatrixFromLocalToGlobalCoordinates;
        }
    }
}
