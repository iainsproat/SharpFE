//-----------------------------------------------------------------------
// <copyright file="LinearSolverSVD.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    using SharpFE.Solvers;
    using SharpFE.Stiffness;
    using SharpFE.Maths.MatrixSolvers;

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
            Svd<NodalDegreeOfFreedom, NodalDegreeOfFreedom> svd = new Svd<NodalDegreeOfFreedom, NodalDegreeOfFreedom>(stiffnessMatrix, true);
            return svd.Solve(forceVector);
        }
    }
}
