using System.Threading.Tasks;
using Marvin.Resources.UI.ResourceService;
using Marvin.Serialization;
using Marvin.Tools.Wcf;

namespace Marvin.Resources.UI
{
    /// <summary>
    /// Model interface for resource interaction
    /// </summary>
    public interface IResourceServiceModel : IHttpServiceConnector
    {
        /// <summary>
        /// Full type tree of the currently installed resources
        /// </summary>
        ResourceTypeModel TypeTree { get; }

        /// <summary>
        /// Returns the type tree of all resources
        /// </summary>
        Task<ResourceTypeModel> GetTypeTree();

        /// <summary>
        /// Returns the resource tree
        /// </summary>
        Task<ResourceModel[]> GetResourceTree();

        /// <summary>
        /// Returns the resource tree
        /// </summary>
        Task<ResourceModel[]> GetResources(ResourceQuery query);

        /// <summary>
        /// Creates a new resource with the given plugin name
        /// </summary>
        Task<ResourceModel> CreateResource(string typeName, MethodEntry constructor);

        /// <summary>
        /// Saves the given resource with all changes
        /// </summary>
        Task<ResourceModel> SaveResource(ResourceModel resource);

        /// <summary>
        /// Removed the given resource
        /// </summary>
        Task<bool> RemoveResource(long resourceId);

        /// <summary>
        /// Gets the details of a resource with the resource id
        /// </summary>
        Task<ResourceModel[]> GetDetails(params long[] resourceIds);

        /// <summary>
        /// Invoke method on a resource object
        /// </summary>
        Task<Entry> InvokeMethod(long resourceId, MethodEntry method);
    }
}