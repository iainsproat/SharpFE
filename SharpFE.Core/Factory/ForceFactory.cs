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
            Guard.AgainstInvalidState(() => { return this.modelType != ModelType.Truss1D; },
                                      "Can only use the Create(double valueOfXComponent) method to create a force along the x-axis when a 1D system is in use");
            
            ForceVector newForce = new ForceVector(valueOfXComponent);
            if (this.repository != null)
            {
                this.repository.Add(newForce);
            }
            
            return newForce;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueOfZComponent"></param>
        /// <param name="valueOfMomentAboutYY"></param>
        /// <returns></returns>
        public ForceVector CreateFor1DBeam(double valueOfZComponent, double valueOfMomentAboutYY)
        {
            Guard.AgainstInvalidState(() => { return !(this.modelType == ModelType.Beam1D || this.modelType == ModelType.Frame2D); },
                                      "Can only use the CreateFor1DBeam(double valueOfZComponent, double valueOfMomentAboutYY) method when a 1D Beam system is in use");
            
            ForceVector newForce = new ForceVector(0, 0, valueOfZComponent, 0, valueOfMomentAboutYY, 0);
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
            Guard.AgainstInvalidState(() => { return !this.modelType.IsAllowedDegreeOfFreedomForBoundaryConditions(DegreeOfFreedom.X); },
                                      "Cannot create a boundary condition along the x-axis for this model type.");
            Guard.AgainstInvalidState(() => { return !this.modelType.IsAllowedDegreeOfFreedomForBoundaryConditions(DegreeOfFreedom.Y); },
                                      "Cannot create a boundary condition along the y-axis for this model type.");
            
            ForceVector newForce = new ForceVector(valueOfXComponent, valueOfYComponent);
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
        /// <param name="valueOfZComponent">The component of the force along the global z-axis</param>
        /// <returns>The force vector which has been created</returns>
        public ForceVector CreateForTruss(double valueOfXComponent, double valueOfZComponent)
        {
            Guard.AgainstInvalidState(() => { return !this.modelType.IsAllowedDegreeOfFreedomForBoundaryConditions(DegreeOfFreedom.X); },
                                      "Cannot create a boundary condition along the x-axis for this model type.");
            
            Guard.AgainstInvalidState(() => { return !this.modelType.IsAllowedDegreeOfFreedomForBoundaryConditions(DegreeOfFreedom.Z); },
                                      "Cannot create a boundary condition along the z-axis for this model type.");
            
            ForceVector newForce = new ForceVector(valueOfXComponent, 0,  valueOfZComponent);
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
        /// <param name="valueOfZComponent">The component of the force along the global z-axis</param>
        /// <returns>The force vector which has been created</returns>
        public ForceVector Create(double valueOfXComponent, double valueOfYComponent, double valueOfZComponent, double valueOfXXComponent, double valueOfYYComponent, double valueOfZZComponent)
        {
            ////TODO
            
            ForceVector newForce = new ForceVector(valueOfXComponent, valueOfYComponent,  valueOfZComponent, valueOfXXComponent, valueOfYYComponent, valueOfZZComponent);
            if (this.repository != null)
            {
                this.repository.Add(newForce);
            }
            
            return newForce;
        }
    }
}
