//-----------------------------------------------------------------------
// <copyright file="IStiffnessProvider.cs" company="SharpFE">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------



namespace SharpFE.Stiffness
{
	using System;
	using MathNet.Numerics.LinearAlgebra.Double;
	/// <summary>
	/// Builds the stiffness matrix for a finite element
	/// </summary>
	public interface IStiffnessMatrixBuilder
	{
		KeyedVector<NodalDegreeOfFreedom> GetStrainDisplacementMatrix(FiniteElement element);
		ElementStiffnessMatrix GetStiffnessMatrix(FiniteElement element);
	}
}
