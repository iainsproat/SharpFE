//-----------------------------------------------------------------------
// <copyright file="ForceRepository.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    internal class ForceRepository : Repository<ForceVector>
    {
        /// <summary>
        /// A reverse index for quickly getting all the forces which relate to a node
        /// </summary>
        private Index<IFiniteElementNode, ForceVector> nodalForces = new Index<IFiniteElementNode, ForceVector>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ForceRepository" /> class.
        /// </summary>
        public ForceRepository()
        {
            // empty
        }
        
        /// <summary>
        /// Allows a force vector to be applied to a specific finite element node.
        /// </summary>
        /// <param name="forceToApply">The force vector to apply to the node</param>
        /// <param name="nodeToApplyTo">The node to which the force will be applied.</param>
        /// <exception cref="ArgumentException">The force has not previous been registered with this repository.  The force needs to have been created via an instance of the ForceFactory class which was initialized with this repository as a parameter"</exception>
        public void ApplyForceToNode(ForceVector forceToApply, FiniteElementNode nodeToApplyTo)
        {
            Guard.AgainstBadArgument(
                () => { return !this.Contains(forceToApply); },
                    "The force has not previously been registered with this repository.  The force needs to have been created via an instance of the ForceFactory class which was initialized with this repository as a parameter",
                    "forceToApply");
            
            this.nodalForces.Add(nodeToApplyTo, forceToApply);
        }
        
        /// <summary>
        /// Gets a list of all forces applied to a particular node.
        /// </summary>
        /// <param name="node">The node for which all applied forces will be sought.</param>
        /// <returns>A list of all forces applied to the provided node.</returns>
        public IList<ForceVector> GetAllForcesAppliedTo(IFiniteElementNode node)
        {
            Guard.AgainstNullArgument(node, "node");
            
            IList<ForceVector> response = this.nodalForces.Get(node);
            if (response == null)
            {
                return new List<ForceVector>();
            }
            
            return response;
        }
        
        /// <summary>
        /// Calculates the combination of all forces applied to a given node.
        /// </summary>
        /// <param name="node">The node for which we wish to calculate the combined force vector</param>
        /// <returns>A single force vector representing the combined force on the node</returns>
        public ForceVector GetCombinedForceOn(IFiniteElementNode node) // TODO filter for different loadcases
        {
            Guard.AgainstNullArgument(node, "node");
            
            IList<ForceVector> forces = this.GetAllForcesAppliedTo(node);
            if (forces.IsEmpty())
            {
                return ForceVector.Zero;
            }
            
            KeyedVector<DegreeOfFreedom> combined = new ForceVector(0, 0);
            foreach (ForceVector f in forces)
            {
                combined = combined.Add(f);
            }
            
            return new ForceVector(combined);
        }
        
        /// <summary>
        /// Calculates the combined forces for each requested node and degree of freedom combination, and returns these values as a vector.
        /// The index of the vector corresponds to the same index of the provided nodalDegreesOfFreedoms parameter.
        /// </summary>
        /// <param name="nodalDegreeOfFreedoms">A list of all the <see cref="NodalDegreeOfFreedom">Nodal Degree of Freedoms</see> for which we wish to get a force.</param>
        /// <returns>A vector of forces at each node for each degree of freedom on that node.</returns>
        public KeyedVector<NodalDegreeOfFreedom> GetCombinedForcesFor(IList<NodalDegreeOfFreedom> nodalDegreeOfFreedoms)
        {
            Guard.AgainstNullArgument(nodalDegreeOfFreedoms, "nodalDegreeOfFreedoms");
            
            KeyedVector<NodalDegreeOfFreedom> result = new KeyedVector<NodalDegreeOfFreedom>(nodalDegreeOfFreedoms);
            IDictionary<IFiniteElementNode, ForceVector> cache = new Dictionary<IFiniteElementNode, ForceVector>();
            ForceVector combinedForceOnNode;

            foreach (NodalDegreeOfFreedom item in nodalDegreeOfFreedoms)
            {
                if (item == null || item.Node == null)
                {
                    // garbage in, garbage out!
                    result[item] = 0;
                    continue;
                }
                
                if (cache.ContainsKey(item.Node))
                {
                    combinedForceOnNode = cache[item.Node];
                    result[item] = combinedForceOnNode[item.DegreeOfFreedom];
                }
                else
                {
                    result[item] = this.GetCombinedForceFor(item, out combinedForceOnNode);
                    cache.Add(item.Node, combinedForceOnNode);
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Registers a new force vector with the repository.
        /// </summary>
        /// <param name="item">The new force vector which is to be registered in this repository.</param>
        protected override void AddToRepository(SharpFE.ForceVector item)
        {
            // empty
        }
        
        /// <summary>
        /// Clears the indices of this repository
        /// </summary>
        protected override void ClearRepository()
        {
            this.nodalForces.Clear();
        }
        
        /// <summary>
        /// Removes an item from the indices of this repository
        /// </summary>
        /// <param name="item">The item to remove from the indices of this repository</param>
        /// <returns>true if the item was successfully removed from at least one key in one indice; otherwise, false. false is also returned if the item was not present in any indices.</returns>
        protected override bool RemoveItem(ForceVector item)
        {
            return this.nodalForces.RemoveValue(item);
        }
        
        /// <summary>
        /// Calculates the component of the combined force for the requested node and degree of freedom combination.
        /// </summary>
        /// <param name="item">The <see cref="NodalDegreeOfFreedom">Nodal Degree of Freedom</see> for which we wish to get a component of force.</param>
        /// <param name="combinedForceOnNode">The combined force on the node for all degrees of freedom of that node.</param>
        /// <returns>the component of the applied force at the requested node for the specified degree of freedom on that node.</returns>
        private double GetCombinedForceFor(NodalDegreeOfFreedom item, out ForceVector combinedForceOnNode)
        {
            Guard.AgainstNullArgument(item, "item");
            
            combinedForceOnNode = this.GetCombinedForceOn(item.Node);
            
            return combinedForceOnNode[item.DegreeOfFreedom];
        }
    }
}
