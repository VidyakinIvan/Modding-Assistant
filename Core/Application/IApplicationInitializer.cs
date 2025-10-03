namespace Modding_Assistant.Core.Application
{
    /// <summary>
    /// Interface for basic application initializing
    /// </summary>
    public interface IApplicationInitializer
    {
        /// <summary>
        /// Async method for basic application initializing
        /// </summary>
        public Task InitializeAsync(CancellationToken cancellationToken);
    }
}
