//-----------------------------------------------------------------------
// <copyright file="LinearConstantStressQuadrilateralStiffnessMatrixBuilder.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;
    
    using MathNet.Numerics.LinearAlgebra.Generic;
    using MathNet.Numerics.LinearAlgebra.Double;
    
    using SharpFE.Geometry;
    using SharpFE.Materials;

    /// <summary>
    /// </summary>
    public class LinearConstantStressQuadrilateralStiffnessMatrixBuilder : ElementStiffnessMatrixBuilder<LinearConstantStressQuadrilateral>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public LinearConstantStressQuadrilateralStiffnessMatrixBuilder(LinearConstantStressQuadrilateral element)
            : base(element)
        {
            MaterialMatrixBuilder materialMatrixBuilder = new MaterialMatrixBuilder(this.Element.Material); //TODO should have a cache so only one matrix builder is created for each material used in the model
            this.MaterialMatrix = materialMatrixBuilder.PlaneStressMatrix(this.SupportedStrains);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private IList<Strain> SupportedStrains
        {
            get
            {
                IList<Strain> strains = new List<Strain>(3);
                strains.Add(Strain.LinearStrainX);
                strains.Add(Strain.LinearStrainY);
                strains.Add(Strain.ShearStrainXY);
                return strains;
            }
        }
        
        protected KeyedSquareMatrix<Strain> MaterialMatrix
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override DenseVector ShapeFunctions(XYZ locationInNaturalCoordinates)
        {
            Guard.AgainstNullArgument(locationInNaturalCoordinates, "locationInNaturalCoordinates");
            
            double xi = locationInNaturalCoordinates.X;
            double eta = locationInNaturalCoordinates.Y;
            Guard.AgainstBadArgument("locationInNaturalCoordinates",
                                     () => { return xi < -1 || xi > 1 || eta < -1 || eta > 1; },
                                     "location in natural coordinates must be within the boundary -1 to +1. You provided {0}", locationInNaturalCoordinates);

            DenseVector shapeFunctions = new DenseVector(4);
            
            shapeFunctions[0] = 0.25 * (1 - xi) * (1 - eta);
            shapeFunctions[1] = 0.25 * (1 + xi) * (1 - eta);
            shapeFunctions[2] = 0.25 * (1 + xi) * (1 + eta);
            shapeFunctions[3] = 0.25 * (1 - xi) * (1 + eta);

            return shapeFunctions;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override DenseMatrix ShapeFunctionFirstDerivatives(XYZ locationInNaturalCoordinates)
        {
            Guard.AgainstNullArgument(locationInNaturalCoordinates, "locationInNaturalCoordinates");
            double xi = locationInNaturalCoordinates.X;
            double eta = locationInNaturalCoordinates.Y;
            Guard.AgainstBadArgument("locationInNaturalCoordinates",
                                     () => { return xi < -1 || xi > 1 || eta < -1 || eta > 1; },
                                     "location in natural coordinates must be within the boundary -1 to +1. You provided {0}", locationInNaturalCoordinates);
            

            DenseMatrix shapeFunctionDerivatives = new DenseMatrix(4, 2);

            shapeFunctionDerivatives.At(0, 0, -0.25 * (1 - eta));
            shapeFunctionDerivatives.At(0, 1, -0.25 * (1 - xi));
            shapeFunctionDerivatives.At(1, 0,  0.25 * (1 - eta));
            shapeFunctionDerivatives.At(1, 1, -0.25 * (1 + xi));
            shapeFunctionDerivatives.At(2, 0,  0.25 * (1 + eta));
            shapeFunctionDerivatives.At(2, 1,  0.25 * (1 + xi));
            shapeFunctionDerivatives.At(3, 0, -0.25 * (1 + eta));
            shapeFunctionDerivatives.At(3, 1,  0.25 * (1 - xi));

            return shapeFunctionDerivatives;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location">Location on the finite element in local coordinates (xi, mu).  The z-axis property of the location is ignored.</param>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> StrainDisplacementMatrix(XYZ locationInNaturalCoordinates)
        {
            DenseMatrix AG = A_Matrix(locationInNaturalCoordinates) * G_Matrix(locationInNaturalCoordinates);
            
            IList<Strain> supportedStrains = this.SupportedStrains;
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedLocalNodalDegreeOfFreedoms;
            
            
            return new KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom>(supportedStrains, supportedNodalDegreesOfFreedom, AG);
        }
        
        protected DenseMatrix A_Matrix(XYZ locationInNaturalCoordinates) //TODO make the output a keyed matrix
        {
            DenseMatrix jacobian = this.Jacobian(locationInNaturalCoordinates);
            double J11 = jacobian[0, 0];
            double J12 = jacobian[0, 1];
            double J21 = jacobian[1, 0];
            double J22 = jacobian[1, 1];
            double detJ = jacobian.Determinant();
            Guard.AgainstInvalidState(() => {
                                          return detJ.IsApproximatelyEqualTo(0);
                                      }, "The determinant of the Jacobian is zero.  The Jacobian is {0}", jacobian);
            
            DenseMatrix A = new DenseMatrix(3, 4);
            
            //linear strain x
            A[0, 0] =  J22;
            A[0, 1] = -J12;
            
            //linear strain y
            A[1, 2] = -J21;
            A[1, 3] =  J11;
            
            //shear strain xy
            A[2, 0] = -J21;
            A[2, 1] =  J11;
            A[2, 2] =  J22;
            A[2, 3] = -J12;
            
            A = (DenseMatrix)A.Divide(detJ);
            return A;
        }
        
        protected DenseMatrix G_Matrix(XYZ locationInNaturalCoordinates) //TODO make the output a keyed matrix
        {
            int numNodes = this.Element.Nodes.Count;
            DenseMatrix shapeFunctionDerivs = this.ShapeFunctionFirstDerivatives(locationInNaturalCoordinates);
            
            DenseMatrix G = new DenseMatrix(4, numNodes * 2);
            
            double xiDeriv, etaDeriv;
            int currentColumn;
            for (int i = 0; i < numNodes; i++)
            {
                xiDeriv = shapeFunctionDerivs[i, 0];
                etaDeriv = shapeFunctionDerivs[i, 1];
                
                currentColumn = 2 * i;
                G.At(0, currentColumn, xiDeriv);
                G.At(1, currentColumn, etaDeriv);
                
                currentColumn += 1;
                G.At(2, currentColumn, xiDeriv);
                G.At(3, currentColumn, etaDeriv);
            }
            
            return G;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override StiffnessMatrix LocalStiffnessMatrix()
        {
            KeyedSquareMatrix<Strain> materialMatrix = this.MaterialMatrix;
            
            SharpFE.Maths.Integration.Gauss.GaussianIntegration2D gaussianIntegrator = new SharpFE.Maths.Integration.Gauss.GaussianIntegration2D(2);
            
            Matrix<double> k = gaussianIntegrator.Integrate((gaussIntegrationPointi, gaussIntegrationPointj) => {
                            CartesianPoint pnt = new CartesianPoint(gaussIntegrationPointi, gaussIntegrationPointj, 0);
                            
                            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> strainDisplacementMatrix = this.StrainDisplacementMatrix(pnt);
                            KeyedRowColumnMatrix<NodalDegreeOfFreedom, Strain> bte = strainDisplacementMatrix.TransposeThisAndMultiply<Strain>(materialMatrix);
                            KeyedRowColumnMatrix<NodalDegreeOfFreedom, NodalDegreeOfFreedom> bteb = bte.Multiply<Strain, NodalDegreeOfFreedom>(strainDisplacementMatrix);
                            
                            double jacobianDeterminant = this.Jacobian(pnt).Determinant();
                            bteb = bteb.Multiply(jacobianDeterminant);
                            return (Matrix<double>)bteb;
                        });
            
            double thickness = this.Element.Thickness; //TODO interpolate over element (i.e. allow varying thickness over element)
            k = k.Multiply(thickness);
            
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedLocalNodalDegreeOfFreedoms;
            return new StiffnessMatrix(supportedNodalDegreesOfFreedom, supportedNodalDegreesOfFreedom, k);
        }
    }
}
