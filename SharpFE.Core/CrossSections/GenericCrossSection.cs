/*
 * Copyright Iain Sproat, 2013
 * User: ispro
 * Date: 28/06/2013
 * 
 */
using System;

namespace SharpFE
{
    /// <summary>
    /// Description of GenericCrossSection.
    /// </summary>
    public class GenericCrossSection : ICrossSection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public GenericCrossSection(double sectionArea)
        {
            Guard.AgainstBadArgument(() => {return sectionArea <= 0;},
                                     "sectionArea has to be a positive, non-zero number", 
                                     "sectionArea");
            
            this.Area = sectionArea;
        }
        
        public GenericCrossSection(double sectionArea, double sectionSecondMomentOfAreaAroundYY)
        {
            Guard.AgainstBadArgument(() => {return sectionArea <= 0;},
                                     "sectionArea has to be a positive, non-zero number", 
                                     "sectionArea");
            Guard.AgainstBadArgument(() => {return sectionSecondMomentOfAreaAroundYY <= 0;},
                                     "sectionSecondMomentOfAreaAroundYY has to be a positive, non-zero number", 
                                     "sectionSecondMomentOfAreaAroundYY");
            
            this.Area = sectionArea;
            this.SecondMomentOfAreaAroundYY = sectionSecondMomentOfAreaAroundYY;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double MaximumDepth
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double MaximumWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double Area
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double SecondMomentOfAreaAroundYY
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double SecondMomentOfAreaAroundZZ
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double ExternalPerimeterLength
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double MomentOfInertiaInTorsion
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Coordinate2D GeometricCentroid
        {
            get
            {
                return new Coordinate2D(0, 0);
            }
        }
    }
}
