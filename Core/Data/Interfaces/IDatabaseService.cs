namespace Modding_Assistant.Core.Data.Interfaces
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Initializes the database (migrations, initial data)
        /// </summary>
        Task InitializeAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Checks the availability of the database
        /// </summary>
        Task<bool> CheckHealthAsync(CancellationToken cancellationToken);
    }
}