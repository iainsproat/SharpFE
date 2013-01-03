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
        public ConstantLinearSpring CreateConstantLinearSpring(FiniteElementNode node1, FiniteElementNode node2, double springConstant)
        {
            ConstantLinearSpring newSpring = new ConstantLinearSpring(node1, node2, springConstant);
            if (this.repository != null)
            {
                this.repository.Add(newSpring);
            }
            
            return newSpring;
        }
    }
}
