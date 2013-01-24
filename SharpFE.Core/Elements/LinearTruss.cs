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
        public LinearTruss(FiniteElementNode start, FiniteElementNode end, IMaterial material, ICrossSection crossSection)
            : base(start, end)
        {
            this.CrossSection = crossSection;
            this.Material = material;
        }
        
        public ICrossSection CrossSection
        {
            get;
            private set;
        }
        
        public IMaterial Material
        {
            get;
            private set;
        }
        
        #region Equals and GetHashCode implementation
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
        
        public static bool operator !=(LinearTruss leftHandSide, LinearTruss rightHandSide)
        {
            return !(leftHandSide == rightHandSide);
        }
        
        public override bool Equals(object obj)
        {
            LinearTruss other = obj as LinearTruss;
            if (other == null)
            {
                return false;
            }
            
            return base.Equals(other) && object.Equals(this.Material, other.Material) && object.Equals(this.CrossSection, other.CrossSection);
        }
        
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
