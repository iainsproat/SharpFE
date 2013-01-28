//-----------------------------------------------------------------------
// <copyright file="FiniteElementModel.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using SharpFE;

    /// <summary>
    /// A finite element model is composed of nodes connected by finite elements.
    /// These nodes can be constrained and can have forces applied to them.
    /// </summary>
    public class FiniteElementModel : IModelConstraintProvider, ITopologyQueryable, IEquatable<FiniteElementModel>
    {
        /// <summary>
        /// The finite element nodes of this model
        /// </summary>
        private NodeRepository nodes;
        
        /// <summary>
        /// The finite elements in this model
        /// </summary>
        private ElementRepository elements;
        
        /// <summary>
        /// The forces in this model
        /// </summary>
        private ForceRepository forces;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FiniteElementModel" /> class.
        /// </summary>
        /// <param name="typeOfModel">The axes in which this model is constrained and the type of analysis expected on it.</param>
        public FiniteElementModel(ModelType typeOfModel)
        {
            this.ModelType = typeOfModel;
            this.nodes = new NodeRepository(this.ModelType);
            this.elements = new ElementRepository();
            this.forces = new ForceRepository();
            
            this.NodeFactory = new NodeFactory(this.ModelType, this.nodes);
            this.ElementFactory = new ElementFactory(this.elements);
            this.ForceFactory = new ForceFactory(this.ModelType, this.forces);
        }
        
        /// <summary>
        /// Gets the ModelType of this model.  This defines the axes in which this model is constrained and the type of analysis expected on this model.
        /// </summary>
        public ModelType ModelType
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the <see cref="NodeFactory" /> which is used to create nodes in this model.
        /// </summary>
        public NodeFactory NodeFactory
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the <see cref="ElementFactory" /> which is used to create elements in this model.
        /// </summary>
        public ElementFactory ElementFactory
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the <see cref="ForceFactory" /> which is used to create forces in this model.
        /// Note that the created forces will not yet be applied to any nodes and this must be done ApplyForceToNode method.
        /// </summary>
        public ForceFactory ForceFactory
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the number of nodes in this model
        /// </summary>
        public int NodeCount
        {
            get
            {
                return this.nodes.Count;
            }
        }
        
        /// <summary>
        /// Gets the number of finite elements in this model
        /// </summary>
        public int ElementCount
        {
            get
            {
                return this.elements.Count;
            }
        }
        
        /// <summary>
        /// Gets all the possible degrees of freedom in this entire model
        /// </summary>
        public IList<NodalDegreeOfFreedom> AllDegreesOfFreedom
        {
            get
            {
                IList<DegreeOfFreedom> allowedBoundaryConditionDegreeOfFreedoms = this.ModelType.GetAllowedDegreesOfFreedomForBoundaryConditions();
                IList<NodalDegreeOfFreedom> result = new List<NodalDegreeOfFreedom>(this.NodeCount * allowedBoundaryConditionDegreeOfFreedoms.Count);
                foreach (FiniteElementNode node in this.nodes)
                {
                    foreach (DegreeOfFreedom dof in allowedBoundaryConditionDegreeOfFreedoms)
                    {
                        result.Add(new NodalDegreeOfFreedom(node, dof));
                    }
                }
                
                return result;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IList<NodalDegreeOfFreedom> ConstrainedNodalDegreeOfFreedoms
        {
            get
            {
                return this.nodes.ConstrainedNodalDegreeOfFreedoms;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IList<NodalDegreeOfFreedom> UnconstrainedNodalDegreeOfFreedoms
        {
            get
            {
                return this.nodes.UnconstrainedNodalDegreeOfFreedoms;
            }
        }
                        
        #region Equals and GetHashCode implementation
                /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="leftHandSide">The object on the left of the equality operator</param>
        /// <param name="rightHandSide">The object to the right of the equality operator</param>
        /// <returns>True if the model at the left hand side and the right hand side are equal.</returns>
        public static bool operator ==(FiniteElementModel leftHandSide, FiniteElementModel rightHandSide)
        {
            if (ReferenceEquals(leftHandSide, rightHandSide))
            {
                return true;
            }
            
            if (ReferenceEquals(leftHandSide, null) || ReferenceEquals(rightHandSide, null))
            {
                return false;
            }
            
            return leftHandSide.Equals(rightHandSide);
        }
        
        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="leftHandSide">The object to the left of the inequality operator</param>
        /// <param name="rightHandSide">The object to the right of the inequality operator</param>
        /// <returns>True if the models are not equal, false otherwise.</returns>
        public static bool operator !=(FiniteElementModel leftHandSide, FiniteElementModel rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        /// <summary>
        /// The HashCode of the model
        /// </summary>
        /// <returns>An integer representing the HashCode of the model</returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                if (this.nodes != null)
                {
                    hashCode += 1000000007 * this.nodes.GetHashCode();
                }
                
                if (this.elements != null)
                {
                    hashCode += 1000000009 * this.elements.GetHashCode();
                }
                
                if (this.forces != null)
                {
                    hashCode += 1000000021 * this.forces.GetHashCode();
                }
                
                hashCode += 1000000033 * this.ModelType.GetHashCode();
            }
            
            return hashCode;
        }
        
        /// <summary>
        /// Determines whether another object equals this.
        /// </summary>
        /// <param name="obj">The other object to compare to this.</param>
        /// <returns>True if the other object equals this model.</returns>
        public override bool Equals(object obj)
        {
            FiniteElementModel other = obj as FiniteElementModel;
            return this.Equals(other);
        }
        
        /// <summary>
        /// Determines whether another FiniteElementModel equals this.
        /// </summary>
        /// <param name="other">The other model to compare to this</param>
        /// <returns>True if the node repository, element repository, force repository and model type 
        /// of this model and the other model are equal.</returns>
        public bool Equals(FiniteElementModel other)
        {
            if (other == null)
            {
                return false;
            }
            
            return object.Equals(this.nodes, other.nodes)
                && object.Equals(this.elements, other.elements)
                && object.Equals(this.forces, other.forces)
                && this.ModelType == other.ModelType;
        }
        #endregion
        
        /// <summary>
        /// Constrains a node in a given degree of freedom.
        /// </summary>
        /// <param name="node">The node to constrain</param>
        /// <param name="degreeOfFreedomToConstrain">the degree of freedom in which to constrain the node.</param>
        public void ConstrainNode(FiniteElementNode node, DegreeOfFreedom degreeOfFreedomToConstrain)
        {
            if (!this.ModelType.IsAllowedDegreeOfFreedomForBoundaryConditions(degreeOfFreedomToConstrain))
            {
                throw new ArgumentException("Cannot constrain this degree of freedom in this model type");
            }
            
            this.nodes.ConstrainNode(node, degreeOfFreedomToConstrain);
        }
        
        /// <summary>
        /// Frees a node in a given degree of freedom.
        /// </summary>
        /// <param name="node">The node to free</param>
        /// <param name="degreeOfFreedomToFree">the degree of freedom in which to free the node</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unconstrain", Justification = "Spelling is OK")]
        public void UnconstrainNode(FiniteElementNode node, DegreeOfFreedom degreeOfFreedomToFree)
        {
            this.nodes.UnconstrainNode(node, degreeOfFreedomToFree);
        }
        
        /// <summary>
        /// Determines whether a node is constrained in the given degree of freedom
        /// </summary>
        /// <param name="nodeToCheck">the node to check whether it is constrain in a particular degree of freedom</param>
        /// <param name="degreeOfFreedomToCheck">The particular degree of freedom of the node to check for constraint</param>
        /// <returns>True if this particular node and degree of freedom are constrained; otherwise, false.</returns>
        public bool IsConstrained(FiniteElementNode nodeToCheck, DegreeOfFreedom degreeOfFreedomToCheck)
        {
            return this.nodes.IsConstrained(nodeToCheck, degreeOfFreedomToCheck);
        }
        
        /// <summary>
        /// Determines which elements are connected to the provided node.
        /// </summary>
        /// <param name="node">A finite element node in this model.</param>
        /// <returns>A list of all elements which are connected to the given node.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the given node does not exist in the model or no elements are attached to it.
        /// </exception>
        public IList<IFiniteElement> AllElementsConnectedTo(FiniteElementNode node)
        {
            Guard.AgainstNullArgument(node, "node");
            
            IList<IFiniteElement> response = this.elements.GetAllElementsConnectedTo(node);
            if (response == null)
            {
                throw new ArgumentException("No elements could be found which are attached to the provided node.  Please ensure that the node is part of this finite element model and that you have connected an element to the node", "node");
            }
            
            return response;
        }
        
        /// <summary>
        /// Queries the model to find all the finite elements which directly connect to
        /// the two provided nodes.
        /// </summary>
        /// <param name="node1">The first node connected to the elements to be sought</param>
        /// <param name="node2">The second node connected to the elements to be sought</param>
        /// <returns>A list of all the finite elements which directly connect the two nodes.  An empty list is returned if the nodes are equal.</returns>
        public IList<IFiniteElement> AllElementsDirectlyConnecting(FiniteElementNode node1, FiniteElementNode node2)
        {
            return this.elements.GetAllElementsDirectlyConnecting(node1, node2);
        }
        
        /// <summary>
        /// This gets a force component value for each of the node and degree of freedom combinations of known forces.
        /// The index of each value corresponds to the index of each <see cref="NodalDegreeOfFreedom" /> returned by DegreeOfFreedomWithKnownForce.
        /// </summary>
        /// <returns>Vector of values of all the known force components.</returns>
        public KeyedVector<NodalDegreeOfFreedom> KnownForceVector()
        {
            IList<NodalDegreeOfFreedom> knownForces = this.UnconstrainedNodalDegreeOfFreedoms;
            
            KeyedVector<NodalDegreeOfFreedom> result = new KeyedVector<NodalDegreeOfFreedom>(knownForces);
            ForceVector nodalForce;
            
            foreach (NodalDegreeOfFreedom currentNodalDegreeOfFreedom in knownForces)
            {
                nodalForce = this.forces.GetCombinedForceOn(currentNodalDegreeOfFreedom.Node);
                result[currentNodalDegreeOfFreedom] = nodalForce.GetValue(currentNodalDegreeOfFreedom.DegreeOfFreedom);
            }
            
            return result;
        }
        
        /// <summary>
        /// This gets a displacement component value for each of the node and degree of freedom combinations of known displacements.
        /// The index of each value corresponds to the index of each <see cref="NodalDegreeOfFreedom" /> returned by DegreeOfFreedomWithKnownDisplacement.
        /// </summary>
        /// <returns>Vector of values of all the known displacement components.</returns>
        public KeyedVector<NodalDegreeOfFreedom> KnownDisplacementVector()
        {
            IList<NodalDegreeOfFreedom> knownDisplacements = this.nodes.ConstrainedNodalDegreeOfFreedoms;
            return new KeyedVector<NodalDegreeOfFreedom>(knownDisplacements, 0.0); // TODO allow for non-zero initial displacements
        }
        
        /// <summary>
        /// Applies an external force to a node in the model.
        /// Forces can be applied to one or more nodes by calling this method for each node you wish to apply the force to.
        /// </summary>
        /// <param name="forceToApply">The force vector to apply to a node</param>
        /// <param name="nodeToApplyTo">The node which the force will be applied to.</param>
        public void ApplyForceToNode(ForceVector forceToApply, FiniteElementNode nodeToApplyTo)
        {
            this.forces.ApplyForceToNode(forceToApply, nodeToApplyTo);
        }
        
        /// <summary>
        /// Calculates the combined forces for each requested node and degree of freedom combination, and returns these values as a vector.
        /// The index of the vector corresponds to the same index of the provided nodalDegreesOfFreedoms parameter.
        /// </summary>
        /// <param name="nodalDegreeOfFreedoms">A list of all the <see cref="NodalDegreeOfFreedom">Nodal Degree of Freedoms</see> for which we wish to get a force.</param>
        /// <returns>A vector of forces at each node for each degree of freedom on that node.</returns>
        public KeyedVector<NodalDegreeOfFreedom> GetCombinedForcesFor(IList<NodalDegreeOfFreedom> nodalDegreeOfFreedoms)
        {
            return this.forces.GetCombinedForcesFor(nodalDegreeOfFreedoms);
        }
    }
}
