//-----------------------------------------------------------------------
// <copyright file="Linear3DBeam.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using SharpFE.Elements;
    using SharpFE.Stiffness;
    
    /// <summary>
    /// Beam which carries moment and shear force in both major and minor axes as well as axial force and torsion.
    /// </summary>
    public class Linear3DBeam : LinearBeam
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="mat"></param>
        /// <param name="section"></param>
        public Linear3DBeam(FiniteElementNode start, FiniteElementNode end, IMaterial mat, ICrossSection section)
            : base(start, end, mat, section)
        {
            // empty
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
                case DegreeOfFreedom.X:  // axial
                case DegreeOfFreedom.Y:  // minor-axis shear
                case DegreeOfFreedom.Z:  // major-axis shear
                case DegreeOfFreedom.XX: // torsion
                case DegreeOfFreedom.YY: // major-axis moment
                case DegreeOfFreedom.ZZ: // minor-axis moment
                    return true;
                default:
                    return false;
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
                case ModelType.Truss3D:
                    return false;
                case ModelType.MultiStorey2DSlab:
                    return true;
                case ModelType.Full3D:
                    return true;
                default:
                    throw new NotImplementedException(string.Format(
                        System.Globalization.CultureInfo.InvariantCulture,
                        "Linear3DBeam.IsSupportedModelType(ModelType) has not been defined for a model type of {0}",
                        modelType));
            }
        }
    }
}
