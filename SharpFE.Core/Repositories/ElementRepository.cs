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
    using SharpFE.Cache;

    /// <summary>
    /// Stores elements and manages reverse indexes of values in element properties
    /// </summary>
    internal class ElementRepository : Repository<IFiniteElement>
    {
        /// <summary>
        /// A reverse index for quickly looking up the elements to which finite elment nodes are linked.
        /// </summary>
        private Index<IFiniteElementNode, IFiniteElement> nodeToElementIndex = new Index<IFiniteElementNode, IFiniteElement>();
        
        /// <summary>
        /// A reverse index for quickly looking up other nodes to which finite element nodes are linked via finite elements.
        /// </summary>
        private Index<IFiniteElementNode, IFiniteElementNode> nodeToNodeIndex = new Index<IFiniteElementNode, IFiniteElementNode>();
        
        /// <summary>
        /// 
        /// </summary>
        private Cache<ElementRepository.NodeTuple, IList<IFiniteElement>> cacheConnectingElements = new Cache<ElementRepository.NodeTuple, IList<IFiniteElement>>();
        
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
        public IList<IFiniteElement> GetAllElementsConnectedTo(IFiniteElementNode node)
        {
            return this.nodeToElementIndex.Get(node);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns>
        /// A list of elements directly connecting both the nodes.
        /// If node1 is equal to node2 (i.e. they are the same nodes), then all the elements connected to that node will be returned.
        /// </returns>
        public IList<IFiniteElement> GetAllElementsDirectlyConnecting(IFiniteElementNode node1, IFiniteElementNode node2)
        {
            Guard.AgainstNullArgument(node1, "node1");
            Guard.AgainstNullArgument(node2, "node2");
            
            IList<IFiniteElement> connectingElements;
            
            int currentValidHashForCache = this.GetHashCode();
            ElementRepository.NodeTuple keyForCache = new ElementRepository.NodeTuple(node1, node2);
            return this.cacheConnectingElements.GetOrCreateAndSave(keyForCache, currentValidHashForCache,
                                                            () => {
                                                                IList<IFiniteElement> elementsConnectedToNode1 = this.GetAllElementsConnectedTo(node1);
                                                                
                                                                if (node1.Equals(node2))
                                                                {
                                                                    connectingElements = elementsConnectedToNode1;
                                                                }
                                                                else
                                                                {
                                                                    IList<IFiniteElement> elementsConnectedToNode2 = this.GetAllElementsConnectedTo(node2);
                                                                    connectingElements = elementsConnectedToNode1.Intersect(elementsConnectedToNode2).ToList();
                                                                }
                                                                
                                                                return connectingElements;
                                                            });
        }
        
        /// <summary>
        /// Gets all the other nodes to which a node is directly connected via finite elements.
        /// </summary>
        /// <param name="node">The node for which to search for connected nodes.</param>
        /// <returns>A list of all connected nodes.</returns>
        public IList<IFiniteElementNode> GetAllNodesConnectedViaElementsTo(IFiniteElementNode node)
        {
            return this.nodeToNodeIndex.Get(node);
        }
        
        /// <summary>
        /// Registers a new finite element in the repository, and adds it to the various indexes as necessary.
        /// </summary>
        /// <param name="newItemToRegister">The new finite element to register.</param>
        /// <exception cref="ArgumentException">Thrown if the finite element already exists in the register.</exception>
        protected override void AddToRepository(IFiniteElement newItemToRegister)
        {
            this.RegisterNewElement(newItemToRegister, newItemToRegister.Nodes);
        }
        
        /// <summary>
        /// Removes an item from the indices of the repository
        /// </summary>
        /// <param name="item">the item to remove from the indices of the repository</param>
        /// <returns>true if the item was successfully removed; otherwise, false.  Returns false if the item does not exist in any indice.</returns>
        protected override bool RemoveItem(IFiniteElement item)
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
        private void RegisterNewElement(IFiniteElement element, IList<IFiniteElementNode> nodes)
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
        private bool RemoveItemFromRepositoryIndices(IFiniteElement element, IList<IFiniteElementNode> nodes)
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
        
        /// <summary>
        /// 
        /// </summary>
        private struct NodeTuple : IEquatable<NodeTuple>
        {
            /// <summary>
            /// 
            /// </summary>
            private IFiniteElementNode n1;
            
            /// <summary>
            /// 
            /// </summary>
            private IFiniteElementNode n2;
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="node1"></param>
            /// <param name="node2"></param>
            public NodeTuple(IFiniteElementNode node1, IFiniteElementNode node2)
            {
                this.n1 = node1;
                this.n2 = node2;
            }
            
            #region Equals and GetHashCode implementation
            /// <summary>
            /// 
            /// </summary>
            /// <param name="lhs"></param>
            /// <param name="rhs"></param>
            /// <returns></returns>
            public static bool operator ==(ElementRepository.NodeTuple lhs, ElementRepository.NodeTuple rhs)
            {
                return lhs.Equals(rhs);
            }
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="lhs"></param>
            /// <param name="rhs"></param>
            /// <returns></returns>
            public static bool operator !=(ElementRepository.NodeTuple lhs, ElementRepository.NodeTuple rhs)
            {
                return !(lhs == rhs);
            }
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public override bool Equals(object obj)
            {
                return (obj is ElementRepository.NodeTuple) && this.Equals((ElementRepository.NodeTuple)obj);
            }
            
            /// <summary>
            /// Order of nodes is ignored
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool Equals(ElementRepository.NodeTuple other)
            {
                return (object.Equals(this.n1, other.n1) && object.Equals(this.n2, other.n2))
                    || (object.Equals(this.n1, other.n2) && object.Equals(this.n2, other.n1));
            }
            
            /// <summary>
            /// Order of nodes is ignored
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                int hashCode = 0;
                unchecked
                {
                    if (this.n1 != null)
                    {
                        hashCode += 1000000007 * this.n1.GetHashCode();
                    }
                    
                    if (this.n2 != null)
                    {
                        hashCode += 1000000007 * this.n2.GetHashCode();
                    }
                }
                
                return hashCode;
            }
            #endregion
        }
    }
}
