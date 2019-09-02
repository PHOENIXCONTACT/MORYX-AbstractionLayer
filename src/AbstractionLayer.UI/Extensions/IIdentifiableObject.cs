namespace Marvin.AbstractionLayer.UI
{
    /// <summary>
    /// Object with an id
    /// </summary>
    public interface IIdentifiableObject
    {
        /// <summary>
        /// Identifier of the object
        /// </summary>
        long Id { get; }
    }
}