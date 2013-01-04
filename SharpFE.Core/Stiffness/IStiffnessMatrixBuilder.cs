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
		void Initialize(FiniteElement finiteElement);
		FiniteElement Element { get; }
		KeyedVector<NodalDegreeOfFreedom> GetStrainDisplacementMatrix();
		ElementStiffnessMatrix GetStiffnessMatrix();
		
		ElementStiffnessMatrix BuildGlobalStiffnessMatrix();
		Matrix CalculateElementRotationMatrix();
	}
}
