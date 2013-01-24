//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Double.Factorization;
    using MathNet.Numerics.LinearAlgebra.Generic;
    using SharpFE.Stiffness;

    /// <summary>
    /// Linear solver using Singular Value Decomposition
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SVD")]
    public class LinearSolverSVD : LinearSolver
    {
        public LinearSolverSVD(FiniteElementModel modelToSolve)
            : base(modelToSolve)
        {
            // empty
        }
        
        protected override KeyedVector<NodalDegreeOfFreedom> Solve(StiffnessMatrix stiffnessMatrix, KeyedVector<NodalDegreeOfFreedom> forceVector)
        {
            Svd svd = new DenseSvd(stiffnessMatrix, true);
            Vector<double> X = svd.Solve(forceVector);
            return new KeyedVector<NodalDegreeOfFreedom>(X, forceVector.Keys);
        }
    }
}
