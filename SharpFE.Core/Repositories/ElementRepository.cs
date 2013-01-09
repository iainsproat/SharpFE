//-----------------------------------------------------------------------
// <copyright file="ElementRepository.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MathNet.Numerics.LinearAlgebra.Double;

    /// <summary>
    /// Stores elements and manages reverse indexes of values in element properties
    /// </summary>
    internal class ElementRepository : Repository<FiniteElement>
    {
        /// <summary>
        /// A reverse index for quickly looking up the elements to which finite elment nodes are linked.
        /// </summary>
        private Index<FiniteElementNode, FiniteElement> nodeToElementIndex = new Index<FiniteElementNode, FiniteElement>();
        
        /// <summary>
        /// A reverse index for quickly looking up other nodes to which finite element nodes are linked via finite elements.
        /// </summary>
        private Index<FiniteElementNode, FiniteElementNode> nodeToNodeIndex = new Index<FiniteElementNode, FiniteElementNode>();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementRepository" /> class.
        /// </summary>
        public ElementRepository()
        {
            // empty
        }
        
        /// <summary>
        /// Gets all the finite elements to which a node is connected.
        /// </summary>
        /// <param name="node">The node for which to search for connected finite elements.</param>
        /// <returns>A list of all finite elements connecting the node.</returns>
        public IList<FiniteElement> GetAllElementsConnectedTo(FiniteElementNode node)
        {
            return this.nodeToElementIndex.Get(node);
        }
        
        public IList<FiniteElement> GetAllElementsDirectlyConnecting(FiniteElementNode node1, FiniteElementNode node2)
        {
            Guard.AgainstNullArgument(node1, "node1");
            Guard.AgainstNullArgument(node2, "node2");
            
            IList<FiniteElement> elementsConnectedToNode1 = this.GetAllElementsConnectedTo(node1);
            IList<FiniteElement> elementsConnectedToNode2 = this.GetAllElementsConnectedTo(node2);
            return new List<FiniteElement>(elementsConnectedToNode1.Intersect(elementsConnectedToNode2));
            
            ////TODO cache results, and also node2,node1 (which will be the same)
        }
        
        /// <summary>
        /// Gets all the other nodes to which a node is directly connected via finite elements.
        /// </summary>
        /// <param name="node">The node for which to search for connected nodes.</param>
        /// <returns>A list of all connected nodes.</returns>
        public IList<FiniteElementNode> GetAllNodesConnectedViaElementsTo(FiniteElementNode node)
        {
            return this.nodeToNodeIndex.Get(node);
        }
        
        /// <summary>
        /// Registers a new finite element in the repository, and adds it to the various indexes as necessary.
        /// </summary>
        /// <param name="newItemToRegister">The new finite element to register.</param>
        /// <exception cref="ArgumentException">Thrown if the finite element already exists in the register.</exception>
        protected override void AddToRepository(FiniteElement newItemToRegister)
        {
            this.RegisterNewElement(newItemToRegister, newItemToRegister.Nodes);
        }
        
        /// <summary>
        /// Removes an item from the indices of the repository
        /// </summary>
        /// <param name="item">the item to remove from the indices of the repository</param>
        /// <returns>true if the item was successfully removed; otherwise, false.  Returns false if the item does not exist in any indice.</returns>
        protected override bool RemoveItem(FiniteElement item)
        {
            return this.RemoveItemFromRepositoryIndices(item, item.Nodes);
        }
        
        /// <summary>
        /// Clears the indices of the repository
        /// </summary>
        protected override void ClearRepository()
        {
            this.nodeToElementIndex.Clear();
            this.nodeToNodeIndex.Clear();
        }
        
        /// <summary>
        /// Registers a new finite element in the repository, and adds it to the various indexes as necessary.
        /// </summary>
        /// <param name="element">The new finite element to register.</param>
        /// <param name="nodes">The nodes which form part of the element and should be used to update the various indexes.</param>
        private void RegisterNewElement(FiniteElement element, IList<FiniteElementNode> nodes)
        {
            int numNodes = nodes.Count;
            for (int i = 0; i < numNodes; i++)
            {
                this.nodeToElementIndex.Add(nodes[i], element);
                
                for (int j = i + 1; j < numNodes; j++)
                {
                    this.nodeToNodeIndex.Add(nodes[i], nodes[j]);
                    this.nodeToNodeIndex.Add(nodes[j], nodes[i]);
                }
            }
        }
        
        /// <summary>
        /// Removes an item from the indices of this repository
        /// </summary>
        /// <param name="element">the item to remove</param>
        /// <param name="nodes">the nodes of the item to remove</param>
        /// <returns>true if the item was successfully removed; otherwise, false. Returns false if the item was not found in any index</returns>
        private bool RemoveItemFromRepositoryIndices(FiniteElement element, IList<FiniteElementNode> nodes)
        {
            bool success = false;
            int numNodes = nodes.Count;
            for (int i = 0; i < numNodes; i++)
            {
                if (this.nodeToElementIndex.Remove(nodes[i], element))
                {
                    success = true;
                }
                
                for (int j = i + 1; j < numNodes; j++)
                {
                    if (this.nodeToNodeIndex.Remove(nodes[i], nodes[j]))
                    {
                        success = true;
                    }
                    
                    if (this.nodeToNodeIndex.Remove(nodes[j], nodes[i]))
                    {
                        success = true;
                    }
                }
            }
            
            return success;
        }
    }
}
