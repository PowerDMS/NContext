namespace NContext.Common
{
    using System;

    /// <summary>
    /// Allows end users to implement their own security tokens.
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// Gets the tokens value. Typically a unique identifier of type <see cref="Guid"/> 
        /// or a calculated result from <see cref="Object.GetHashCode"/>.
        /// </summary>
        /// <remarks></remarks>
        String Value { get; }
    }
}