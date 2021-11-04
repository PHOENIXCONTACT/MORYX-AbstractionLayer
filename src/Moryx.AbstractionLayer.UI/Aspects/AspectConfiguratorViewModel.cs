// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Moryx.ClientFramework.Commands;
using Moryx.WpfToolkit;
using Moryx.ClientFramework.Dialog;
using Moryx.Container;

namespace Moryx.AbstractionLayer.UI.Aspects
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
            get => _selectedConfiguration;
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
            get => _aspects;
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
            get => _selectedAspect;
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
            get => _selectedNewAspect;
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

            OkCmd = new AsyncCommand(Ok, _ => true, true);
            AddNewAspectCmd = new RelayCommand(AddNewAspect, CanAddNewAspect);
            RemoveAspectCmd = new RelayCommand(RemoveAspect, CanRemoveAspect);
        }

        /// <inheritdoc />
        protected override Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            var newTypes = _types.Where(pluginName => !Configurations.Select(c => c.TypeName).Contains(pluginName)).ToArray();
            foreach (var newType in newTypes)
                Configurations.Add(new TypedAspectConfiguration {TypeName = newType});

            var unknownTypes = Configurations.Where(pluginName => !_types.Contains(pluginName.TypeName)).ToArray();
            foreach (var unknownType in unknownTypes)
                Configurations.Remove(unknownType);

            var plugins = Container.GetRegisteredImplementations(typeof(IAspect)).Select(t => t.Name);
            PossibleAspects = plugins.ToArray();

            return base.OnInitializeAsync(cancellationToken);
        }

        private Task Ok(object obj)
        {
            return TryCloseAsync(true);
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
