namespace SharpFE
{
    using System;
    
    /// <summary>
    /// Description of IHasConstantCrossSection.
    /// </summary>
    public interface IHasConstantCrossSection : IHasCrossSection
    {
        ICrossSection CrossSection { get; }
    }
}
