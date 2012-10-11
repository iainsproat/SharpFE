//-----------------------------------------------------------------------
// <copyright file="NodeRepository.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains all nodes
    /// </summary>
    internal class NodeRepository : Repository<FiniteElementNode>
    {
        /// <summary>
        /// Stores the nodal degrees of freedom which are constrained, and those which are not.
        /// </summary>
        private UniqueIndex<bool, NodalDegreeOfFreedom> constrainedNodalDegreesOfFreedom = new UniqueIndex<bool, NodalDegreeOfFreedom>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeRepository" /> class.
        /// </summary>
        /// <param name="typeOfModel">The type of model for which to store nodes</param>
        public NodeRepository(ModelType typeOfModel)
        {
            this.ModelType = typeOfModel;
        }
        
        /// <summary>
        /// Gets the type of model for which this repository stores nodes
        /// </summary>
        public ModelType ModelType
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets all node and degree of freedom combinations which are constrained.
        /// </summary>
        /// <returns>A list of all constrained node and degree of freedom combinations</returns>
        public IList<NodalDegreeOfFreedom> ConstrainedNodalDegreeOfFreedoms
        {
            get
            {
                return new List<NodalDegreeOfFreedom>(this.constrainedNodalDegreesOfFreedom[true]);
            }
        }
        
        /// <summary>
        /// Gets all node and degree of freedom combinations which are not constrained.
        /// </summary>
        /// <returns>A list of all unconstrained node and degree of freedom combinations</returns>
        public IList<NodalDegreeOfFreedom> UnconstrainedNodalDegreeOfFreedoms
        {
            get
            {
                return new List<NodalDegreeOfFreedom>(this.constrainedNodalDegreesOfFreedom[false]);
            }
        }
        
        /// <summary>
        /// Constrains a node in a given degree of freedom.
        /// </summary>
        /// <param name="node">The node to constrain</param>
        /// <param name="degreeOfFreedomToConstrain">the degree of freedom in which to constrain the node.</param>
        public void ConstrainNode(FiniteElementNode node, DegreeOfFreedom degreeOfFreedomToConstrain)
        {
            NodalDegreeOfFreedom nodalDegreeOfFreedomToConstrain = new NodalDegreeOfFreedom(node, degreeOfFreedomToConstrain);
            this.ConstrainNode(nodalDegreeOfFreedomToConstrain);
        }
        
        /// <summary>
        /// Constrains a node in a given degree of freedom.
        /// </summary>
        /// <param name="nodalDegreeOfFreedomToConstrain">The node and degree of freedom to constrain</param>
        public void ConstrainNode(NodalDegreeOfFreedom nodalDegreeOfFreedomToConstrain)
        {
            this.constrainedNodalDegreesOfFreedom.Add(true, nodalDegreeOfFreedomToConstrain);
        }
        
        /// <summary>
        /// Frees a node in a given degree of freedom.
        /// </summary>
        /// <param name="node">The node to free</param>
        /// <param name="degreeOfFreedomToFree">the degree of freedom in which to free the node</param>
        public void UnconstrainNode(FiniteElementNode node, DegreeOfFreedom degreeOfFreedomToFree)
        {
            NodalDegreeOfFreedom nodalDegreeOfFreedomToFree = new NodalDegreeOfFreedom(node, degreeOfFreedomToFree);
            this.UnconstrainNode(nodalDegreeOfFreedomToFree);
        }
        
        /// <summary>
        /// Frees a node in a given degree of freedom.
        /// </summary>
        /// <param name="nodalDegreeOfFreedomToFree">The node and degree of freedom to free</param>
        public void UnconstrainNode(NodalDegreeOfFreedom nodalDegreeOfFreedomToFree)
        {
            this.constrainedNodalDegreesOfFreedom.Add(false, nodalDegreeOfFreedomToFree);
        }
        
        /// <summary>
        /// Determines whether a node is constrained in the given degree of freedom
        /// </summary>
        /// <param name="nodeToCheck">the node to check whether it is constrain in a particular degree of freedom</param>
        /// <param name="degreeOfFreedomToCheck">The particular degree of freedom of the node to check for constrainment</param>
        /// <returns>True if this particular node and degree of freedom are constrained; otherwise, false.</returns>
        public bool IsConstrained(FiniteElementNode nodeToCheck, DegreeOfFreedom degreeOfFreedomToCheck)
        {
            NodalDegreeOfFreedom nodeDof = new NodalDegreeOfFreedom(nodeToCheck, degreeOfFreedomToCheck);
            return this.IsConstrained(nodeDof);
        }
        
        /// <summary>
        /// Determines whether a node is constrained in the given degree of freedom
        /// </summary>
        /// <param name="nodalDegreeOfFreedomToCheck">The node and degree of freedom to check whether it is constrained</param>
        /// <returns>True if this particular node and degree of freedom are constrained; otherwise, false.</returns>
        public bool IsConstrained(NodalDegreeOfFreedom nodalDegreeOfFreedomToCheck)
        {
            return this.constrainedNodalDegreesOfFreedom.KeyOfValue(nodalDegreeOfFreedomToCheck);
        }
        
        /// <summary>
        /// Register a new node with this repository
        /// </summary>
        /// <param name="item">The new item to register</param>
        protected override void AddToRepository(FiniteElementNode item)
        {
            Guard.AgainstNullArgument(item, "item");
            
            IList<DegreeOfFreedom> allowedDegreesOfFreedom = this.ModelType.GetAllowedDegreesOfFreedomForBoundaryConditions();
            foreach (DegreeOfFreedom degreeOfFreedom in allowedDegreesOfFreedom)
            {
                this.constrainedNodalDegreesOfFreedom.Add(false, new NodalDegreeOfFreedom(item, degreeOfFreedom));
            }
        }
        
        /// <summary>
        /// Clears the indices of this repository
        /// </summary>
        protected override void ClearRepository()
        {
            this.constrainedNodalDegreesOfFreedom.Clear();
        }
        
        /// <summary>
        /// Removes an item from the indices of this repository
        /// </summary>
        /// <param name="item">The item to remove from the indices of this repository</param>
        /// <returns>true if the item was successfully removed from at least one key in one indice; otherwise, false. false is also returned if the item was not present in any indices.</returns>
        protected override bool RemoveItem(FiniteElementNode item)
        {
            bool success = false;
            IList<DegreeOfFreedom> supportedDegreesOfFreedom = this.ModelType.GetAllowedDegreesOfFreedomForBoundaryConditions();
            
            foreach (DegreeOfFreedom degreeOfFreedom in supportedDegreesOfFreedom)
            {
                if (this.constrainedNodalDegreesOfFreedom.RemoveValue(new NodalDegreeOfFreedom(item, degreeOfFreedom)))
                {
                    success = true;
                }
            }
            
            return success;
        }
    }
}
