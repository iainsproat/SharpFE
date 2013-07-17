//-----------------------------------------------------------------------
// <copyright file="ElementFactory.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012, 2013
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Creates elements and adds them to the repository if it exists.
    /// </summary>
    public class ElementFactory
    {
        /// <summary>
        /// The repository to add new elements to.
        /// </summary>
        private ElementRepository repository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementFactory" /> class.
        /// </summary>
        internal ElementFactory(ModelType typeOfModel)
            : this(typeOfModel, null)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementFactory" /> class.
        /// </summary>
        /// <param name="elementRepository">The repository into which to add the new elements which are created by this factory.</param>
        internal ElementFactory(ModelType typeOfModel, ElementRepository elementRepository)
        {
            this.ModelType = typeOfModel;
            this.repository = elementRepository;
        }
        
        public ModelType ModelType
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Creates a new <see cref="Spring" /> finite element.
        /// </summary>
        /// <param name="node1">The node at the start of the spring</param>
        /// <param name="node2">The node at the end of the spring</param>
        /// <param name="springConstant">The constant value of stiffness of the spring.</param>
        /// <returns>The newly created Spring element</returns>
        public LinearConstantSpring CreateLinearConstantSpring(IFiniteElementNode node1, IFiniteElementNode node2, double springConstant)
        {
            Guard.AgainstInvalidState(() => { return !LinearConstantSpring.IsASupportedModelType(this.ModelType); },
                                      "LinearConstantSpring are not available in a model of type {0}",
                                      this.ModelType);
            
            LinearConstantSpring newSpring = new LinearConstantSpring(node1, node2, springConstant);
            if (this.repository != null)
            {
                this.repository.Add(newSpring);
            }
            
            return newSpring;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="material"></param>
        /// <param name="crossSection"></param>
        /// <returns></returns>
        public LinearTruss CreateLinearTruss(IFiniteElementNode node1, IFiniteElementNode node2, IMaterial material, ICrossSection crossSection)
        {
            Guard.AgainstInvalidState(() => { return !LinearTruss.IsASupportedModelType(this.ModelType); },
                                      "LinearTruss are not available in a model of type {0}",
                                      this.ModelType);
            
            LinearTruss newTruss = new LinearTruss(node1, node2, material, crossSection);
            if (this.repository != null)
            {
                this.repository.Add(newTruss);
            }
            
            return newTruss;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="material"></param>
        /// <param name="crossSection"></param>
        /// <returns></returns>
        public Linear1DBeam CreateLinear1DBeam(IFiniteElementNode start, IFiniteElementNode end, IMaterial material, ICrossSection crossSection)
        {
            Guard.AgainstInvalidState(() => { return !Linear1DBeam.IsASupportedModelType(this.ModelType); },
                                      "Linear1DBeams are not available in a model of type {0}",
                                      this.ModelType);
            
            Linear1DBeam newBeam = new Linear1DBeam(start, end, material, crossSection);
            if (this.repository != null)
            {
                this.repository.Add(newBeam);
            }
            
            return newBeam;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="material"></param>
        /// <param name="crossSection"></param>
        /// <returns></returns>
        public Linear3DBeam CreateLinear3DBeam(IFiniteElementNode start, IFiniteElementNode end, IMaterial material, ICrossSection crossSection)
        {
            Guard.AgainstInvalidState(() => { return !Linear3DBeam.IsASupportedModelType(this.ModelType); },
                                      "Linear3DBeams are not available in a model of type {0}",
                                      this.ModelType);
            
            Linear3DBeam newBeam = new Linear3DBeam(start, end, material, crossSection);
            if (this.repository != null)
            {
                this.repository.Add(newBeam);
            }
            
            return newBeam;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node0"></param>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="material"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public LinearConstantStrainTriangle CreateLinearConstantStrainTriangle(IFiniteElementNode node0, IFiniteElementNode node1, IFiniteElementNode node2, IMaterial material, double thickness)
        {
            Guard.AgainstInvalidState(() => { return !LinearConstantStrainTriangle.IsASupportedModelType(this.ModelType); },
                                      "LinearConstantStrainTriangle are not available in a model of type {0}",
                                      this.ModelType);
            
            LinearConstantStrainTriangle newTriangle = new LinearConstantStrainTriangle(node0, node1, node2, material, thickness);
            if (this.repository != null)
            {
                this.repository.Add(newTriangle);
            }
            
            return newTriangle;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node0"></param>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="node3"></param>
        /// <param name="material"></param>
        /// <param name="thickness"></param>
        /// <returns></returns>
        public LinearConstantStressQuadrilateral CreateLinearConstantStressQuadrilateral(IFiniteElementNode node0, IFiniteElementNode node1, IFiniteElementNode node2, IFiniteElementNode node3, IMaterial material, double thickness)
        {
            Guard.AgainstInvalidState(() => { return !LinearConstantStressQuadrilateral.IsASupportedModelType(this.ModelType); },
                                      "LinearConstantStressQuadrilateral are not available in a model of type {0}",
                                      this.ModelType);
            //TODO error check for non-planar nodes.  i.e. one node out of plane of the other three
            //TODO error check for out of sequence nodes.  Nodes should be sequenced in a clockwise or anti-clockwise manner.
            LinearConstantStressQuadrilateral newQuad = new LinearConstantStressQuadrilateral(node0, node1, node2, node3, material, thickness);
            if (this.repository != null)
            {
                this.repository.Add(newQuad);
            }
            
            return newQuad;
        }
    }
}
