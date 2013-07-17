//-----------------------------------------------------------------------
// <copyright file="ElementStiffnessMatrixBuilder.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012 - 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    
    using MathNet.Numerics.LinearAlgebra.Double;
    
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
        public abstract DenseVector ShapeFunctions(XYZ locationInLocalCoordinates);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationInLocalCoordinates"></param>
        /// <returns></returns>
        public abstract DenseMatrix ShapeFunctionFirstDerivatives(XYZ locationInLocalCoordinates);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">Location on the finite element in local coordinates (xi, mu).  The z-axis property of the location is ignored.</param>
        /// <returns></returns>
        protected DenseMatrix Jacobian(XYZ locationInLocalCoordinates)
        {
            DenseMatrix firstDerivs = this.ShapeFunctionFirstDerivatives(locationInLocalCoordinates);
            DenseMatrix nodeCoords = this.NodeCoordinatesAsMatrix();
            
            return (DenseMatrix)firstDerivs.TransposeThisAndMultiply(nodeCoords);
        }
        
        protected DenseMatrix NodeCoordinatesAsMatrix()
        {
            int numNodes = this.Element.Nodes.Count;
            DenseMatrix nodeCoords = new DenseMatrix(numNodes, 2);
            IFiniteElementNode currentNode;
            for (int i = 0; i < numNodes; i++)
            {
                currentNode = this.Element.Nodes[i];
                nodeCoords[i, 0] = currentNode.X;
                nodeCoords[i, 1] = currentNode.Y;
            }
            
            return nodeCoords;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> StrainDisplacementMatrix(XYZ locationInLocalCoordinates);
        
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
            Guard.AgainstNullArgument(rowNode, "rowNode");
            Guard.AgainstNullArgument(columnNode, "columnNode");
            
            try
            {
                return this.StiffnessMatrixInGlobalCoordinates.At(rowNode, rowDegreeOfFreedom, columnNode, columnDegreeOfFreedom);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return 0.0;
            }
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
            
            
            KeyedSquareMatrix<NodalDegreeOfFreedom> t = this.BuildStiffnessRotationMatrixFromLocalToGlobalCoordinates();
            
            k = new StiffnessMatrix(t.RowKeys, t.ColumnKeys, k); //pad out local stiffness matrix with zeros so that it is the same size as the rotational matrix
            Guard.AgainstInvalidState(() => { return !k.Determinant().IsApproximatelyEqualTo(0.0); }, ///TODO calculating the determinant is computationally intensive.  We should use another method of model verification to speed this up.
                                      "The stiffness matrix for an individual element should be singular and non-invertible. i.e. it should have a zero determinant.  This is not the case for element {0} of type {1}",
                                      this.Element,
                                      this.Element.GetType());
            
            ////FIXME these multiplications assume the keys of both matrices are ordered and identical
            KeyedSquareMatrix<NodalDegreeOfFreedom> kt = k.Multiply(t); // K*T
            KeyedSquareMatrix<NodalDegreeOfFreedom> ttransposedkt = t.TransposeThisAndMultiply(kt); // (T^)*K*T
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
            
            KeyedSquareMatrix<NodalDegreeOfFreedom> elementRotationMatrixFromLocalToGlobalCoordinates = new KeyedSquareMatrix<NodalDegreeOfFreedom>(this.Element.SupportedGlobalNodalDegreeOfFreedoms);

            foreach (IFiniteElementNode node in this.Element.Nodes)
            {
                foreach (DegreeOfFreedom dofi in this.Element.SupportedGlobalBoundaryConditionDegreeOfFreedom)
                {
                    foreach (DegreeOfFreedom dofj in this.Element.SupportedGlobalBoundaryConditionDegreeOfFreedom)
                    {
                        if (!((dofi.IsLinear() && dofj.IsLinear()) || (dofi.IsRotational() && dofj.IsRotational())))
                        {
                            continue;
                        }
                        
                        //HACK this is a big pile of horribly un-clean code. it copies x into a larger matrix of [x 0, 0 x]
                        DegreeOfFreedom i = dofi;
                        DegreeOfFreedom j = dofj;
                        if (dofi.IsRotational() && dofj.IsRotational())
                        {
                            // shift from rotational to linear
                            i = (DegreeOfFreedom)((int)dofi - 3);
                            j = (DegreeOfFreedom)((int)dofj - 3);
                        }
                        
                        double valueToBeCopied = rotationMatrix.At(i, j);
                        elementRotationMatrixFromLocalToGlobalCoordinates.At(new NodalDegreeOfFreedom(node, dofi), new NodalDegreeOfFreedom(node, dofj), valueToBeCopied);
                    }
                }
            }
            
            return elementRotationMatrixFromLocalToGlobalCoordinates;
        }
    }
}
