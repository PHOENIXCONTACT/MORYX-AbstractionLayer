using System.Windows.Media;

namespace Marvin.AbstractionLayer.UI
{
    /// <summary>
    /// Severity for the messages which are displayed in the empty details view
    /// </summary>
    public enum MessageSeverity
    {
        /// <summary>
        /// Severity for an error
        /// </summary>
        Error,

        /// <summary>
        /// Severity for an info
        /// </summary>
        Info,

        /// <summary>
        /// Severity for a warning
        /// </summary>
        Warning,
    }
    /// <summary>
    /// This is the empty details view model for all UI.Interaction projects
    /// </summary>
    public abstract class EmptyDetailsViewModelBase : EditModeViewModelBase, IDetailsViewModel
    {
        #region Fields

        private readonly SolidColorBrush _errorColorBrush = Brushes.Red;
        private readonly SolidColorBrush _infoColorBrush = Brushes.Black;
        private readonly SolidColorBrush _warningColorBrush = Brushes.DarkOrange;

        #endregion

        #region Properties

        /// <summary>
        /// Current message to display like the start message or an error
        /// </summary>
        public string CurrentMessage { get; private set; }

        /// <summary>
        /// Color for the current message to highlight for example an error
        /// </summary>
        public SolidColorBrush CurrentColorBrush { get; private set; }

        #endregion

        ///
        public void Initialize(string typeName)
        {
        }

        /// <summary>
        /// Display a message at the empty details view like an info, error or warning
        /// </summary>
        /// <param name="severity">Severity of the message to display</param>
        /// <param name="message">Message to display</param>
        /// <param name="parameters">Parameters which will be included in the message if possible</param>
        public void Display(MessageSeverity severity, string message, params object[] parameters)
        {
            CurrentMessage = string.Format(message, parameters);
            switch (severity)
            {
                case MessageSeverity.Error:
                    CurrentColorBrush = _errorColorBrush;
                    break;
                case MessageSeverity.Warning:
                    CurrentColorBrush = _warningColorBrush;
                    break;
                default:
                    CurrentColorBrush = _infoColorBrush;
                    break;
            }

            NotifyOfPropertyChange(() => CurrentMessage);
            NotifyOfPropertyChange(() => CurrentColorBrush);
        }
    }
}