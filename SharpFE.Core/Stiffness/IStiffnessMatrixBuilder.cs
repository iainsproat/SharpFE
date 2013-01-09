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
    public interface IStiffnessMatrixBuilder<T> : IStiffnessProvider where T : FiniteElement
    {
        T Element { get; }
        KeyedRowColumnMatrix<DegreeOfFreedom, NodalDegreeOfFreedom> GetShapeFunctionVector(FiniteElementNode location);
        KeyedRowColumnMatrix<Strain, NodalDegreeOfFreedom> GetStrainDisplacementMatrix();
        StiffnessMatrix GetLocalStiffnessMatrix();
        
        void BuildGlobalStiffnessMatrix();
        Matrix CalculateElementRotationMatrix();
    }
}
