//-----------------------------------------------------------------------
// <copyright file="IStiffnessProvider.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    
    /// <summary>
    /// 
    /// </summary>
    public interface IStiffnessProvider
    {
        /// <summary>
        /// 
        /// </summary>
        StiffnessMatrix GlobalStiffnessMatrix { get; } ////FIXME only here for testing, to be removed
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowNode"></param>
        /// <param name="rowDegreeOfFreedom"></param>
        /// <param name="columnNode"></param>
        /// <param name="columnDegreeOfFreedom"></param>
        /// <returns></returns>
        double GetGlobalStiffnessAt(FiniteElementNode rowNode, DegreeOfFreedom rowDegreeOfFreedom, FiniteElementNode columnNode, DegreeOfFreedom columnDegreeOfFreedom);
    }
}
