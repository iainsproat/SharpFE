//-----------------------------------------------------------------------
// <copyright file="IStiffnessProvider.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Stiffness
{
    using System;
    
    public interface IStiffnessProvider
    {
        StiffnessMatrix GlobalStiffnessMatrix { get; } ////FIXME only here for testing, to be removed
 
        double GetGlobalStiffnessAt(FiniteElementNode rowNode, DegreeOfFreedom rowDegreeOfFreedom, FiniteElementNode columnNode, DegreeOfFreedom columnDegreeOfFreedom);
    }
}
