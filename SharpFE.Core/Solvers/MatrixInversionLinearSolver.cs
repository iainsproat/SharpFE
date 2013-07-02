/*
 * Copyright Iain Sproat, 2013
 * User: ispro
 * Date: 02/07/2013
 * 
 */


namespace SharpFE
{
    using System;
    using SharpFE.Solvers;
    using SharpFE.Stiffness;
    /// <summary>
    /// Description of MatrixInversionLinearSolver.
    /// </summary>
    public class MatrixInversionLinearSolver : LinearSolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelToSolve"></param>
        public MatrixInversionLinearSolver(FiniteElementModel modelToSolve)
            : base(modelToSolve)
        {
            // empty
        }
        
        protected override KeyedVector<NodalDegreeOfFreedom> Solve(StiffnessMatrix stiffnessMatrix, KeyedVector<NodalDegreeOfFreedom> forceVector)
        {
            KeyedSquareMatrix<NodalDegreeOfFreedom> inverse = stiffnessMatrix.Inverse();
            KeyedVector<NodalDegreeOfFreedom> solution = inverse.Multiply(forceVector);
            return solution;
        }
    }
}
