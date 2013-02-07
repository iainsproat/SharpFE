//-----------------------------------------------------------------------
// <copyright file="LinearTruss.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;

    using SharpFE.Elements;
    using SharpFE.Stiffness;

    /// <summary>
    /// Linear element which has constant cross section
    /// and constant material properties.
    /// Also known as a Rod element.
    /// </summary>
    public class LinearTruss : FiniteElement1D, IHasMaterial, IHasConstantCrossSection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="material"></param>
        /// <param name="crossSection"></param>
        public LinearTruss(FiniteElementNode start, FiniteElementNode end, IMaterial material, ICrossSection crossSection)
            : base(start, end)
        {
            this.CrossSection = crossSection;
            this.Material = material;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public ICrossSection CrossSection
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IMaterial Material
        {
            get;
            private set;
        }
        
        #region Equals and GetHashCode implementation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftHandSide"></param>
        /// <param name="rightHandSide"></param>
        /// <returns></returns>
        public static bool operator ==(LinearTruss leftHandSide, LinearTruss rightHandSide)
        {
            if (object.ReferenceEquals(leftHandSide, rightHandSide))
            {
                return true;
            }
            
            if (object.ReferenceEquals(leftHandSide, null) || object.ReferenceEquals(rightHandSide, null))
            {
                return false;
            }
            
            return leftHandSide.Equals(rightHandSide);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftHandSide"></param>
        /// <param name="rightHandSide"></param>
        /// <returns></returns>
        public static bool operator !=(LinearTruss leftHandSide, LinearTruss rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as LinearTruss);
            }
        
        public bool Equals(LinearTruss other)
        {
            if (other == null)
            {
                return false;
            }
            
            return base.Equals(other) && object.Equals(this.Material, other.Material) && object.Equals(this.CrossSection, other.CrossSection);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            unchecked
            {
                hashCode += base.GetHashCode();
                hashCode += 1000000007 * this.Material.GetHashCode();
                hashCode += 1000000022 * this.CrossSection.GetHashCode();
            }
            
            return hashCode;
        }
        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="degreeOfFreedom"></param>
        /// <returns></returns>
        public override bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom)
        {
            switch (degreeOfFreedom)
            {
                case DegreeOfFreedom.X:
                    return true;
                case DegreeOfFreedom.Y:
                case DegreeOfFreedom.Z:
                case DegreeOfFreedom.XX:
                case DegreeOfFreedom.YY:
                case DegreeOfFreedom.ZZ:
                default:
                    return false;
            }
        }
    }
}
