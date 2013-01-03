//-----------------------------------------------------------------------
// <copyright file="IStiffnessProvider.cs" company="SharpFE">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace SharpFE.Stiffness
{
	/// <summary>
	/// Builds the stiffness matrix for a finite element
	/// </summary>
	public interface IStiffnessMatrixBuilder
	{
		ElementStiffnessMatrix GetStiffnessMatrix(FiniteElement element);
	}
}
