namespace Modding_Assistant.Core.Application
{
    /// <summary>
    /// Interface for basic application initialization
    /// </summary>
    public interface IApplicationInitializer
    {
        /// <summary>
        /// Async method for basic application initialization
        /// </summary>
        public Task InitializeAsync(CancellationToken cancellationToken);
    }
}
