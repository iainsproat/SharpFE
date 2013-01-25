//-----------------------------------------------------------------------
// <copyright file="IHasCrossSection.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Justification = "Used to differentiate between elements which have cross sections and those which don't, without using attribute reflection")]
    public interface IHasCrossSection
    {
    }
}
