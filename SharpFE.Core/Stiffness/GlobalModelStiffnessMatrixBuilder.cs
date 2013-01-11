//-----------------------------------------------------------------------
// <copyright file="StiffnessMatrixBuilder.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    using System.Collections.Generic;
    using SharpFE.Cache;
    
    /// <summary>
    /// Builds the global stiffness matrix by summing all the
    /// individual finite element stiffness matrices
    /// </summary>
    public class GlobalModelStiffnessMatrixBuilder
    {
        /// <summary>
        /// The finite element model from which to build the matrices
        /// </summary>
        private FiniteElementModel parent;
        
        private ElementStiffnessMatrixBuilderFactory stiffnessFactory = new ElementStiffnessMatrixBuilderFactory();
        
        private IDictionary<int, IStiffnessProvider> elementStiffnessProviderCache = new Dictionary<int, IStiffnessProvider>();
        private Cache<Tuple<NodalDegreeOfFreedom, NodalDegreeOfFreedom>, double> stiffnessCache = new Cache<Tuple<NodalDegreeOfFreedom, NodalDegreeOfFreedom>, double>();
        
        private int currentModelHash;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="StiffnessMatrixBuilder" /> class.
        /// </summary>
        /// <param name="parentModel">The model from which the stiffness matrices will be built.</param>
        public GlobalModelStiffnessMatrixBuilder(FiniteElementModel parentModel)
        {
            this.parent = parentModel;
            this.currentModelHash = this.parent.GetHashCode();
        }
        
        public StiffnessMatrix BuildGlobalStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> allDegreesOfFreedom = this.parent.AllDegreesOfFreedom;
            
            return this.BuildStiffnessSubMatrix(allDegreesOfFreedom, allDegreesOfFreedom);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to known forces and known displacements
        /// </summary>
        /// <returns>A stiffness matrix for known forces and known displacements</returns>
        public StiffnessMatrix BuildKnownForcesKnownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> knownForces = this.parent.DegreesOfFreedomWithKnownForce;
            
            IList<NodalDegreeOfFreedom> knownDisplacements = this.parent.DegreesOfFreedomWithKnownDisplacement;
            if (knownDisplacements == null || knownDisplacements.Count == 0)
            {
                throw new InvalidOperationException("The model has no constraints and therefore cannot be solved");
            }
            
            return this.BuildStiffnessSubMatrix(knownForces, knownDisplacements);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to known forces and unknown displacements
        /// </summary>
        /// <returns>A stiffness matrix for known forces and unknown displacements</returns>
        public StiffnessMatrix BuildKnownForcesUnknownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> knownForceIdentifiers = this.parent.DegreesOfFreedomWithKnownForce;
            if (knownForceIdentifiers == null || knownForceIdentifiers.Count == 0)
            {
                throw new InvalidOperationException("The model has too many constraints and no displacements will occur.  The reactions of each node equals the forces applied to each node.");
            }
            
            IList<NodalDegreeOfFreedom> unknownDisplacementIdentifiers = this.parent.DegreesOfFreedomWithUnknownDisplacement;
            
            return this.BuildStiffnessSubMatrix(knownForceIdentifiers, unknownDisplacementIdentifiers);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to unknown forces and known displacements
        /// </summary>
        /// <returns>A stiffness matrix for unknown forces and known displacements</returns>
        public StiffnessMatrix BuildUnknownForcesKnownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> unknownForces = this.parent.DegreesOfFreedomWithUnknownForce;
            
            IList<NodalDegreeOfFreedom> knownDisplacements = this.parent.DegreesOfFreedomWithKnownDisplacement;
            return this.BuildStiffnessSubMatrix(unknownForces, knownDisplacements);
        }
        
        /// <summary>
        /// The stiffness matrix which relates to unknown forces and unknown displacements
        /// </summary>
        /// <returns>A stiffness matrix for unknown forces and unknown displacements</returns>
        public StiffnessMatrix BuildUnknownForcesUnknownDisplacementStiffnessMatrix()
        {
            IList<NodalDegreeOfFreedom> unknownForces = this.parent.DegreesOfFreedomWithUnknownForce;
            
            IList<NodalDegreeOfFreedom> unknownDisplacements = this.parent.DegreesOfFreedomWithUnknownDisplacement;
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
            
            // update to ensure validity of items stored in cache
            this.currentModelHash = this.parent.GetHashCode();
            
            int numRows = rowKeys.Count;
            int numCols = columnKeys.Count;
            
            Guard.AgainstBadArgument(
                () => { return numRows == 0; },
                "There must be at least one row",
                "rowKeys");
            Guard.AgainstBadArgument(
                () => { return numCols == 0; },
                "There must be at least one column",
                "columnKeys");
            
            int modelHashCode = this.parent.GetHashCode();
            
            StiffnessMatrix result = new StiffnessMatrix(rowKeys, columnKeys);
            
            IList<FiniteElement> connectedElements;
            foreach (NodalDegreeOfFreedom row in rowKeys)
            {
                foreach (NodalDegreeOfFreedom column in columnKeys)
                {
                    connectedElements = this.parent.GetAllElementsDirectlyConnecting(row.Node, column.Node);
                    double currentResult = this.SumStiffnessesForAllElementsAt(connectedElements, row, column);
                    if (currentResult != 0)
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
        private double SumStiffnessesForAllElementsAt(IList<FiniteElement> elementsDirectlyConnectingRowAndColumnNodes, NodalDegreeOfFreedom row, NodalDegreeOfFreedom column)
        {
            double totalStiffness = 0.0;
            
            Tuple<NodalDegreeOfFreedom, NodalDegreeOfFreedom> cacheKey = new Tuple<NodalDegreeOfFreedom, NodalDegreeOfFreedom>(row, column);
            if (stiffnessCache.ContainsKey(cacheKey, this.currentModelHash, out totalStiffness))
            {
                return totalStiffness;
            }
            
            foreach (FiniteElement e in elementsDirectlyConnectingRowAndColumnNodes)
            {
                IStiffnessProvider elementStiffnessMatrixBuilder = this.GetElementStiffnessProvider(e);
                totalStiffness += elementStiffnessMatrixBuilder.GetGlobalStiffnessAt(row.Node, row.DegreeOfFreedom, column.Node, column.DegreeOfFreedom);
            }
            
            this.stiffnessCache.Save(cacheKey, totalStiffness, this.currentModelHash);
            
            return totalStiffness;
        }
        
        private IStiffnessProvider GetElementStiffnessProvider(FiniteElement element)
        {
            int elementHash = element.GetHashCode();
            
            // check the cache, and retrieve if available
            if (this.elementStiffnessProviderCache.ContainsKey(elementHash))
            {
                return (IStiffnessProvider)this.elementStiffnessProviderCache[elementHash];
            }
            
            IStiffnessProvider builder = this.stiffnessFactory.Create(element);
            
            // store in the cache
            this.elementStiffnessProviderCache.Add(elementHash, builder);
            return builder;
        }
        

    }
}
