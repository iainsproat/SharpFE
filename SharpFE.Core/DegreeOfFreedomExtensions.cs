namespace SharpFE
{
    using System;
    
    public static class DegreeOfFreedomExtensions
    {
        public static bool IsLinear(this DegreeOfFreedom dof)
        {
            return (int)dof < 3;
        }
        
        public static bool IsRotational(this DegreeOfFreedom dof)
        {
            return !dof.IsLinear();
        }
    }
}
