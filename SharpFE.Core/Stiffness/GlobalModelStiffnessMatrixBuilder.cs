//-----------------------------------------------------------------------
// <copyright file="GlobalModelStiffnessMatrixBuilder.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Builds the global stiffness matrix by summing all the
    /// individual finite element stiffness matrices
    /// </summary>
    public class GlobalModelStiffnessMatrixBuilder
    {
        /// <summary>
        /// The finite element model from which to build the matrices
        /// </summary>
        private ITopologyQueryable parent;
        
        private IModelConstraintProvider constraintProvider;
        
        /// <summary>
        /// 
        /// </summary>
        private IElementStiffnessMatrixBuilderFactory stiffnessMatrixBuilderFactory;
        
        /// <summary>
        /// 
        /// </summary>
        private IDictionary<int, IElementStiffnessCalculator> elementStiffnessProviderCache = new Dictionary<int, IElementStiffnessCalculator>();
        
        /// <summary>
        /// 
        /// </summary>
        private IDictionary<Tuple<NodalDegreeOfFreedom, NodalDegreeOfFreedom>, double> stiffnessCache = new Dictionary<Tuple<NodalDegreeOfFreedom, NodalDegreeOfFreedom>, double>();
        
        public GlobalModelStiffnessMatrixBuilder(FiniteElementModel model)
            : this( model, model)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalModelStiffnessMatrixBuilder" /> class.
        /// </summary>
        /// <param name="parentModel">The model from which the stiffness matrices will be built.</param>
        public GlobalModelStiffnessMatrixBuilder(ITopologyQueryable parentModel, IModelConstraintProvider modelConstraintProvider)
            : this(parentModel, modelConstraintProvider, new ElementStiffnessMatrixBuilderFactory())
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalModelStiffnessMatrixBuilder" /> class.
        /// </summary>
        /// <param name="parentModel"></param>
        /// <param name="elementStiffnessMatrixBuilderFactory"></param>
        public GlobalModelStiffnessMatrixBuilder(ITopologyQueryable parentModel, IModelConstraintProvider constraintProv, IElementStiffnessMatrixBuilderFactory elementStiffnessMatrixBuilderFactory)
        {
            this.parent = parentModel;
            this.constraintProvider = constraintProv;
            this.stiffnessMatrixBuilderFactory = elementStiffnessMatrixBuilderFactory; 
        }
        
        /// <summary>
        /// Gets a list of all the nodes and degrees of freedom which have a known force.
        /// </summary>
        /// <remarks>
        /// This is exactly the same as the list of unknown displacements.
        /// The presence of a force on a node does not necessarily mean that it is known.
        /// For example, an external force applied to a fixed node does not mean that you will know the reaction of that node (without calculating it).
        /// Counter intuitively, a node without an external force applied to does actually have a known force - the force is zero.
        /// </remarks>
        public IList<NodalDegreeOfFreedom> DegreesOfFreedomWithKnownForce
        {
            get
            {
                return this.constraintProvider.UnconstrainedNodalDegreeOfFreedoms;
            }
        }
        
        /// <summary>
        /// Gets a list of all the nodes and degrees of freedom which have a known displacement.
        /// </summary>
        /// <remarks>
        /// This will be all the nodes which are constrained in a particular degree of freedom.
        /// </remarks>
        public IList<NodalDegreeOfFreedom> DegreesOfFreedomWithKnownDisplacement
        {
            get
            {
                return this.constraintProvider.ConstrainedNodalDegreeOfFreedoms;
            }
        }
        
        /// <summary>
        /// Gets a list of all the nodes and degrees of freedom which have an unknown force.
        /// This is exactly the same list as those which have a known displacement.
        /// </summary>
        public IList<NodalDegreeOfFreedom> DegreesOfFreedomWithUnknownForce
        {
            get
            {
                return this.constraintProvider.ConstrainedNodalDegreeOfFreedoms;
            }
        }
        
        /// <summary>
        /// Gets a list of all the nodes and degrees of freedom which have an unknown displacement.
        /// </summary>
        /// <remarks>
        /// This will be all the degrees of freedom of each node which are not constrained.
        /// </remarks>
        public IList<NodalDegreeOfFreedom> DegreesOfFreedomWithUnknownDisplacement
        {
            get
            {
                return this.constraintProvider.UnconstrainedNodalDegreeOfFreedoms;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StiffnessMatrix BuildGlobalStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> allDegreesOfFreedom = this.constraintProvider.AllDegreesOfFreedom;
            
            return this.BuildStiffnessSubMatrix(allDegreesOfFreedom, allDegreesOfFreedom);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to known forces and known displacements
        /// </summary>
        /// <returns>A stiffness matrix for known forces and known displacements</returns>
        public StiffnessMatrix BuildKnownForcesKnownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> knownForces = this.DegreesOfFreedomWithKnownForce;
            
            IList<NodalDegreeOfFreedom> knownDisplacements = this.DegreesOfFreedomWithKnownDisplacement;
            Guard.AgainstInvalidState(() => { return knownDisplacements == null || knownDisplacements.IsEmpty(); },
                                      "The model has no constraints and therefore cannot be solved");
                        
            return this.BuildStiffnessSubMatrix(knownForces, knownDisplacements);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to known forces and unknown displacements
        /// </summary>
        /// <returns>A stiffness matrix for known forces and unknown displacements</returns>
        public StiffnessMatrix BuildKnownForcesUnknownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> knownForceIdentifiers = this.DegreesOfFreedomWithKnownForce;
            Guard.AgainstInvalidState(() => { return knownForceIdentifiers == null || knownForceIdentifiers.IsEmpty(); },
                                          "The model has too many constraints and no displacements will occur.  The reactions of each node equals the forces applied to each node.");
            
            IList<NodalDegreeOfFreedom> unknownDisplacementIdentifiers = this.DegreesOfFreedomWithUnknownDisplacement;
            
            return this.BuildStiffnessSubMatrix(knownForceIdentifiers, unknownDisplacementIdentifiers);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to unknown forces and known displacements
        /// </summary>
        /// <returns>A stiffness matrix for unknown forces and known displacements</returns>
        public StiffnessMatrix BuildUnknownForcesKnownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> unknownForces = this.DegreesOfFreedomWithUnknownForce;
            
            IList<NodalDegreeOfFreedom> knownDisplacements = this.DegreesOfFreedomWithKnownDisplacement;
            return this.BuildStiffnessSubMatrix(unknownForces, knownDisplacements);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to unknown forces and unknown displacements
        /// </summary>
        /// <returns>A stiffness matrix for unknown forces and unknown displacements</returns>
        public StiffnessMatrix BuildUnknownForcesUnknownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> unknownForces = this.DegreesOfFreedomWithUnknownForce;
            
            IList<NodalDegreeOfFreedom> unknownDisplacements = this.DegreesOfFreedomWithUnknownDisplacement;
            return this.BuildStiffnessSubMatrix(unknownForces, unknownDisplacements);
        }
        
        /// <summary>
        /// This function iterates through all the elements which provide stiffnesses for the given combination of nodal degree of freedoms
        /// and sums them to provide the total stiffness
        /// </summary>
        /// <param name="rowKeys">The nodal degree of freedoms which represent the rows of the matrix</param>
        /// <param name="columnKeys">The nodal degree of freedoms which represents the columns of the matrix</param>
        /// <returns>A matrix representing the stiffness for these combinations of rows and columns</returns>
        private StiffnessMatrix BuildStiffnessSubMatrix(IList<NodalDegreeOfFreedom> rowKeys, IList<NodalDegreeOfFreedom> columnKeys)
        {
            Guard.AgainstNullArgument(rowKeys, "rowKeys");
            Guard.AgainstNullArgument(columnKeys, "columnKeys");
            
            int numRows = rowKeys.Count;
            int numCols = columnKeys.Count;
            
            Guard.AgainstBadArgument(
                "rowKeys",
                () => { return numRows == 0; },
                "There must be at least one row");
            Guard.AgainstBadArgument(
                "columnKeys",
                () => { return numCols == 0; },
                "There must be at least one column");
            
            StiffnessMatrix result = new StiffnessMatrix(rowKeys, columnKeys);
            
            IList<IFiniteElement> connectedElements;
            foreach (NodalDegreeOfFreedom row in rowKeys)
            {
                foreach (NodalDegreeOfFreedom column in columnKeys)
                {
                    connectedElements = this.parent.AllElementsDirectlyConnecting(row.Node, column.Node);
                    double currentResult = this.SumStiffnessesForAllElementsAt(connectedElements, row, column);
                    if (!currentResult.IsApproximatelyEqualTo(0.0))
                    {
                        result.At(row, column, currentResult);
                    }
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Sums all the stiffnesses across all elements which are connected to the given nodes
        /// </summary>
        /// <param name="elementsDirectlyConnectingRowAndColumnNodes">The list of all directly connected elements between both the row and column node</param>
        /// <param name="row">The node and degree of freedom which define the row</param>
        /// <param name="column">The node and degree of freedom which define the column</param>
        /// <returns>A double representing the stiffness for this element</returns>
        private double SumStiffnessesForAllElementsAt(IList<IFiniteElement> elementsDirectlyConnectingRowAndColumnNodes, NodalDegreeOfFreedom row, NodalDegreeOfFreedom column)
        {
            double totalStiffness = 0.0;
            
            Tuple<NodalDegreeOfFreedom, NodalDegreeOfFreedom> cacheKey = new Tuple<NodalDegreeOfFreedom, NodalDegreeOfFreedom>(row, column);
            if (this.stiffnessCache.TryGetValue(cacheKey, out totalStiffness))
            {
                return totalStiffness;
            }
            
            foreach (IFiniteElement e in elementsDirectlyConnectingRowAndColumnNodes)
            {
                IElementStiffnessCalculator elementStiffnessMatrixBuilder = this.GetElementStiffnessProvider(e);
                totalStiffness += elementStiffnessMatrixBuilder.GetStiffnessInGlobalCoordinatesAt(row.Node, row.DegreeOfFreedom, column.Node, column.DegreeOfFreedom);
            }
            
            this.stiffnessCache[cacheKey] = totalStiffness;
            
            return totalStiffness;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private IElementStiffnessCalculator GetElementStiffnessProvider(IFiniteElement element)
        {
            int elementHash = element.GetHashCode();
            
            // check the cache, and retrieve if available
            if (this.elementStiffnessProviderCache.ContainsKey(elementHash))
            {
                return (IElementStiffnessCalculator)this.elementStiffnessProviderCache[elementHash];
            }
            
            IElementStiffnessCalculator builder = this.stiffnessMatrixBuilderFactory.Create(element);
            
            // store in the cache
            this.elementStiffnessProviderCache.Add(elementHash, builder);
            return builder;
        }
    }
}
