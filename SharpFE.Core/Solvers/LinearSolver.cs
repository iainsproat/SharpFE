//-----------------------------------------------------------------------
// <copyright file="LinearSolver.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using MathNet.Numerics.LinearAlgebra.Double;
    using SharpFE;

    /// <summary>
    /// Carries out a simple, linear analysis to solve a static, implicit finite element problem.
    /// </summary>
    public class LinearSolver : IFiniteElementSolver
    {
        /// <summary>
        /// The model with the data to solve
        /// </summary>
        private FiniteElementModel model;
        
        /// <summary>
        /// A helper class to build the stiffness matrices from the model
        /// </summary>
        private StiffnessMatrixBuilder matrixBuilder;
        
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LinearSolver" /> class.
        /// </summary>
        /// <param name="modelToSolve">The model on which to run the analysis</param>
        public LinearSolver(FiniteElementModel modelToSolve)
            : this(modelToSolve, new StiffnessMatrixBuilder(modelToSolve))
        {
            // empty
        }
        
        public LinearSolver(FiniteElementModel modelToSolve, StiffnessMatrixBuilder stiffnessMatrixBuilder)
        {
            Guard.AgainstNullArgument(modelToSolve, "modelToSolve");
            Guard.AgainstNullArgument(stiffnessMatrixBuilder, "stiffnessMatrixBuilder");
            
            this.model = modelToSolve;
            this.matrixBuilder = stiffnessMatrixBuilder;
        }
        #endregion
        
        /// <summary>
        /// Solves the model containing the finite element problem
        /// </summary>
        /// <returns>The results of analysis</returns>
        public FiniteElementResults Solve()
        {
            this.ThrowIfNotAValidModel();
            
            Vector displacements = this.CalculateUnknownDisplacements();
            Vector reactions = this.CalculateUnknownReactions(displacements);
            
            IList<NodalDegreeOfFreedom> displacementIdentifiers = this.model.DegreesOfFreedomWithUnknownDisplacement;
            IList<NodalDegreeOfFreedom> reactionIdentifiers = this.model.DegreesOfFreedomWithUnknownForce;
            
            reactions = this.CombineExternalForcesOnReactionNodesWithReactions(reactionIdentifiers, reactions);
            
            return this.CreateResults(displacementIdentifiers, reactionIdentifiers, displacements, reactions);
        }
        
        /// <summary>
        /// Checks as to whether the model is valid for solving.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the model is invalid for solving.</exception>
        private void ThrowIfNotAValidModel()
        {
            if (this.model.NodeCount < 2)
            {
                throw new InvalidOperationException("The model has less than 2 nodes and so cannot be solved");
            }
            
            if (this.model.ElementCount < 1)
            {
                throw new InvalidOperationException("The model has no elements and so cannot be solved");
            }
        }
        
        /// <summary>
        /// Calculates part of the problem for unknown displacements
        /// </summary>
        /// <returns>A vector of the displacements which were previously unknown, and have now been solved</returns>
        private Vector CalculateUnknownDisplacements()
        {
            Matrix knownForcesUnknownDisplacementStiffnesses = this.matrixBuilder.BuildKnownForcesUnknownDisplacementStiffnessMatrix(); // K11
            Vector knownForces = this.model.KnownForceVector(); // Fk
            Matrix knownForcesKnownDisplacementsStiffnesses = this.matrixBuilder.BuildKnownForcesKnownDisplacementStiffnessMatrix(); // K12
            Vector knownDisplacements = this.model.KnownDisplacementVector(); // Uk
            
            // solve for unknown displacements
            // Uu = K11^-1 * (Fk + (K12 * Uk))
            Vector forcesDueToExternallyAppliedDisplacements = (Vector)knownForcesKnownDisplacementsStiffnesses.Multiply(knownDisplacements); // K12 * Uk
            Vector externallyAppliedForces = (Vector)knownForces.Add(forcesDueToExternallyAppliedDisplacements); // Fk + (K12 * Uk)
            
            if (knownForcesUnknownDisplacementStiffnesses.Determinant() == 0)
            {
                throw new InvalidOperationException(
                    string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "We are unable to solve this as we cannot inverse a matrix of zero determinant.\r\nMatrix of stiffnesses for known forces and unknown displacements:\r\n {0}",
                        knownForcesUnknownDisplacementStiffnesses));
            }
            
            Matrix inverse = (Matrix)knownForcesUnknownDisplacementStiffnesses.Inverse(); // K11^-1
            Vector unknownDisplacements = (Vector)inverse.Multiply(externallyAppliedForces); // K11^-1 * (Fk + (K12 * Uk))
            return unknownDisplacements;
        }
        
        /// <summary>
        /// Calculates part of the stiffness equations for the unknown reactions.
        /// </summary>
        /// <param name="unknownDisplacements">A vector of the displacements which were previously unknown</param>
        /// <returns>A vector of the reactions which were previously unknown, and have now been solved</returns>
        private Vector CalculateUnknownReactions(Vector unknownDisplacements)
        {
            Guard.AgainstNullArgument(unknownDisplacements, "unknownDisplacements");
            
            // Fu = K21 * Uu + K22 * Uk
            Matrix unknownForcesUnknownDisplacementStiffnesses = this.matrixBuilder.BuildUnknownForcesUnknownDisplacementStiffnessMatrix(); // K21
            Matrix unknownForcesKnownDisplacementsStiffnesses = this.matrixBuilder.BuildUnknownForcesKnownDisplacementStiffnessMatrix(); // K22
            Vector knownDisplacements = this.model.KnownDisplacementVector(); // Uk
            
            Vector lhsStatement = (Vector)unknownForcesUnknownDisplacementStiffnesses.Multiply(unknownDisplacements); // K21 * Uu
            Vector rhsStatement = (Vector)unknownForcesKnownDisplacementsStiffnesses.Multiply(knownDisplacements); // K22 * Uk
            
            Vector unknownReactions = (Vector)lhsStatement.Add(rhsStatement);
            return unknownReactions;
        }
        
        /// <summary>
        /// The user may have placed reactions directly on to fixed supports.
        /// These are ignored during the calculation, but the correct answer for the total reaction must include them
        /// </summary>
        /// <param name="reactionIdentifiers">Identifers for the nodes and degrees of freedom which are fixed and therefore have reactions</param>
        /// <param name="reactions">The calculated values of the reactions</param>
        /// <returns>The calculated values of the reactions with additional external forces added where applicable.</returns>
        private Vector CombineExternalForcesOnReactionNodesWithReactions(IList<NodalDegreeOfFreedom> reactionIdentifiers, Vector reactions)
        {
            Vector externalForcesOnReactionNodes = this.model.GetCombinedForcesFor(reactionIdentifiers);
            return (Vector)reactions.Add(externalForcesOnReactionNodes);
        }
        
        /// <summary>
        /// Puts the data into the results data structure.
        /// </summary>
        /// <param name="displacementIdentifiers">Identifers for the nodes and degrees of freedom which are free and therefore have displacements</param>
        /// <param name="reactionIdentifiers">Identifers for the nodes and degrees of freedom which are fixed and therefore have reactions</param>
        /// <param name="displacements">The calculated displacements.  The index of the values in the vector matches the index of the displacement identifiers.</param>
        /// <param name="reactions">The calculated displacements.  The index of the values in the vector matches the index of the displacement identifiers.</param>
        /// <returns>The results in a presentable data structure</returns>
        private FiniteElementResults CreateResults(IList<NodalDegreeOfFreedom> displacementIdentifiers, IList<NodalDegreeOfFreedom> reactionIdentifiers, Vector displacements, Vector reactions)
        {
            Guard.AgainstNullArgument(displacementIdentifiers, "displacementIdentifiers");
            Guard.AgainstNullArgument(reactionIdentifiers, "reactionIdentifiers");
            Guard.AgainstNullArgument(displacements, "displacements");
            Guard.AgainstNullArgument(reactions, "reactions");
            
            FiniteElementResults results = new FiniteElementResults(this.model.ModelType);
            results.AddMultipleDisplacements(displacementIdentifiers, displacements);
            results.AddMultipleReactions(reactionIdentifiers, reactions);
            return results;
        }
    }
}
