//-----------------------------------------------------------------------
// <copyright file="LinearConstantStrainTriangleStiffnessMatrixBuilder.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;
    
    using MathNet.Numerics.LinearAlgebra.Double;
    
    using SharpFE.Materials;
    
    /// <summary>
    /// </summary>
    public class LinearConstantStrainTriangleStiffnessMatrixBuilder : ElementStiffnessMatrixBuilder<LinearConstantStrainTriangle>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public LinearConstantStrainTriangleStiffnessMatrixBuilder(LinearConstantStrainTriangle element)
            : base(element)
        {
            MaterialMatrixBuilder materialMatrixBuilder = new MaterialMatrixBuilder(this.Element.Material); //TODO should have a cache so only one matrix builder is created for each material used in the model
            this.MaterialMatrix = materialMatrixBuilder.PlaneStrainMatrix(this.SupportedStrains);
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected IList<Strain> SupportedStrains
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
        public override DenseVector ShapeFunctions(XYZ locationInLocalCoordinates)
        {
            throw new NotImplementedException("LinearConstantStrainTriangleStiffnessMatrixBuilder.ShapeFunctions has been implemented, but uses global coordinates (x, y) rather than natural coordinates (xi, mu)");

            Guard.AgainstNullArgument(locationInLocalCoordinates, "locationInLocalCoordinates");
            double xi = locationInLocalCoordinates.X;
            double eta = locationInLocalCoordinates.Y;
            Guard.AgainstBadArgument("locationInLocalCoordinates",
                                     () => { return xi < -1 || xi > 1 || eta < -1 || eta > 1; },
                                     "location in local coordinates must be within the boundary -1 to +1. You provided {0}", locationInLocalCoordinates);
            
            DenseVector shapeFunctions = new DenseVector(3);
            
            IFiniteElementNode node0 = this.Element.Nodes[0];
            IFiniteElementNode node1 = this.Element.Nodes[1];
            IFiniteElementNode node2 = this.Element.Nodes[2];
            
            double constant = 1.0 / (2.0 * this.Element.Area);
            double n1 = ((node1.X * node2.Y) - (node2.X * node1.Y))
                + ((node1.Y - node2.Y) * locationInLocalCoordinates.X)
                + ((node2.X - node1.X) * locationInLocalCoordinates.Y);
            
            double n2 = ((node2.X * node0.Y) - (node0.X * node2.Y))
                + ((node2.Y - node1.Y) * locationInLocalCoordinates.X)
                + ((node0.X - node2.X) * locationInLocalCoordinates.Y);
            
            double n3 = ((node0.X * node1.Y) - (node1.X * node0.Y))
                + ((node0.Y - node1.Y) * locationInLocalCoordinates.X)
                + ((node1.X - node0.X) * locationInLocalCoordinates.Y);
            
            shapeFunctions[0] = constant * n1;
            
            shapeFunctions[1] = constant * n2;
            
            shapeFunctions[2] = constant * n3;

            return shapeFunctions;
        
        }
        
        public override DenseMatrix ShapeFunctionFirstDerivatives(XYZ locationInLocalCoordinates)
        {
            throw new NotImplementedException("LinearconstantStrainTriangleStiffnessMatrix.ShapeFunctionFirstDerivatives");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> StrainDisplacementMatrix(XYZ locationInLocalCoordinates)
        {
            IList<Strain> supportedStrains = this.SupportedStrains;
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedLocalNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> strainDisplacementMatrix = new KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom>(supportedStrains, supportedNodalDegreesOfFreedom);
            
            IFiniteElementNode node0 = this.Element.Nodes[0];
            IFiniteElementNode node1 = this.Element.Nodes[1];
            IFiniteElementNode node2 = this.Element.Nodes[2];
            
            double constant = 1.0 / (2.0 * this.Element.Area);
            
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * (node1.Y - node2.Y));
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * (node2.Y - node0.Y));
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * (node0.Y - node1.Y));
            
            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * (node2.X - node1.X));
            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * (node0.X - node2.X));
            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * (node1.X - node0.X));
            
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * (node2.X - node1.X));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * (node1.Y - node2.Y));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * (node0.X - node2.X));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * (node2.Y - node0.Y));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * (node1.X - node0.X));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * (node0.Y - node1.Y));
            
            return strainDisplacementMatrix;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override StiffnessMatrix LocalStiffnessMatrix()
        {
            double elementVolume = this.Element.Thickness * this.Element.Area;
            KeyedSquareMatrix<Strain> materialMatrix = this.MaterialMatrix;
            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> strainDisplacementMatrix = this.StrainDisplacementMatrix(null);
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, Strain> transposedStrainDisplacementMatrix = strainDisplacementMatrix.Transpose();
            
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, Strain> bte = transposedStrainDisplacementMatrix.Multiply<Strain, Strain>(materialMatrix);
            
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, NodalDegreeOfFreedom> bteb = bte.Multiply<Strain, NodalDegreeOfFreedom>(strainDisplacementMatrix);
            
            StiffnessMatrix k = new StiffnessMatrix(bteb.Multiply(elementVolume));
            return k;
        }
    }
}
