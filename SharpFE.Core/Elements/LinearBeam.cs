//-----------------------------------------------------------------------
// <copyright file="?.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace SharpFE.Elements
{
    /// <summary>
    /// Description of Beam.
    /// </summary>
    public abstract class LinearBeam : FiniteElement1D, IHasConstantCrossSection, IHasMaterial
    {
        protected LinearBeam(FiniteElementNode start, FiniteElementNode end, IMaterial mat, ICrossSection section)
            : base(start, end)
        {
            Guard.AgainstNullArgument(mat, "mat");
            Guard.AgainstNullArgument(section, "section");
            
            this.Material = mat;
            this.CrossSection = section;
        }
        
        public IMaterial Material
        {
            get;
            private set;
        }
        
        public ICrossSection CrossSection
        {
            get;
            private set;
        }
    }
}
