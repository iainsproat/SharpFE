//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using SharpFE.Stiffness;

namespace SharpFE
{
    /// <summary>
    /// Linear solver using Singular Value Decomposition
    /// </summary>
    public class LinearSolverSVD : LinearSolver
    {
        public LinearSolverSVD(FiniteElementModel modelToSolve)
            : base(modelToSolve)
        {
            // empty
        }
        
        protected override KeyedVector<NodalDegreeOfFreedom> Solve(StiffnessMatrix A, KeyedVector<NodalDegreeOfFreedom> B)
        {
            Svd svd = new DenseSvd(A, true);
            Vector<double> X = svd.Solve(B);
            return new KeyedVector<NodalDegreeOfFreedom>(X, B.Keys);
        }
    }
}
