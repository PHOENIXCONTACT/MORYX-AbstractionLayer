namespace Marvin.Resources.UI.Interaction
{
    /// <summary>
    /// Interface representing the resource head information
    /// </summary>
    public interface IResourceHead
    {
        /// <summary>
        /// The Id of the resource
        /// </summary>
        long Id { get; }

        /// <summary>
        /// The name of the resource
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Description of the resource
        /// </summary>
        string Description { get; set; }
    }
}