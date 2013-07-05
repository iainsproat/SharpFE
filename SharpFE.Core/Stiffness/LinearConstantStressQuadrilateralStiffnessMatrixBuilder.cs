//-----------------------------------------------------------------------
// <copyright file="LinearConstantStressQuadrilateralStiffnessMatrixBuilder.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;
    
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
        public override KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> ShapeFunctionVector(IFiniteElementNode location)
        {
            ////TODO should be able to get the below from the ModelType
            IList<DegreeOfFreedom> supportedDegreesOfFreedom = new List<DegreeOfFreedom>(2);
            supportedDegreesOfFreedom.Add(DegreeOfFreedom.X);
            supportedDegreesOfFreedom.Add(DegreeOfFreedom.Y);
            
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> shapeFunctions = new KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom>(supportedDegreesOfFreedom, supportedNodalDegreesOfFreedom);
            
            IFiniteElementNode node0 = this.Element.Nodes[0];
            IFiniteElementNode node1 = this.Element.Nodes[1];
            IFiniteElementNode node2 = this.Element.Nodes[2];
                        
            //            double constant = 1.0 / (2.0 * quad.Area);
            //            double N1 = (node1.OriginalX * node2.OriginalY - node2.OriginalX * node1.OriginalY)
            //                + (node1.OriginalY - node2.OriginalY) * location.OriginalX
            //                + (node2.OriginalX - node1.OriginalX) * location.OriginalY;
//
            //            double N2 = (node2.OriginalX * node0.OriginalY - node0.OriginalX * node2.OriginalY)
            //                + (node2.OriginalY - node1.OriginalY) * location.OriginalX
            //                + (node0.OriginalX - node2.OriginalX) * location.OriginalY;
//
            //            double N3 = (node0.OriginalX * node1.OriginalY - node1.OriginalX * node0.OriginalY)
            //                + (node0.OriginalY - node1.OriginalY) * location.OriginalX
            //                + (node1.OriginalX - node0.OriginalX) * location.OriginalY;
//
            //            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * N1);
            //            shapeFunctions.At(DegreeOfFreedom.Y, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * N1);
//
            //            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * N2);
            //            shapeFunctions.At(DegreeOfFreedom.Y, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * N2);
//
            //            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * N3);
            //            shapeFunctions.At(DegreeOfFreedom.Y, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * N3);
            return shapeFunctions;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> StrainDisplacementMatrix(IFiniteElementNode location)
        {
            IList<Strain> supportedStrains = this.SupportedStrains;
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> strainDisplacementMatrix = new KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom>(supportedStrains, supportedNodalDegreesOfFreedom);
            
            IFiniteElementNode node0 = this.Element.Nodes[0];
            IFiniteElementNode node1 = this.Element.Nodes[1];
            IFiniteElementNode node2 = this.Element.Nodes[2];
            IFiniteElementNode node3 = this.Element.Nodes[3];
            
            double constant = 1.0 / this.Element.Area;
  
//TODO            
//            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * (node1.Y - node2.Y));
//            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * (node2.Y - node0.Y));
//            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * (node0.Y - node1.Y));
//            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node3, DegreeOfFreedom.X), constant * (node0.Y - node1.Y));
//            
//            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * (node2.X - node1.X));
//            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * (node0.X - node2.X));
//            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * (node1.X - node0.X));
//            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node3, DegreeOfFreedom.Y), constant * (node1.X - node0.X));
//            
//            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * (node2.X - node1.X));
//            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * (node1.Y - node2.Y));
//            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * (node0.X - node2.X));
//            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * (node2.Y - node0.Y));
//            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * (node1.X - node0.X));
//            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * (node0.Y - node1.Y));
//            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node3, DegreeOfFreedom.X), constant * (node1.X - node0.X));
//            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node3, DegreeOfFreedom.Y), constant * (node0.Y - node1.Y));
            
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
            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> strainDisplacementMatrix = this.StrainDisplacementMatrix(null);//FIXME
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, Strain> transposedStrainDisplacementMatrix = strainDisplacementMatrix.Transpose();
            
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, Strain> bte = transposedStrainDisplacementMatrix.Multiply<Strain, Strain>(materialMatrix);
            
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, NodalDegreeOfFreedom> bteb = bte.Multiply<Strain, NodalDegreeOfFreedom>(strainDisplacementMatrix);
            
            StiffnessMatrix k = new StiffnessMatrix(bteb.Multiply(elementVolume));
            return k;
        }
    }
}
