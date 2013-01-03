//-----------------------------------------------------------------------
// <copyright file="LinearElastic.cs" company="SharpFE">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SharpFE.Stiffness
{
	/// <summary>
	/// Provides stiffness for linear elastic springs in 1D
	/// </summary>
	public class LinearElastic1DSpringStiffnessMatrixBuilder : IStiffnessMatrixBuilder
	{
		public LinearElastic1DSpringStiffnessMatrixBuilder()
		{
			SpringConstant = 1;
		}
		
		public LinearElastic1DSpringStiffnessMatrixBuilder(double constant)
		{
			this.SpringConstant = constant;
		}
		
		public double SpringConstant
		{
			get;
			private set;
		}
		
		/// <summary>
        /// Generates the stiffness matrix for the given element
        /// </summary>
        public ElementStiffnessMatrix GetStiffnessMatrix(FiniteElement element)
        {
        	ConstantLinearSpring spring = element as ConstantLinearSpring;
			if (spring == null)
			{
				throw new NotImplementedException("LinearElasticSpring has only been implemented for Spring finite elements");
			}
			
			ElementStiffnessMatrix matrix = new ElementStiffnessMatrix(spring.SupportedNodalDegreeOfFreedoms);
            matrix.At(spring.StartNode, DegreeOfFreedom.X, spring.StartNode, DegreeOfFreedom.X, this.SpringConstant);
            matrix.At(spring.StartNode, DegreeOfFreedom.X, spring.EndNode, DegreeOfFreedom.X, -this.SpringConstant);
            matrix.At(spring.EndNode, DegreeOfFreedom.X, spring.StartNode, DegreeOfFreedom.X, -this.SpringConstant);
            matrix.At(spring.EndNode, DegreeOfFreedom.X, spring.EndNode, DegreeOfFreedom.X, this.SpringConstant);
            
            return matrix;
        }
	}
}
