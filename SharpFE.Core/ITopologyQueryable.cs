//-----------------------------------------------------------------------
// <copyright file="ITopologyQueryable.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// Provides methods for the searching and querying
    /// the topology of the model
    /// </summary>
    public interface ITopologyQueryable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        IList<FiniteElement> AllElementsConnectedTo(FiniteElementNode node);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        IList<FiniteElement> AllElementsDirectlyConnecting(FiniteElementNode node1, FiniteElementNode node2);        
    }
}
