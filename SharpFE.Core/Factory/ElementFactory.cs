//-----------------------------------------------------------------------
// <copyright file="ElementFactory.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
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
        internal ElementFactory()
            : this(null)
        {
            // empty
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementFactory" /> class.
        /// </summary>
        /// <param name="elementRepository">The repository into which to add the new elements which are created by this factory.</param>
        internal ElementFactory(ElementRepository elementRepository)
        {
            this.repository = elementRepository;
        }
        
        /// <summary>
        /// Creates a new <see cref="Spring" /> finite element.
        /// </summary>
        /// <param name="node1">The node at the start of the spring</param>
        /// <param name="node2">The node at the end of the spring</param>
        /// <param name="springConstant">The constant value of stiffness of the spring.</param>
        /// <returns>The newly created Spring element</returns>
        public LinearConstantSpring CreateLinearConstantSpring(FiniteElementNode node1, FiniteElementNode node2, double springConstant)
        {
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
        public LinearTruss CreateLinearTruss(FiniteElementNode node1, FiniteElementNode node2, IMaterial material, ICrossSection crossSection)
        {
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
        public Linear1DBeam CreateLinear1DBeam(FiniteElementNode start, FiniteElementNode end, IMaterial material, ICrossSection crossSection)
        {
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
        public Linear3DBeam CreateLinear3DBeam(FiniteElementNode start, FiniteElementNode end, IMaterial material, ICrossSection crossSection)
        {
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
        public LinearConstantStrainTriangle CreateLinearConstantStrainTriangle(FiniteElementNode node0, FiniteElementNode node1, FiniteElementNode node2, IMaterial material, double thickness)
        {
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
        public LinearConstantStressQuadrilateral CreateLinearConstantStressQuadrilateral(FiniteElementNode node0, FiniteElementNode node1, FiniteElementNode node2, FiniteElementNode node3, IMaterial material, double thickness)
        {
            LinearConstantStressQuadrilateral newQuad = new LinearConstantStressQuadrilateral(node0, node1, node2, node3, material, thickness);
            if (this.repository != null)
            {
                this.repository.Add(newQuad);
            }
            
            return newQuad;
        }
    }
}
