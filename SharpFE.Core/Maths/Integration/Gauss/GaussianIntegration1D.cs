namespace SharpFE.Maths.Integration.Gauss
{
    using System;
    
    public class GaussianIntegration1D
    {
        public static double[] GetIntegrationValues(int numIntegrationPoints)
        {
            switch(numIntegrationPoints)
            {
                case 1:
                    return new double[]
                    {
                        0
                    };
                case 2:
                    return new double[]
                    {
                        -1.0 / Math.Sqrt(3),
                        1 / Math.Sqrt(3)
                    };
                case 3:
                    return new double[]
                    {
                        -Math.Sqrt(3.0 / 5.0),
                        0,
                        Math.Sqrt(3.0 / 5.0)
                    };
                default:
                    throw new InvalidOperationException(string.Format("Cannot get weights for {0} number of integration points", numIntegrationPoints));
            }
        }
        
        public static double[] GetWeights(int numIntegrationPoints)
        {
            switch(numIntegrationPoints)
            {
                case 1:
                    return new double[]
                    {
                        2.0
                    };
                case 2:
                    return new double[]
                    {
                        1.0,
                        1.0
                    };
                case 3:
                    return new double[]
                    {
                        5.0 / 9.0,
                        8.0 / 9.0,
                        5.0 / 9.0
                    };
                default:
                    throw new InvalidOperationException(string.Format("Cannot get weights for {0} number of integration points", numIntegrationPoints));
            }
        }
        
        public GaussianIntegration1D(int numIntegrationPoints)
        {
            Guard.AgainstBadArgument("numIntegrationPoints",
                                     () => {
                                         return numIntegrationPoints < 0;
                                     },
                                     "Number of integration points must be a positive integer");
            
            this.NumberIntegrationPoints = numIntegrationPoints;
            this.Weights = GaussianIntegration1D.GetWeights(this.NumberIntegrationPoints);
            this.IntegrationValue = GaussianIntegration1D.GetIntegrationValues(this.NumberIntegrationPoints);
        }
        
        public int NumberIntegrationPoints
        {
            get;
            private set;
        }
        
        public double[] Weights
        {
            get;
            private set;
        }
        
        public double[] IntegrationValue
        {
            get;
            private set;
        }
        
        public double Solve(Func<double, double> integral)
        {
            double result = 0;
            for (int i = 0; i < this.NumberIntegrationPoints; i++)
            {
                result += this.Weights[i] * integral(this.IntegrationValue[i]);
            }
            
            return result;
        }
    }
}
