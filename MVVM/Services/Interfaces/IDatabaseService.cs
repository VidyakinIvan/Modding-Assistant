using System.Threading;
using System.Threading.Tasks;

namespace Modding_Assistant.MVVM.Services.Interfaces
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Initializes the database (migrations, initial data)
        /// </summary>
        Task InitializeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks the availability of the database
        /// </summary>
        Task<bool> CheckHealthAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Cleans up the database
        /// </summary>
        Task ClearAsync(CancellationToken cancellationToken = default);
    }
}