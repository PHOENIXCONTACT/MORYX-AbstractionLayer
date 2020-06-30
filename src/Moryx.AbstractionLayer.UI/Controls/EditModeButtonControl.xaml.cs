// Copyright (c) 2020, Phoenix Contact GmbH & Co. KG
// Licensed under the Apache License, Version 2.0

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using C4I;

namespace Marvin.AbstractionLayer.UI.Controls
{
    /// <summary>
    /// Interaction logic for EditModeButtonControl.xaml
    /// </summary>
    public partial class EditModeButtonControl : UserControl
    {
        public EditModeButtonControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dependency property for the <see cref="EnterEditCmd"/>
        /// </summary>
        public static readonly DependencyProperty EnterEditCmdProperty = DependencyProperty.Register(
            "EnterEditCmd", typeof(ICommand), typeof(EditModeButtonControl), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Property to bind a command to enter the edit mode
        /// </summary>
        public ICommand EnterEditCmd
        {
            get { return (ICommand)GetValue(EnterEditCmdProperty); }
            set { SetValue(EnterEditCmdProperty, value); }
        }

        /// <summary>
        /// Dependency property for the <see cref="EnterEditContent"/>
        /// </summary>
        public static readonly DependencyProperty EnterEditContentProperty = DependencyProperty.Register(
            "EnterEditContent", typeof(object), typeof(EditModeButtonControl), new PropertyMetadata(default(object)));

        /// <summary>
        /// Property to bind the content for the enter edit mode button
        /// </summary>
        public object EnterEditContent
        {
            get { return GetValue(EnterEditContentProperty); }
            set { SetValue(EnterEditContentProperty, value); }
        }

        /// <summary>
        /// Dependency property for the <see cref="CancelEditCmd"/>
        /// </summary>
        public static readonly DependencyProperty CancelEditCmdProperty = DependencyProperty.Register(
            "CancelEditCmd", typeof(ICommand), typeof(EditModeButtonControl), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Property to bind a command to cancel the edit mode
        /// </summary>
        public ICommand CancelEditCmd
        {
            get { return (ICommand)GetValue(CancelEditCmdProperty); }
            set { SetValue(CancelEditCmdProperty, value); }
        }

        /// <summary>
        /// Dependency property for the <see cref="CancelEditContent"/>
        /// </summary>
        public static readonly DependencyProperty CancelEditContentProperty = DependencyProperty.Register(
            "CancelEditContent", typeof(object), typeof(EditModeButtonControl), new PropertyMetadata(default(object)));

        /// <summary>
        /// Property to bind the content for the cancel edit mode button
        /// </summary>
        public object CancelEditContent
        {
            get { return GetValue(CancelEditContentProperty); }
            set { SetValue(CancelEditContentProperty, value); }
        }

        /// <summary>
        /// Dependency property for the <see cref="SaveCmd"/>
        /// </summary>
        public static readonly DependencyProperty SaveCmdProperty = DependencyProperty.Register(
            "SaveCmd", typeof(ICommand), typeof(EditModeButtonControl), new PropertyMetadata(default(ICommand)));

        /// <summary>
        /// Property to bind a command to save the current changes
        /// </summary>
        public ICommand SaveCmd
        {
            get { return (ICommand)GetValue(SaveCmdProperty); }
            set { SetValue(SaveCmdProperty, value); }
        }

        /// <summary>
        /// Dependency property for the <see cref="SaveContent"/>
        /// </summary>
        public static readonly DependencyProperty SaveContentProperty = DependencyProperty.Register(
            "SaveContent", typeof(object), typeof(EditModeButtonControl), new PropertyMetadata(default(object)));

        /// <summary>
        /// Property to bind the content for the save button
        /// </summary>
        public object SaveContent
        {
            get { return GetValue(SaveContentProperty); }
            set { SetValue(SaveContentProperty, value); }
        }
    }
}
