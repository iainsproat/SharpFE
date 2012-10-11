//-----------------------------------------------------------------------
// <copyright file="ForceFactory.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Factory which creates new forces.
    /// The forces can also be registered in a repository.
    /// </summary>
    public class ForceFactory
    {
        /// <summary>
        /// The <see cref="ModelType" /> of the model for which this is a factory for.
        /// </summary>
        private ModelType modelType;
        
        /// <summary>
        /// The repository into which to register new forces if the repository exists.
        /// </summary>
        private ForceRepository repository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ForceFactory" /> class.
        /// </summary>
        /// <param name="typeOfModel">The type of model for which this new instance will create forces.</param>
        internal ForceFactory(ModelType typeOfModel)
            : this(typeOfModel, null)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ForceFactory" /> class.
        /// </summary>
        /// <param name="typeOfModel">The type of model for which this new instance will create forces.</param>
        /// <param name="forceRepository">The repository into which to register newly created forces.</param>
        internal ForceFactory(ModelType typeOfModel, ForceRepository forceRepository)
        {
            this.modelType = typeOfModel;
            this.repository = forceRepository;
        }
        
        /// <summary>
        /// Creates a new force for a 1D ModelType
        /// </summary>
        /// <param name="valueOfXComponent">The component of the force along the global x-axis</param>
        /// <returns>The force vector which has been created</returns>
        public ForceVector Create(double valueOfXComponent)
        {
            if (this.modelType != ModelType.Truss1D && this.modelType != ModelType.Beam1D)
            {
                throw new InvalidOperationException("Can only create a Force with an x-value when a 1D system is in use");
            }
            
            ForceVector newForce = new ForceVector(valueOfXComponent);
            if (this.repository != null)
            {
                this.repository.Add(newForce);
            }
            
            return newForce;
        }
        
        /// <summary>
        /// Creates a new force for a 2D ModelType
        /// </summary>
        /// <param name="valueOfXComponent">The component of the force along the global x-axis</param>
        /// <param name="valueOfYComponent">The component of the force along the global y-axis</param>
        /// <returns>The force vector which has been created</returns>
        public ForceVector Create(double valueOfXComponent, double valueOfYComponent)
        {
            if (this.modelType == ModelType.Truss1D)
            {
                throw new InvalidOperationException("Cannot create a Node with an x and y value in a 1D model");
            }
            
            if (this.modelType == ModelType.Slab2D)
            {
                throw new InvalidOperationException("Cannot create a Force with an x and y translation values in a 2D slab model");
            }
            
            ForceVector newForce = new ForceVector(valueOfXComponent, valueOfYComponent);
            if (this.repository != null)
            {
                this.repository.Add(newForce);
            }
            
            return newForce;
        }
    }
}
