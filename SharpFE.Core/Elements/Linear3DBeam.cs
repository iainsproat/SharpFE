//-----------------------------------------------------------------------
// <copyright file="Linear3DBeam.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;
    using SharpFE.Elements;
    
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
        public Linear3DBeam(IFiniteElementNode start, IFiniteElementNode end, IMaterial mat, ICrossSection section)
            : base(start, end, mat, section)
        {
            // empty
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="degreeOfFreedom"></param>
        /// <returns></returns>
        public override IList<DegreeOfFreedom> SupportedLocalBoundaryConditionDegreeOfFreedom
        {
            get
            {
                return new List<DegreeOfFreedom>
                {
                    DegreeOfFreedom.X,  // axial
                    DegreeOfFreedom.Y,  // minor-axis shear
                    DegreeOfFreedom.Z,  // major-axis shear
                    DegreeOfFreedom.XX, // torsion
                    DegreeOfFreedom.YY, // major-axis moment
                    DegreeOfFreedom.ZZ // minor-axis moment
                };
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
                case ModelType.Membrane3D:
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
