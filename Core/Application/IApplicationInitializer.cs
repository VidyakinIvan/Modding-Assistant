using Microsoft.Extensions.Hosting;
using Modding_Assistant.MVVM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
