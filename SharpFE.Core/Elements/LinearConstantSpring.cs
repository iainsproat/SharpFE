﻿//-----------------------------------------------------------------------
// <copyright file="Spring.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra.Generic;
    using SharpFE.Stiffness;
    using SharpFE.Elements;

    /// <summary>
    /// A spring is a 1D linear element which has a constant stiffness along the local x-axis.
    /// </summary>
    public class LinearConstantSpring : FiniteElement1D
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantLinearSpring" /> class.
        /// </summary>
        /// <param name="node1">The node at the start of the spring.</param>
        /// <param name="node2">The node at the end of the spring.</param>
        /// <param name="springConstant">The value which defines the constant stiffness of the spring.</param>
        internal LinearConstantSpring(FiniteElementNode node1, FiniteElementNode node2, double springConstant)
            : base(node1, node2)
        {
        	this.SpringConstant = springConstant;
        }
        
        public double SpringConstant
        {
        	get;
        	private set;
        }
        
        public override bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom)
		{
			switch(degreeOfFreedom)
			{
				case DegreeOfFreedom.X:
					return true;
				case DegreeOfFreedom.Y:
				case DegreeOfFreedom.Z:
				case DegreeOfFreedom.XX:
				case DegreeOfFreedom.YY:
				case DegreeOfFreedom.ZZ:
				default:
					return false;
			}
		}
    }
}
