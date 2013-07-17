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
        public override DenseVector ShapeFunctions(XYZ locationInNaturalCoordinates)
        {
            Guard.AgainstNullArgument(locationInNaturalCoordinates, "locationInNaturalCoordinates");
            double xi = locationInNaturalCoordinates.X;
            double eta = locationInNaturalCoordinates.Y;
            Guard.AgainstBadArgument("locationInNaturalCoordinates",
                                     () => { return xi < -1 || xi > 1 || eta < -1 || eta > 1; },
                                     "location in local coordinates must be within the boundary -1 to +1. You provided {0}", locationInNaturalCoordinates);
            
            DenseVector shapeFunctions = new DenseVector(3);
            
            IFiniteElementNode node0 = this.Element.Nodes[0];
            IFiniteElementNode node1 = this.Element.Nodes[1];
            IFiniteElementNode node2 = this.Element.Nodes[2];
            
            IDictionary<IFiniteElementNode, XYZ> localCoords = this.Element.CalculateLocalPositionsOfNodes();
            XYZ node0Pos = localCoords[node0];
            XYZ node1Pos = localCoords[node1];
            XYZ node2Pos = localCoords[node2];
            
            //FIXME locationInNaturalCoordinates is used as if it is a local coordinate, not a natural coordinate
            double constant = 1.0 / (2.0 * this.Element.Area);
            double n1 = ((node1Pos.X * node2Pos.Y) - (node2Pos.X * node1Pos.Y))
                + ((node1Pos.Y - node2Pos.Y) * locationInNaturalCoordinates.X)
                + ((node2Pos.X - node1Pos.X) * locationInNaturalCoordinates.Y);
            
            double n2 = ((node2Pos.X * node0Pos.Y) - (node0Pos.X * node2Pos.Y))
                + ((node2Pos.Y - node1Pos.Y) * locationInNaturalCoordinates.X)
                + ((node0Pos.X - node2Pos.X) * locationInNaturalCoordinates.Y);
            
            double n3 = ((node0Pos.X * node1Pos.Y) - (node1Pos.X * node0Pos.Y))
                + ((node0Pos.Y - node1Pos.Y) * locationInNaturalCoordinates.X)
                + ((node1Pos.X - node0Pos.X) * locationInNaturalCoordinates.Y);
            
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
            
            IDictionary<IFiniteElementNode, XYZ> localCoords = this.Element.CalculateLocalPositionsOfNodes();
            XYZ node0Pos = localCoords[node0];
            XYZ node1Pos = localCoords[node1];
            XYZ node2Pos = localCoords[node2];
            
            double constant = 1.0 / (2.0 * this.Element.Area);
            
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * (node1Pos.Y - node2Pos.Y));
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * (node2Pos.Y - node0Pos.Y));
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * (node0Pos.Y - node1Pos.Y));
            
            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * (node2Pos.X - node1Pos.X));
            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * (node0Pos.X - node2Pos.X));
            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * (node1Pos.X - node0Pos.X));
            
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * (node2Pos.X - node1Pos.X));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * (node1Pos.Y - node2Pos.Y));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * (node0Pos.X - node2Pos.X));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * (node2Pos.Y - node0Pos.Y));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * (node1Pos.X - node0Pos.X));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * (node0Pos.Y - node1Pos.Y));
            
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
