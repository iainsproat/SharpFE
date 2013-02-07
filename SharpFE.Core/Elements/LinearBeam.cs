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
    public abstract class LinearBeam : FiniteElement1D, IHasConstantCrossSection, IHasMaterial, IEquatable<LinearBeam>
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
        
        #region Equals and GetHashCode implementation
        public override bool Equals(object obj)
        {
            return this.Equals(obj as LinearBeam);
        }
        
        public bool Equals(LinearBeam other)
        {
            if (other == null)
            {
                return false;
            }
            
            return base.Equals(other) && object.Equals(this.Material, other.Material) && object.Equals(this.CrossSection, other.CrossSection);
        }
        
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked {
                hashCode += 1000000002 * base.GetHashCode();
                if (Material != null)
                    hashCode += 1000000007 * Material.GetHashCode();
                if (CrossSection != null)
                    hashCode += 1000000009 * CrossSection.GetHashCode();
            }
            return hashCode;
        }

        
        public static bool operator ==(LinearBeam lhs, LinearBeam rhs)
        {
            if (ReferenceEquals(lhs, rhs))
                return true;
            if (ReferenceEquals(lhs, null) || ReferenceEquals(rhs, null))
                return false;
            return lhs.Equals(rhs);
        }
        
        public static bool operator !=(LinearBeam lhs, LinearBeam rhs)
        {
            return !(lhs == rhs);
        }
        #endregion

    }
}
