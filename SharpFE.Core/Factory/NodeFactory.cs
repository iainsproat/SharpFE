//-----------------------------------------------------------------------
// <copyright file="NodeFactory.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Description of NodeFactory.
    /// </summary>
    public class NodeFactory
    {
        /// <summary>
        /// The type of model for which we will be creating new nodes.
        /// </summary>
        private ModelType modelType;
        
        /// <summary>
        /// The repository with which to register newly created nodes.
        /// </summary>
        private NodeRepository repo;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeFactory" /> class.
        /// </summary>
        /// <param name="typeOfModel">The type of model for which this factory will be creating new nodes.</param>
        internal NodeFactory(ModelType typeOfModel)
            : this(typeOfModel, null)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeFactory" /> class.
        /// </summary>
        /// <param name="typeOfModel">The type of model for which this factory will be creating new nodes.</param>
        /// <param name="repository">The repository with which to register newly created nodes.</param>
        internal NodeFactory(ModelType typeOfModel, NodeRepository repository)
        {
            this.modelType = typeOfModel;
            this.repo = repository;
        }
        
        /// <summary>
        /// Create a new node in a 1D model type
        /// </summary>
        /// <param name="coordinateAlongGlobalXAxis">The location of the node along the global x-axis</param>
        /// <returns>The newly created node</returns>
        public FiniteElementNode Create(double coordinateAlongGlobalXAxis)
        {
            if (this.modelType.GetDimensions() != GeometryDimensionality.OneDimension)
            {
                throw new InvalidOperationException("Can only create a Node with an x-coordinate when a 1D system is in use");
            }
            
            FiniteElementNode newNode = new FiniteElementNode(coordinateAlongGlobalXAxis);
            
            if (this.repo != null)
            {
                this.repo.Add(newNode);
            }
            
            return newNode;
        }
        
        /// <summary>
        /// Create a new node in a 2D model type
        /// </summary>
        /// <param name="coordinateAlongGlobalXAxis">The location of the node along the global x-axis</param>
        /// <param name="coordinateAlongGlobalYAxis">The location of the node along the global y-axis</param>
        /// <returns>The newly created node</returns>
        public FiniteElementNode Create(double coordinateAlongGlobalXAxis, double coordinateAlongGlobalYAxis)
        {
            if (this.modelType.GetDimensions() != GeometryDimensionality.TwoDimension)
            {
                throw new InvalidOperationException("Can only create a Node with an x and y coordinate when a 2D system is in use");
            }
            
            if (!this.modelType.IsAllowedDegreeOfFreedomForGeometry(DegreeOfFreedom.Y))
        	{
        		throw new InvalidOperationException(String.Format(
            		"Cannot define geometry in the Y-direction for model type of {0}.  Use the CreateForTruss method instead.",
            		this.modelType));
        	}
            
            FiniteElementNode newNode = new FiniteElementNode(coordinateAlongGlobalXAxis, coordinateAlongGlobalYAxis);
            if (this.repo != null)
            {
                this.repo.Add(newNode);
            }
            
            return newNode;
        }
        
        public FiniteElementNode CreateForTruss(double coordinateAlongGlobalXAxis, double coordinateAlongGlobalZAxis)
        {
        	if (this.modelType.GetDimensions() != GeometryDimensionality.TwoDimension)
        	{
        		throw new InvalidOperationException("Can only create a Node with an x and z coordinate when a 2D system is in use");
        	}
        	
        	if (!this.modelType.IsAllowedDegreeOfFreedomForGeometry(DegreeOfFreedom.Z))
        	{
        		throw new InvalidOperationException(String.Format(
            		"Cannot define geometry in the Z-direction for model type of {0}.  Use the Create method instead.",
            		this.modelType));
        	}
        	
        	FiniteElementNode newNode = new FiniteElementNode(coordinateAlongGlobalXAxis, 0, coordinateAlongGlobalZAxis);
            if (this.repo != null)
            {
                this.repo.Add(newNode);
            }
            
            return newNode;
        }
        
        /// <summary>
        /// Create a new node in a 2D model type
        /// </summary>
        /// <param name="coordinateAlongGlobalXAxis">The location of the node along the global x-axis</param>
        /// <param name="coordinateAlongGlobalYAxis">The location of the node along the global y-axis</param>
        /// <param name="coordinateAlongGlobalZAxis">The location of the node along the global z-axis</param>
        /// <returns>The newly created node</returns>
        public FiniteElementNode Create(double coordinateAlongGlobalXAxis, double coordinateAlongGlobalYAxis, double coordinateAlongGlobalZAxis)
        {
            if (this.modelType.GetDimensions() != GeometryDimensionality.ThreeDimension)
            {
                throw new InvalidOperationException("Can only create a Node with an x, y and z coordinate when a 3D system is in use");
            }
            
            FiniteElementNode newNode = new FiniteElementNode(coordinateAlongGlobalXAxis, coordinateAlongGlobalYAxis, coordinateAlongGlobalZAxis);
            if (this.repo != null)
            {
                this.repo.Add(newNode);
            }
            
            return newNode;
        }
    }
}
