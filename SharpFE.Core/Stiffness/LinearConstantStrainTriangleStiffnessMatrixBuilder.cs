//-----------------------------------------------------------------------
// <copyright file="LinearConstantStrainTriangleStiffnessMatrixBuilder.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;
    
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
            // empty
        }
        
        /// <summary>
        /// 
        /// </summary>
        private static IList<Strain> SupportedStrains
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> ShapeFunctionVector(FiniteElementNode location)
        {
            ////TODO check that location is within or on the bounds of the finite element.
            ////TODO should be able to get the below from the ModelType
            IList<DegreeOfFreedom> supportedDegreesOfFreedom = new List<DegreeOfFreedom>(2){
                DegreeOfFreedom.X,
                DegreeOfFreedom.Y};
            
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> shapeFunctions = new KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom>(supportedDegreesOfFreedom, supportedNodalDegreesOfFreedom);
            
            IFiniteElementNode node0 = this.Element.Nodes[0];
            IFiniteElementNode node1 = this.Element.Nodes[1];
            IFiniteElementNode node2 = this.Element.Nodes[2];
            
            double constant = 1.0 / (2.0 * this.Element.Area);
            double n1 = ((node1.OriginalX * node2.OriginalY) - (node2.OriginalX * node1.OriginalY))
                + ((node1.OriginalY - node2.OriginalY) * location.OriginalX)
                + ((node2.OriginalX - node1.OriginalX) * location.OriginalY);
            
            double n2 = ((node2.OriginalX * node0.OriginalY) - (node0.OriginalX * node2.OriginalY))
                + ((node2.OriginalY - node1.OriginalY) * location.OriginalX)
                + ((node0.OriginalX - node2.OriginalX) * location.OriginalY);
            
            double n3 = ((node0.OriginalX * node1.OriginalY) - (node1.OriginalX * node0.OriginalY))
                + ((node0.OriginalY - node1.OriginalY) * location.OriginalX)
                + ((node1.OriginalX - node0.OriginalX) * location.OriginalY);
            
            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * n1);
            shapeFunctions.At(DegreeOfFreedom.Y, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * n1);
            
            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * n2);
            shapeFunctions.At(DegreeOfFreedom.Y, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * n2);
            
            shapeFunctions.At(DegreeOfFreedom.X, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * n3);
            shapeFunctions.At(DegreeOfFreedom.Y, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * n3);

            return shapeFunctions;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> StrainDisplacementMatrix(FiniteElementNode location)
        {
            IList<Strain> supportedStrains = LinearConstantStrainTriangleStiffnessMatrixBuilder.SupportedStrains;
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> strainDisplacementMatrix = new KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom>(supportedStrains, supportedNodalDegreesOfFreedom);
            
            IFiniteElementNode node0 = this.Element.Nodes[0];
            IFiniteElementNode node1 = this.Element.Nodes[1];
            IFiniteElementNode node2 = this.Element.Nodes[2];
            
            double constant = 1.0 / (2.0 * this.Element.Area);
            
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * (node1.OriginalY - node2.OriginalY));
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * (node2.OriginalY - node0.OriginalY));
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * (node0.OriginalY - node1.OriginalY));
            
            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * (node2.OriginalX - node1.OriginalX));
            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * (node0.OriginalX - node2.OriginalX));
            strainDisplacementMatrix.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * (node1.OriginalX - node0.OriginalX));
            
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * (node2.OriginalX - node1.OriginalX));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * (node1.OriginalY - node2.OriginalY));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * (node0.OriginalX - node2.OriginalX));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * (node2.OriginalY - node0.OriginalY));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * (node1.OriginalX - node0.OriginalX));
            strainDisplacementMatrix.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * (node0.OriginalY - node1.OriginalY));
            
            return strainDisplacementMatrix;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override StiffnessMatrix LocalStiffnessMatrix()
        {
            double elementVolume = this.Element.Thickness * this.Element.Area;
            KeyedMatrix<Strain> materialMatrix = this.MaterialMatrix();
            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> strainDisplacementMatrix = null; ////FIXME //this.StrainDisplacementMatrix();
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, Strain> transposedStrainDisplacementMatrix = strainDisplacementMatrix.Transpose();
            
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, Strain> bte = transposedStrainDisplacementMatrix.Multiply<Strain, Strain>(materialMatrix);
            
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, NodalDegreeOfFreedom> bteb = bte.Multiply<Strain, NodalDegreeOfFreedom>(strainDisplacementMatrix);
            
            StiffnessMatrix k = new StiffnessMatrix(bteb.Multiply(elementVolume), bteb.RowKeys, bteb.ColumnKeys);
            return k;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private KeyedMatrix<Strain> MaterialMatrix()
        {
            IMaterial material = this.Element.Material;
            double constant = material.YoungsModulus / ((1.0 + material.PoissonsRatio) * (1.0 - (2.0 * material.PoissonsRatio)));
            
            KeyedMatrix<Strain> materialMatrix = new KeyedMatrix<Strain>(LinearConstantStrainTriangleStiffnessMatrixBuilder.SupportedStrains);
            materialMatrix.At(Strain.LinearStrainX, Strain.LinearStrainX, constant * (1.0 - material.PoissonsRatio));
            materialMatrix.At(Strain.LinearStrainX, Strain.LinearStrainY, constant * material.PoissonsRatio);
            
            materialMatrix.At(Strain.LinearStrainY, Strain.LinearStrainX, constant * material.PoissonsRatio);
            materialMatrix.At(Strain.LinearStrainY, Strain.LinearStrainY, constant * (1.0 - material.PoissonsRatio));
            
            materialMatrix.At(Strain.ShearStrainXY, Strain.ShearStrainXY, constant * ((1.0 - (2.0 * material.PoissonsRatio)) / 2.0));
            
            return materialMatrix;
        }
    }
}
