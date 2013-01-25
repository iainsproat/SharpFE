//-----------------------------------------------------------------------
// <copyright file="IModelConstraintProvider.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Provides information about degrees of freedom constraints in the model
    /// </summary>
    public interface IModelConstraintProvider
    {
        /// <summary>
        /// 
        /// </summary>
        IList<NodalDegreeOfFreedom> AllDegreesOfFreedom { get; }
        
        /// <summary>
        /// 
        /// </summary>
        IList<NodalDegreeOfFreedom> ConstrainedNodalDegreeOfFreedoms { get; }
        
        /// <summary>
        /// 
        /// </summary>
        IList<NodalDegreeOfFreedom> UnconstrainedNodalDegreeOfFreedoms { get; }
    }
}
