using Modding_Assistant.MVVM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Modding_Assistant.MVVM.View.Markup
{
    public class LocalizeExtension : MarkupExtension
    {
        public string Key { get; set; }
        public LocalizeExtension(string key) => Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var locService = (ILocalizationService)((App)App.Current)._host.Services.GetService(typeof(ILocalizationService));
            Debug.WriteLine(Key);
            return locService?.GetString(Key) ?? $"[{Key}]";
        }
    }
}
