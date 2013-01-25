//-----------------------------------------------------------------------
// <copyright file="GenericElasticMaterial.cs" company="Iain Sproat">
//     Copyright Iain Sproat, 2013.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    
    /// <summary>
    /// </summary>
    public class GenericElasticMaterial : ILinearElasticMaterial
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericElasticMaterial">GenericElasticMaterial</see> class.
        /// </summary>
        /// <param name="rho">Density of the material</param>
        /// <param name="youngsMod">Young's modulus of the elastic material</param>
        /// <param name="nu">Poisson's ratio of the elastic material</param>
        /// <param name="shearModElasticity">Shear modulus of elasticity of the elastic material</param>
        public GenericElasticMaterial(double rho, double youngsMod, double nu, double shearModElasticity)
        {
            this.Density = rho;
            this.YoungsModulus = youngsMod;
            this.PoissonsRatio = nu;
            this.ShearModulusElasticity = shearModElasticity;
        }
        
        /// <summary>
        /// Gets the density of the material
        /// </summary>
        public double Density
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the youngs modulus of the material
        /// </summary>
        public double YoungsModulus
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the Poisson's ratio of the material
        /// </summary>
        public double PoissonsRatio
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets the shear modulus of elasticity of the material
        /// </summary>
        public double ShearModulusElasticity
        {
            get;
            private set;
        }
    }
}
