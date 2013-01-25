//-----------------------------------------------------------------------
// <copyright file="LinearSolverSVD.cs" company="Iain Sproat">
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "SVD", Justification = "abbreviation is suitable")]
    public class LinearSolverSVD : LinearSolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelToSolve"></param>
        public LinearSolverSVD(FiniteElementModel modelToSolve)
            : base(modelToSolve)
        {
            // empty
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stiffnessMatrix"></param>
        /// <param name="forceVector"></param>
        /// <returns></returns>
        protected override KeyedVector<NodalDegreeOfFreedom> Solve(StiffnessMatrix stiffnessMatrix, KeyedVector<NodalDegreeOfFreedom> forceVector)
        {
            Svd svd = new DenseSvd(stiffnessMatrix, true);
            Vector<double> solution = svd.Solve(forceVector);
            return new KeyedVector<NodalDegreeOfFreedom>(solution, forceVector.Keys);
        }
    }
}
