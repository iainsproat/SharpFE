//-----------------------------------------------------------------------
// <copyright file="Linear1DBeam.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using SharpFE.Elements;
    using SharpFE.Stiffness;

    /// <summary>
    /// This is a linear 1D finite element.
    /// It only calculates major axis bending moment and major-axis shear force.
    /// </summary>
    public class Linear1DBeam : LinearBeam
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="mat"></param>
        /// <param name="section"></param>
        public Linear1DBeam(IFiniteElementNode start, IFiniteElementNode end, IMaterial mat, ICrossSection section)
            : base(start, end, mat, section)
        {
            // empty
        }
        
        /// <summary>
        /// Elastic stiffness per metre length.
        /// </summary>
        public double StiffnessEA
        {
            get
            {
                return this.Material.YoungsModulus * this.CrossSection.Area;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Az", Justification = "common engineering term")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Az", Justification = "common engineering term")]
        public double ShearStiffnessGAz
        {
            get
            {
                return this.Material.ShearModulusElasticity * this.CrossSection.Area;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Iy", Justification = "common engineering term")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Iy", Justification = "common engineering term")]
        public double BendingStiffnessEIy
        {
            get
            {
                return this.Material.YoungsModulus * this.CrossSection.SecondMomentOfAreaAroundYY;
            }
        }
        
        public static bool IsASupportedModelType(ModelType modelType)
        {
            switch(modelType)
            {
               case ModelType.Truss1D:
                    return false;
                case ModelType.Beam1D:
                    return true;
                case ModelType.Truss2D:
                    return false;
                case ModelType.Frame2D:
                    return true;
                case ModelType.Slab2D:
                    return true;
                case ModelType.Membrane2D:
                    return false;
                case ModelType.Truss3D:
                    return false;
                case ModelType.MultiStorey2DSlab:
                    return true;
                case ModelType.Full3D:
                    return false;
                default:
                    throw new NotImplementedException(string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "Linear1DBeam.IsSupportedModelType(ModelType) has not been defined for a model type of {0}",
                        modelType));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="degreeOfFreedom"></param>
        /// <returns></returns>
        public override bool IsASupportedBoundaryConditionDegreeOfFreedom(DegreeOfFreedom degreeOfFreedom)
        {
            switch (degreeOfFreedom)
            {
                case DegreeOfFreedom.Z: // major-axis shear
                case DegreeOfFreedom.YY: // major-axis moment
                    return true;
                case DegreeOfFreedom.X:  // axial force
                case DegreeOfFreedom.Y:  // minor-axis shear
                case DegreeOfFreedom.XX: // torsion
                case DegreeOfFreedom.ZZ: // minor-axis
                default:
                    return false;
            }
        }
    }
}
