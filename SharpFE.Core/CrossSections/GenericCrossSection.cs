/*
 * Copyright Iain Sproat, 2013
 * User: ispro
 * Date: 28/06/2013
 * 
 */

namespace SharpFE
{
    using System;
    
    public class GenericCrossSection : ICrossSection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public GenericCrossSection(double sectionArea)
            : this(sectionArea, 0, 0, true)
        {
            Guard.AgainstBadArgument(() => {return sectionArea <= 0;},
                                     "sectionArea has to be a positive, non-zero number", 
                                     "sectionArea");
        }
        
        public GenericCrossSection(double sectionArea, double sectionSecondMomentOfAreaAroundYY)
            : this(sectionArea, sectionSecondMomentOfAreaAroundYY, 0, true)
        {
            Guard.AgainstBadArgument(() => {return sectionArea <= 0;},
                                     "sectionArea has to be a positive, non-zero number", 
                                     "sectionArea");
            Guard.AgainstBadArgument(() => {return sectionSecondMomentOfAreaAroundYY <= 0;},
                                     "sectionSecondMomentOfAreaAroundYY has to be a positive, non-zero number", 
                                     "sectionSecondMomentOfAreaAroundYY");
            
        }
        
        public GenericCrossSection(double sectionArea, double sectionSecondMomentOfAreaAroundYY, double sectionSecondMomentOfAreaAroundZZ)
            :this(sectionArea, sectionSecondMomentOfAreaAroundYY, sectionSecondMomentOfAreaAroundZZ, true)
        {
            Guard.AgainstBadArgument(() => {return sectionArea <= 0;},
                                     "sectionArea has to be a positive, non-zero number", 
                                     "sectionArea");
            Guard.AgainstBadArgument(() => {return sectionSecondMomentOfAreaAroundYY <= 0;},
                                     "sectionSecondMomentOfAreaAroundYY has to be a positive, non-zero number", 
                                     "sectionSecondMomentOfAreaAroundYY");
            Guard.AgainstBadArgument(() => {return sectionSecondMomentOfAreaAroundZZ <= 0;},
                                     "sectionSecondMomentOfAreaAroundZZ has to be a positive, non-zero number", 
                                     "sectionSecondMomentOfAreaAroundZZ");
        }
        
        /// <summary>
        /// All public constructors should delegate to this constructor.
        /// </summary>
        /// <param name="sectionArea"></param>
        /// <param name="sectionSecondMomentOfAreaAroundYY"></param>
        /// <param name="sectionSecondMomentOfAreaAroundZZ"></param>
        /// <param name="flag">Not used. The flag is only here to make this constructor signature unique and prevent compilation errors.</param>
        protected GenericCrossSection (double sectionArea, double sectionSecondMomentOfAreaAroundYY, double sectionSecondMomentOfAreaAroundZZ, bool flag)
        {
            this.Area = sectionArea;
            this.SecondMomentOfAreaAroundYY = sectionSecondMomentOfAreaAroundYY;
            this.SecondMomentOfAreaAroundZZ = sectionSecondMomentOfAreaAroundZZ;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double MaximumDepth
        {
            get
            {
                throw new NotImplementedException("GenericCrossSection.MaximumDepth");
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double MaximumWidth
        {
            get
            {
                throw new NotImplementedException("GenericCrossSection.MaximumWidth");
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
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double ExternalPerimeterLength
        {
            get
            {
                throw new NotImplementedException("GenericCrossSection.ExternalPerimeterLength");
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public double MomentOfInertiaInTorsion
        {
            get
            {
                throw new NotImplementedException("GenericCrossSection.MomentOfInertiaInTorsion");
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
