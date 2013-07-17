namespace SharpFE.Maths.Integration.Gauss
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Generic;

    public class GaussianIntegration2D
    {
        public GaussianIntegration2D(int numIntegrationPoints)
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
        
        public double Integrate(Func<double, double, double> integrand)
        {
            double result = 0;
            int numIterations = this.DetermineNumberOfIterations();
            
            for (int i = 0; i < numIterations; i++)
            {
                for (int j = 0; j < numIterations; j++)
                {
                    result += this.Weights[i] * this.Weights[j] * integrand(this.IntegrationValue[i], this.IntegrationValue[j]);
                }
            }
            
            return result;
        }
        
        public Matrix<double> Integrate(Func<double, double, Matrix<double>> integrand)
        {
            Matrix<double> result = null;
            int numIterations = this.DetermineNumberOfIterations();
            
            for (int i = 0; i < numIterations; i++)
            {
                for (int j = 0; j < numIterations; j++)
                {
                    Matrix<double> temp = integrand(this.IntegrationValue[i], this.IntegrationValue[j]);
                    temp = temp.Multiply(this.Weights[i] * this.Weights[j]);
                    if (result == null)
                    {
                        result = temp;
                    }
                    else
                    {
                        result = result.Add(temp);
                    }
                }
            }
            
            return result;
        }
        
        protected int DetermineNumberOfIterations()
        {
            int numIterations = this.NumberIntegrationPoints;
            if (numIterations == 0)
            {
                numIterations = 1;
            }
            
            return numIterations;
        }
    }
}
