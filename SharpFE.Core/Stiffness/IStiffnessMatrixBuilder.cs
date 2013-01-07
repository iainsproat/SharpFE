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
	/// Builds the stiffness matrix for a single finite element
	/// </summary>
	public interface IStiffnessMatrixBuilder
	{
		FiniteElement Element { get; }
		KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> GetShapeFunctionVector(FiniteElementNode location);
		KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> GetStrainDisplacementMatrix();
		StiffnessMatrix GetLocalStiffnessMatrix();
		StiffnessMatrix GlobalStiffnessMatrix { get; }
		
		double GetGlobalStiffnessAt(FiniteElementNode rowNode, DegreeOfFreedom rowDegreeOfFreedom, FiniteElementNode columnNode, DegreeOfFreedom columnDegreeOfFreedom);
		void BuildGlobalStiffnessMatrix();
		Matrix CalculateElementRotationMatrix();
	}
}
