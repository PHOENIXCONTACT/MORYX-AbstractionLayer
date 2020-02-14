using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using C4I;
using Marvin.ClientFramework.Dialog;
using Marvin.Container;

namespace Marvin.AbstractionLayer.UI.Aspects
{
    /// <summary>
    /// Aspect configurator view model. Created by the <see cref="IAspectConfiguratorFactory"/>
    /// </summary>
    [Plugin(LifeCycle.Transient, typeof(IAspectConfigurator))]
    public class AspectConfiguratorViewModel : DialogScreen, IAspectConfigurator
    {
        #region Dependencies

        /// <summary>
        /// Local container to resolve all registered aspects
        /// </summary>
        public IContainer Container { get; set; }

        #endregion

        private readonly string[] _types;
        private TypedAspectConfiguration _selectedConfiguration;
        private AspectConfiguration _selectedAspect;
        private string _selectedNewAspect;
        private ObservableCollection<AspectConfiguration> _aspects;

        /// <summary>
        /// Current reference to the configuration
        /// </summary>
        public ICollection<TypedAspectConfiguration> Configurations { get; }

        /// <summary>
        /// Possible aspect types
        /// </summary>
        public string[] PossibleAspects { get; private set; }

        /// <summary>
        /// Command to add an aspect
        /// </summary>
        public ICommand AddNewAspectCmd { get; }

        /// <summary>
        /// Command to remove an aspect
        /// </summary>
        public ICommand RemoveAspectCmd { get; }

        /// <summary>
        /// Command for closing the dialog
        /// </summary>
        public ICommand OkCmd { get; }

        /// <summary>
        /// Selected aspect configuration
        /// </summary>
        public TypedAspectConfiguration SelectedConfiguration
        {
            get { return _selectedConfiguration; }
            set
            {
                _selectedConfiguration = value;

                Aspects = _selectedConfiguration != null
                    ? new ObservableCollection<AspectConfiguration>(_selectedConfiguration.Aspects)
                    : null;

                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Local observable collection for the aspects
        /// </summary>
        public ObservableCollection<AspectConfiguration> Aspects
        {
            get { return _aspects; }
            private set
            {
                _aspects = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Selected existing aspect
        /// </summary>
        public AspectConfiguration SelectedAspect
        {
            get { return _selectedAspect; }
            set
            {
                _selectedAspect = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Selected new aspect
        /// </summary>
        public string SelectedNewAspect
        {
            get { return _selectedNewAspect; }
            set
            {
                _selectedNewAspect = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Constructor for the factory to create the configurator
        /// </summary>
        public AspectConfiguratorViewModel(ICollection<TypedAspectConfiguration> current, string[] types)
        {
            _types = types;
            Configurations = current;

            OkCmd = new RelayCommand(Ok);
            AddNewAspectCmd = new RelayCommand(AddNewAspect, CanAddNewAspect);
            RemoveAspectCmd = new RelayCommand(RemoveAspect, CanRemoveAspect);
        }

        /// <inheritdoc />
        protected override void OnInitialize()
        {
            base.OnInitialize();

            var newTypes = _types.Where(pluginName => !Configurations.Select(c => c.TypeName).Contains(pluginName)).ToArray();
            foreach (var newType in newTypes)
                Configurations.Add(new TypedAspectConfiguration {TypeName = newType});

            var unknownTypes = Configurations.Where(pluginName => !_types.Contains(pluginName.TypeName)).ToArray();
            foreach (var unknownType in unknownTypes)
                Configurations.Remove(unknownType);

            var plugins = Container.GetRegisteredImplementations(typeof(IAspect)).Select(t => t.Name);
            PossibleAspects = plugins.ToArray();
        }

        private void Ok(object obj)
        {
            TryClose(true);
        }

        private bool CanRemoveAspect(object obj)
        {
            return SelectedConfiguration != null && SelectedAspect != null;
        }

        private void RemoveAspect(object obj)
        {
            var aspect = SelectedAspect;

            SelectedConfiguration.Aspects.Remove(aspect);
            Aspects.Remove(aspect);

            NotifyOfPropertyChange(nameof(Aspects));
        }

        private bool CanAddNewAspect(object obj)
        {
            return SelectedConfiguration != null && !string.IsNullOrWhiteSpace(SelectedNewAspect);
        }

        private void AddNewAspect(object obj)
        {
            var newAspect = new AspectConfiguration
            {
                PluginName = SelectedNewAspect
            };
            SelectedConfiguration.Aspects.Add(newAspect);
            Aspects.Add(newAspect);

            NotifyOfPropertyChange(nameof(Aspects));
        }
    }
}
