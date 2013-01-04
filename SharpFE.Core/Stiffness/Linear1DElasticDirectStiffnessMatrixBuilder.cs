//-----------------------------------------------------------------------
// <copyright file="LinearElastic.cs" company="SharpFE">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;

namespace SharpFE.Stiffness
{
	/// <summary>
	/// Provides stiffness for linear elastic springs in 1D
	/// </summary>
	public class Linear1DElasticDirectStiffnessMatrixBuilder : LinearTrussStiffnessMatrixBuilder
	{
		public Linear1DElasticDirectStiffnessMatrixBuilder(double constant)
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
        public override ElementStiffnessMatrix GetStiffnessMatrix()
        {
        	ConstantLinearSpring spring = this.CastElementToSpring();
			
			ElementStiffnessMatrix matrix = new ElementStiffnessMatrix(this.Element.SupportedNodalDegreeOfFreedoms);
            matrix.At(spring.StartNode, DegreeOfFreedom.X, spring.StartNode, DegreeOfFreedom.X, this.SpringConstant);
            matrix.At(spring.StartNode, DegreeOfFreedom.X, spring.EndNode, DegreeOfFreedom.X, -this.SpringConstant);
            matrix.At(spring.EndNode, DegreeOfFreedom.X, spring.StartNode, DegreeOfFreedom.X, -this.SpringConstant);
            matrix.At(spring.EndNode, DegreeOfFreedom.X, spring.EndNode, DegreeOfFreedom.X, this.SpringConstant);
            
            return matrix;
        }
        
        private ConstantLinearSpring CastElementToSpring()
        {
        	ConstantLinearSpring spring = this.Element as ConstantLinearSpring;
			if (spring == null)
			{
				throw new NotImplementedException("LinearElasticSpring has only been implemented for Spring finite elements");
			}
			
			return spring;
        }
	}
}
