//-----------------------------------------------------------------------
// <copyright file="FiniteElementResults.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Stores collections of values which represent the results of a single finite element analysis procedure
    /// </summary>
    public class FiniteElementResults
    {
        /// <summary>
        /// Displacements which form this set of results
        /// </summary>
        private IDictionary<IFiniteElementNode, DisplacementVector> displacements = new Dictionary<IFiniteElementNode, DisplacementVector>();
        
        /// <summary>
        /// Reactions which form this set of results
        /// </summary>
        private IDictionary<IFiniteElementNode, ReactionVector> reactions = new Dictionary<IFiniteElementNode, ReactionVector>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FiniteElementResults" /> class.
        /// </summary>
        /// <param name="typeOfModel">The type of model which generated these results.</param>
        public FiniteElementResults(ModelType typeOfModel)
            : this(Guid.NewGuid(), DateTime.UtcNow, typeOfModel)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FiniteElementResults" /> class.
        /// </summary>
        /// <param name="guid">The unique Id of these results</param>
        /// <param name="resultsCreatedAt">The time at which these results were created.</param>
        /// <param name="typeOfModel">The type of model which generated these results.</param>
        internal FiniteElementResults(Guid guid, DateTime resultsCreatedAt, ModelType typeOfModel)
        {
            this.Id = guid;
            this.ResultsCreatedAt = resultsCreatedAt;
            this.ModelType = typeOfModel;
        }
        
        /// <summary>
        /// Gets the unique Id of these results
        /// </summary>
        public Guid Id
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the time at which these results were created
        /// </summary>
        public DateTime ResultsCreatedAt
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the type of model and analysis which created these results.
        /// </summary>
        public ModelType ModelType
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Adds a displacement result to this set of results
        /// </summary>
        /// <param name="nodalDegreeOfFreedom">The node and global degree of freedom identifier of the displacement component.</param>
        /// <param name="displacementComponent">The value of the displacement in this global direction</param>
        public void AddDisplacement(NodalDegreeOfFreedom nodalDegreeOfFreedom, double displacementComponent)
        {
            if (!this.displacements.ContainsKey(nodalDegreeOfFreedom.Node))
            {
                this.displacements.Add(nodalDegreeOfFreedom.Node, new DisplacementVector(nodalDegreeOfFreedom.Node));
            }
            
            this.displacements[nodalDegreeOfFreedom.Node].SetValue(nodalDegreeOfFreedom.DegreeOfFreedom, displacementComponent);
        }
        
        /// <summary>
        /// The displacement vector of the given node
        /// </summary>
        /// <param name="displacedNode">The node for which a displacement is to be found.</param>
        /// <returns>The vector of displacements at this node</returns>
        public DisplacementVector GetDisplacement(FiniteElementNode displacedNode)
        {
            return this.displacements[displacedNode];
        }
        
        /// <summary>
        /// Adds a reaction result to this set of results.
        /// </summary>
        /// <param name="nodalDegreeOfFreedom">The node and global degree of freedom identifier for this reaction component.</param>
        /// <param name="forceComponent">The value of the reaction in this global direction</param>
        public void AddReaction(NodalDegreeOfFreedom nodalDegreeOfFreedom, double forceComponent)
        {
            if (!this.reactions.ContainsKey(nodalDegreeOfFreedom.Node))
            {
                this.reactions.Add(nodalDegreeOfFreedom.Node, new ReactionVector(nodalDegreeOfFreedom.Node));
            }
            
            this.reactions[nodalDegreeOfFreedom.Node].SetValue(nodalDegreeOfFreedom.DegreeOfFreedom, forceComponent);
        }
        
        /// <summary>
        /// The reaction vector which occurs at the provided node
        /// </summary>
        /// <param name="supportNode">The node to search for a reaction</param>
        /// <returns>A vector representing the reaction at the support</returns>
        public ReactionVector GetReaction(FiniteElementNode supportNode)
        {
        	if (!this.reactions.ContainsKey(supportNode))
        	{
        		throw new ArgumentException(String.Format(System.Globalization.CultureInfo.InvariantCulture, 
        		                                          "Node {0} is not a support node and therefore has no reaction forces",
        		                                          supportNode));
        	}
        	
            return this.reactions[supportNode];
        }
        
        /// <summary>
        /// Adds multiple displacements to the set of results
        /// </summary>
        /// <param name="displacements">The value of the displacements.  The order of this vector matches the order of the identifiers in the corresponding parameter</param>
        public void AddMultipleDisplacements(KeyedVector<NodalDegreeOfFreedom> disp)
        {
            Guard.AgainstNullArgument(disp, "displacements");
            
            foreach (KeyValuePair<NodalDegreeOfFreedom, double> kvp in disp)
            {
                this.AddDisplacement(kvp.Key, kvp.Value);
            }
        }
        
        /// <summary>
        /// Adds multiple reaction to this set of results
        /// </summary>
        /// <param name="reactions">The value of the reactions.  The order of this vector matches the order of the identifiers in the corresponding parameter.</param>
        public void AddMultipleReactions(KeyedVector<NodalDegreeOfFreedom> react)
        {
            Guard.AgainstNullArgument(reactions, "reactions");
            
            foreach (KeyValuePair<NodalDegreeOfFreedom, double> kvp in react)
            {
                this.AddReaction(kvp.Key, kvp.Value);
            }
        }
    }
}
