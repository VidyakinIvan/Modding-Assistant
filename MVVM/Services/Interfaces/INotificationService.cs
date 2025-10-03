namespace Modding_Assistant.MVVM.Services.Interfaces
{
    /// <summary>
    /// Service for displaying user notifications and dialogs
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Shows error message dialog
        /// </summary>
        void ShowError(string message, string caption);

        /// <summary>
        /// Shows warning message dialog
        /// </summary>
        void ShowWarning(string message, string caption);

        /// <summary>
        /// Shows information message dialog
        /// </summary>
        void ShowInformation(string message, string caption);

        /// <summary>
        /// Shows confirmation dialog with Yes/No buttons
        /// </summary>
        /// <returns>True if user selected Yes, otherwise False</returns>
        bool ShowConfirmation(string message, string caption);
    }
}
