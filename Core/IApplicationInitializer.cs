using Microsoft.Extensions.Hosting;
using Modding_Assistant.MVVM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modding_Assistant.Core
{
    public interface IApplicationInitializer
    {
        public Task InitializeAsync(CancellationToken cancellationToken);
    }
}
