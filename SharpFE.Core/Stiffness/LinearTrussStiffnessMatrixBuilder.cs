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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> ShapeFunctionVector(FiniteElementNode location)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Generates the transposed strain-displacement matrix for the given element
        /// </summary>
        /// <returns></returns>
        public override KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> StrainDisplacementMatrix(FiniteElementNode location)
        {
            IList<Strain> supportedDegreesOfFreedom = new List<Strain>(1);
            supportedDegreesOfFreedom.Add(Strain.LinearStrainX);
            IList<NodalDegreeOfFreedom> supportedNodalDegreeOfFreedoms = this.Element.SupportedNodalDegreeOfFreedoms;
            KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> strainDisplacementMatrix = new KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom>(supportedDegreesOfFreedom, supportedNodalDegreeOfFreedoms);
            
            double originalLength = this.Element.OriginalLength;
            
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(this.Element.StartNode, DegreeOfFreedom.X), -1.0 / originalLength);
            strainDisplacementMatrix.At(Strain.LinearStrainX, new NodalDegreeOfFreedom(this.Element.EndNode, DegreeOfFreedom.X), 1.0 / originalLength);
            
            return strainDisplacementMatrix;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override StiffnessMatrix LocalStiffnessMatrix()
        {
            double stiffness = this.CalculateStiffnessConstant();
            
            StiffnessMatrix matrix = new StiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.X, this.Element.StartNode, DegreeOfFreedom.X, stiffness);
            matrix.At(this.Element.StartNode, DegreeOfFreedom.X, this.Element.EndNode, DegreeOfFreedom.X, -stiffness);
            matrix.At(this.Element.EndNode, DegreeOfFreedom.X, this.Element.StartNode, DegreeOfFreedom.X, -stiffness);
            matrix.At(this.Element.EndNode, DegreeOfFreedom.X, this.Element.EndNode, DegreeOfFreedom.X, stiffness);
            
            return matrix;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
