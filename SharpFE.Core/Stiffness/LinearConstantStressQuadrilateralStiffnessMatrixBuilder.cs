//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace SharpFE.Stiffness
{
    /// <summary>
    /// Description of LinearConstantStressQuadrilateralStiffnessMatrixBuilder.
    /// </summary>
    public class LinearConstantStressQuadrilateralStiffnessMatrixBuilder : ElementStiffnessMatrixBuilder<LinearConstantStressQuadrilateral>
    {
        public LinearConstantStressQuadrilateralStiffnessMatrixBuilder(LinearConstantStressQuadrilateral element)
            : base(element)
        {
            // empty
        }
        
        public override KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> GetShapeFunctionVector(FiniteElementNode location)
        {
            ////TODO should be able to get the below from the ModelType
            IList<DegreeOfFreedom> supportedDegreesOfFreedom = new List<DegreeOfFreedom>(2);
            supportedDegreesOfFreedom.Add(DegreeOfFreedom.X);
            supportedDegreesOfFreedom.Add(DegreeOfFreedom.Y);
            
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> shapeFunctions = new KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom>(supportedDegreesOfFreedom, supportedNodalDegreesOfFreedom);
            
            FiniteElementNode node0 = this.Element.Nodes[0];
            FiniteElementNode node1 = this.Element.Nodes[1];
            FiniteElementNode node2 = this.Element.Nodes[2];
            
            
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
        
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> GetStrainDisplacementMatrix()
        {
            IList<Strain> supportedStrains = this.SupportedStrains;
            IList<NodalDegreeOfFreedom> supportedNodalDegreesOfFreedom = this.Element.SupportedNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> B = new KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom>(supportedStrains, supportedNodalDegreesOfFreedom);
            
            FiniteElementNode node0 = this.Element.Nodes[0];
            FiniteElementNode node1 = this.Element.Nodes[1];
            FiniteElementNode node2 = this.Element.Nodes[2];
            FiniteElementNode node3 = this.Element.Nodes[3];
            
//            double constant = 1.0 / (2.0 * quad.Area);
//            
//            B.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * (node1.OriginalY - node2.OriginalY));
//            B.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * (node2.OriginalY - node0.OriginalY));
//            B.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * (node0.OriginalY - node1.OriginalY));
//            
//            B.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * (node2.OriginalX - node1.OriginalX));
//            B.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * (node0.OriginalX - node2.OriginalX));
//            B.At(Strain.LinearStrainY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * (node1.OriginalX - node0.OriginalX));
//            
//            B.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.X), constant * (node2.OriginalX - node1.OriginalX));
//            B.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node0, DegreeOfFreedom.Y), constant * (node1.OriginalY - node2.OriginalY));
//            B.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.X), constant * (node0.OriginalX - node2.OriginalX));
//            B.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node1, DegreeOfFreedom.Y), constant * (node2.OriginalY - node0.OriginalY));
//            B.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.X), constant * (node1.OriginalX - node0.OriginalX));
//            B.At(Strain.ShearStrainXY, new NodalDegreeOfFreedom(node2, DegreeOfFreedom.Y), constant * (node0.OriginalY - node1.OriginalY));
            
            return B;
        }
        
        public override StiffnessMatrix GetLocalStiffnessMatrix()
        {
            double elementVolume = this.Element.Thickness * this.Element.Area; ////FIXME
            KeyedMatrix<Strain> E = this.MaterialMatrix();
            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> B = this.GetStrainDisplacementMatrix();
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, Strain> BT = B.Transpose();
            
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, Strain> BTE = BT.Multiply<Strain, Strain>(E);
            
            KeyedRowColumnMatrix<NodalDegreeOfFreedom, NodalDegreeOfFreedom> BTEB = BTE.Multiply<Strain, NodalDegreeOfFreedom>(B);
            
            StiffnessMatrix k = new StiffnessMatrix(BTEB.Multiply(elementVolume), BTEB.RowKeys, BTEB.ColumnKeys);
            return k;
        }
        
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
        
        private KeyedMatrix<Strain> MaterialMatrix()
        {
            IMaterial material = this.Element.Material;
            double constant = material.YoungsModulus / ((1.0 + material.PoissonsRatio) * (1.0 - 2.0 * material.PoissonsRatio));
            
            KeyedMatrix<Strain> E = new KeyedMatrix<Strain>(this.SupportedStrains);
            // FIXME the below is for constant strain, not constant stress
//            E.At(Strain.LinearStrainX, Strain.LinearStrainX, constant * (1.0 - material.PoissonsRatio));
//            E.At(Strain.LinearStrainX, Strain.LinearStrainY, constant * material.PoissonsRatio);
//            
//            E.At(Strain.LinearStrainY, Strain.LinearStrainX, constant * material.PoissonsRatio);
//            E.At(Strain.LinearStrainY, Strain.LinearStrainY, constant * (1.0 - material.PoissonsRatio));
//            
//            E.At(Strain.ShearStrainXY, Strain.ShearStrainXY, constant * ((1.0 - 2.0 * material.PoissonsRatio) / 2.0));
            
            return E;
        }
    }
}
