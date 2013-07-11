namespace SharpFE
{
    using System;
    
    public static class DegreeOfFreedomExtensions
    {
        public static bool IsLinear(this DegreeOfFreedom dof)
        {
            switch (dof)
            {
                case DegreeOfFreedom.X:
                case DegreeOfFreedom.Y:
                case DegreeOfFreedom.Z:
                    return true;
                case DegreeOfFreedom.XX:
                case DegreeOfFreedom.YY:
                case DegreeOfFreedom.ZZ:
                    return false;
                default:
                    throw new NotImplementedException(
                        string.Format(
                            "DegreeOfFreedomExtensions.IsLinear(DegreeOfFreedom) is not implemented for a DegreeOfFreedom for {0}",
                            dof));
            }
        }
        
        public static bool IsRotational(this DegreeOfFreedom dof)
        {
            return !dof.IsLinear();
        }
    }
}
