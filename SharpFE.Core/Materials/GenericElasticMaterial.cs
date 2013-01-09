namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Description of GenericElasticMaterial.
    /// </summary>
    public class GenericElasticMaterial : ILinearElasticMaterial
    {
        public GenericElasticMaterial(double rho, double E, double nu, double G)
        {
            this.Density = rho;
            this.YoungsModulus = E;
            this.PoissonsRatio = nu;
            this.ShearModulusElasticity = G;
        }
        
        public double Density
        {
            get;
            private set;
        }
        
        public double YoungsModulus
        {
            get;
            private set;
        }
        
        public double PoissonsRatio
        {
            get;
            private set;
        }
        
        public double ShearModulusElasticity
        {
            get;
            private set;
        }
    }
}
