using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Marvin.AbstractionLayer.UI
{
    /// <summary>
    /// Extensions for collections used within the abstraction layer ui
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Merge a recursive model structure into an existing observable view model structure
        /// </summary>
        public static void MergeTree<TModel>(
            this Collection<TreeItemViewModel> current,
            IReadOnlyList<TModel> updated,
            IMergeStrategy<TModel, TreeItemViewModel> strategy)
            where TModel : class, ITreeItemModel<TModel>
        {
            // Use 'for'-loop, because it is modification tolerant
            int currentIndex = 0, updatedIndex = 0;
            while (currentIndex < current.Count || updatedIndex < updated.Count)
            {
                var currentItem = GetItem(current, currentIndex);
                var updatedItem = GetItem(updated, updatedIndex);
                // Default: Update the model if it matches
                if (currentItem != null && currentItem.Id == updatedItem?.Id)
                {
                    strategy.UpdateModel(currentItem, updatedItem);
                    currentIndex++;
                    updatedIndex++;
                    MergeTree(currentItem.Children, updatedItem.Children, strategy);
                }
                // Item was appended or inserted
                else if (currentItem == null || HasId(updated, currentItem.Id, updatedIndex))
                {
                    currentItem = strategy.FromModel(updatedItem);
                    current.Insert(currentIndex, currentItem);
                    currentIndex++;
                    updatedIndex++;
                    MergeTree(currentItem.Children, updatedItem.Children, strategy);
                }
                // In any other case: Remove
                else
                {
                    current.RemoveAt(currentIndex);
                    updatedIndex++;
                }
            }
        }

        private static TItem GetItem<TItem>(IReadOnlyList<TItem> source, int index)
            where TItem : class
        {
            return index < source.Count ? source[index] : null;
        }

        private static bool HasId<TItem>(IReadOnlyList<TItem> source, long id, int index)
            where TItem : ITreeItemModel<TItem>
        {
            for (; index < source.Count; index++)
            {
                if (source[index].Id == id)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Merges a collection of view models with a list of models
        /// </summary>
        public static void MergeCollection<TViewModel, TModel>(
            this ICollection<TViewModel> collection,
            IReadOnlyList<TModel> updated,
            IMergeStrategy<TModel, TViewModel> strategy)
            where TViewModel : IIdentifiableObject
            where TModel : IIdentifiableObject
        {
            // Remove those without id or not existing in the updated collection
            var removed = collection.Where(r => r.Id == 0 || updated.All(u => u.Id != r.Id)).ToList();
            foreach (var obj in removed)
                collection.Remove(obj);

            foreach (var updatedModel in updated)
            {
                var match = collection.FirstOrDefault(r => r.Id == updatedModel.Id);
                if (match != null)
                {
                    strategy.UpdateModel(match, updatedModel);
                }
                else
                {
                    var vm = strategy.FromModel(updatedModel);
                    collection.Add(vm);
                }
            }
        }
    }
}