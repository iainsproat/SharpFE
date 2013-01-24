//-----------------------------------------------------------------------
// <copyright file="LinearTrussStiffnessMatrixBuilder.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;
    using SharpFE.Elements;
    
    /// <summary>
    /// Description of LinearElasticMaterialCrossSectionStiffnessBuilder.
    /// </summary>
    public class LinearTrussStiffnessMatrixBuilder : ElementStiffnessMatrixBuilder<FiniteElement1D>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finiteElement"></param>
        public LinearTrussStiffnessMatrixBuilder(FiniteElement1D finiteElement)
            : base(finiteElement)
        {
            // empty
        }

        public override KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> GetShapeFunctionVector(FiniteElementNode location)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Generates the transposed strain-displacement matrix for the given element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> GetStrainDisplacementMatrix()
        {
            IList<Strain> supportedDegreesOfFreedom = new List<Strain>(1);
            supportedDegreesOfFreedom.Add(Strain.LinearStrainX);
            IList<NodalDegreeOfFreedom> supportedNodalDegreeOfFreedoms = this.Element.SupportedNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> B = new KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom>(supportedDegreesOfFreedom, supportedNodalDegreeOfFreedoms);
            
            double originalLength = this.Element.OriginalLength;
            
            B.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(this.Element.StartNode, DegreeOfFreedom.X), -1.0 / originalLength);
            B.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(this.Element.EndNode, DegreeOfFreedom.X), 1.0 / originalLength);
            
            return B;
        }
        
        public override StiffnessMatrix GetLocalStiffnessMatrix()
        {
            double stiffness = this.CalculateStiffnessConstant();
            
            StiffnessMatrix matrix = new StiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.X, this.Element.StartNode, DegreeOfFreedom.X, stiffness);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.X, this.Element.EndNode, DegreeOfFreedom.X, -stiffness);
            matrix.At(this.Element.EndNode, DegreeOfFreedom.X, this.Element.StartNode, DegreeOfFreedom.X, -stiffness);
            matrix.At(this.Element.EndNode, DegreeOfFreedom.X, this.Element.EndNode, DegreeOfFreedom.X, stiffness);
            
            return matrix;
        }
        
        private double CalculateStiffnessConstant()
        {
            try
            {
                LinearConstantSpring spring = this.Element as LinearConstantSpring;
                if (spring != null)
                {
                    return spring.SpringConstant;
                }
            }
            catch (InvalidCastException)
            {
                // do nothing
            }
            
            LinearTruss truss = this.Element as LinearTruss;
            
            return truss.Material.YoungsModulus * truss.CrossSection.Area / this.Element.OriginalLength;
        }
    }
}
