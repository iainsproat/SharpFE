//-----------------------------------------------------------------------
// <copyright file="LinearTruss.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;

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
        public LinearTruss(IFiniteElementNode start, IFiniteElementNode end, IMaterial material, ICrossSection crossSection)
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
        public override IList<DegreeOfFreedom> SupportedBoundaryConditionDegreeOfFreedom
        {
            get
            {
                return new List<DegreeOfFreedom>
                {
                    DegreeOfFreedom.X
                };
            }
        }
        
        public static bool IsASupportedModelType(ModelType modelType)
        {
            switch(modelType)
            {
                case ModelType.Truss1D:
                case ModelType.Beam1D:
                case ModelType.Truss2D:
                case ModelType.Frame2D:
                case ModelType.Slab2D:
                case ModelType.Membrane2D:
                case ModelType.Truss3D:
                case ModelType.Membrane3D:
                case ModelType.MultiStorey2DSlab:
                case ModelType.Full3D:
                    return true;
                default:
                    throw new NotImplementedException(string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "LinearTruss.IsSupportedModelType(ModelType) has not been defined for a model type of {0}",
                        modelType));
            }
        }
    }
}
