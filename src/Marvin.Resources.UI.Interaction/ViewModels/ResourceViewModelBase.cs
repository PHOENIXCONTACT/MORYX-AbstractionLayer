using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Marvin.Resources.UI.Interaction.ResourceInteraction;
using Marvin.Serialization;

namespace Marvin.Resources.UI.Interaction
{
    /// <summary>
    /// Base class for resource view models
    /// Will hold the base properties
    /// </summary>
    public class ResourceViewModelBase : ValidationViewModelBase, IEditableObject, IResourceHead
    {
        private readonly ResourceModel _model;
        private string _name;
        private string _globalIdentifier;
        private string _localIdentifier;
        private string _description;
        private Entry[] _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceViewModelBase"/> class.
        /// </summary>
        internal ResourceViewModelBase(ResourceModel model)
        {
            _model = model;
            Properties = new Entry[0];

            CopyFromModel();
        }

        /// <summary>
        /// Id of the resource
        /// </summary>
        public long Id => _model.Id;

        /// <summary>
        /// Name of the resource
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "The name can not be empty!")]
        [MinLength(3, ErrorMessage = "The name must be longer than 3 characters!")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value; 
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Typename of the resource
        /// </summary>
        public string Type => _model.Type;

        /// <summary>
        /// Local identifier of the resource
        /// </summary>
        public string LocalIdentifier
        {
            get { return _localIdentifier; }
            set
            {
                _localIdentifier = value; 
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Global identifier of this resource
        /// </summary>
        public string GlobalIdentifier
        {
            get { return _globalIdentifier; }
            set
            {
                _globalIdentifier = value; 
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Description of this resource
        /// </summary>
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Properties of this resource
        /// </summary>
        public Entry[] Properties
        {
            get { return _properties; }
            set
            {
                _properties = value;
                NotifyOfPropertyChange();
            }
        }

        #region IEditableObject

        ///
        public virtual void BeginEdit()
        {

        }

        ///
        public virtual void EndEdit()
        {
            if (!IsValid)
                return;

            CopyToModel();
        }

        protected void CopyToModel()
        {
            _model.Name = Name;
            _model.GlobalIdentifier = GlobalIdentifier;
            _model.LocalIdentifier = LocalIdentifier;
            _model.Description = Description;

            _model.Properties = Properties.ToList();
        }

        ///
        public virtual void CancelEdit()
        {
            CopyFromModel();
        }

        protected void CopyFromModel()
        {
            Name = _model.Name;
            GlobalIdentifier = _model.GlobalIdentifier;
            LocalIdentifier = _model.LocalIdentifier;
            Description = _model.Description;

            if (_model.Properties != null)
            {
                Properties = _model.Properties.Select(property => property.Clone() as Entry).ToArray();
            }
        }

        #endregion
    }
}