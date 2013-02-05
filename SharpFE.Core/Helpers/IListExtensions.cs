//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace SharpFE
{
    /// <summary>
    /// Extends IList.
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// Determines whether a list is empty. i.e. it has a Count of zero.
        /// </summary>
        /// <param name="list">The list to check as to whether it is empty</param>
        /// <returns>True if the list is empty, otherwise false.</returns>
        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list.Count == 0;
        }
    }
}
