/*
 * Copyright Iain Sproat, 2013
 * User: ispro
 * Date: 04/07/2013
 * 
 */
using System;
using System.Collections.Generic;

namespace SharpFE.Materials
{
    public class MaterialMatrixBuilder
    {
        
        public MaterialMatrixBuilder(IMaterial mat)
        {
            this.Material = mat;
        }
        
        public IMaterial Material
        {
            get;
            private set;
        }
        
        public KeyedSquareMatrix<Strain> PlaneStressMatrix(IList<Strain> supportedStrains)
        {
            double poissons = this.Material.PoissonsRatio;
            double youngs = this.Material.YoungsModulus;
            double lameConstantLambda = poissons * youngs / (1 - (poissons * poissons));
            double lameConstantMu = youngs / (2 * (1 + poissons));
            return this.CreateMaterialMatrix(supportedStrains, lameConstantLambda, lameConstantMu);
            
        }
        
        public KeyedSquareMatrix<Strain> PlaneStrainMatrix(IList<Strain> supportedStrains)
        {
            double poissons = this.Material.PoissonsRatio;
            double youngs = this.Material.YoungsModulus;
            double lameConstantLambda = poissons * youngs / ((1 + poissons) * (1 - 2 * poissons));
            double lameConstantMu = youngs / (2 * (1 + poissons));
            return this.CreateMaterialMatrix(supportedStrains, lameConstantLambda, lameConstantMu);
            
        }
        
        protected KeyedSquareMatrix<Strain> CreateMaterialMatrix(IList<Strain> supportedStrains, double lameConstantLambda, double lameConstantMu)
        {
            KeyedSquareMatrix<Strain> matrix = CreateMaterialMatrix(lameConstantLambda, lameConstantMu);
            return matrix.SubMatrix(supportedStrains);
        }
        
        protected KeyedSquareMatrix<Strain> CreateMaterialMatrix(double lameConstantLambda, double lameConstantMu)
        {
            IList<Strain> keys = new List<Strain>
            {
                Strain.LinearStrainX,
                Strain.LinearStrainY,
                Strain.LinearStrainZ,
                Strain.ShearStrainXY,
                Strain.ShearStrainYZ,
                Strain.ShearStrainXZ
            };
            KeyedSquareMatrix<Strain> matrix = new KeyedSquareMatrix<Strain>(keys);

            matrix.At(Strain.LinearStrainX, Strain.LinearStrainX, lameConstantLambda + 2 * lameConstantMu);
            matrix.At(Strain.LinearStrainY, Strain.LinearStrainY, lameConstantLambda + 2 * lameConstantMu);
            matrix.At(Strain.LinearStrainZ, Strain.LinearStrainZ, lameConstantLambda + 2 * lameConstantMu);
            
            matrix.At(Strain.LinearStrainX, Strain.LinearStrainY, lameConstantLambda);
            matrix.At(Strain.LinearStrainX, Strain.LinearStrainZ, lameConstantLambda);
            matrix.At(Strain.LinearStrainY, Strain.LinearStrainX, lameConstantLambda);
            matrix.At(Strain.LinearStrainY, Strain.LinearStrainZ, lameConstantLambda);
            matrix.At(Strain.LinearStrainZ, Strain.LinearStrainX, lameConstantLambda);
            matrix.At(Strain.LinearStrainZ, Strain.LinearStrainY, lameConstantLambda);
            
            matrix.At(Strain.ShearStrainXY, Strain.ShearStrainXY, lameConstantMu);
            matrix.At(Strain.ShearStrainYZ, Strain.ShearStrainYZ, lameConstantMu);
            matrix.At(Strain.ShearStrainXZ, Strain.ShearStrainXZ, lameConstantMu);
            
            return matrix;
        }
    }
}
