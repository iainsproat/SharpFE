//-----------------------------------------------------------------------
// <copyright file="LinearBeam.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE.Elements
{
    using System;
    
    /// <summary>
    /// </summary>
    public abstract class LinearBeam : FiniteElement1D, IHasConstantCrossSection, IHasMaterial
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="mat"></param>
        /// <param name="section"></param>
        protected LinearBeam(FiniteElementNode start, FiniteElementNode end, IMaterial mat, ICrossSection section)
            : base(start, end)
        {
            Guard.AgainstNullArgument(mat, "mat");
            Guard.AgainstNullArgument(section, "section");
            
            this.Material = mat;
            this.CrossSection = section;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IMaterial Material
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public ICrossSection CrossSection
        {
            get;
            private set;
        }
    }
}
