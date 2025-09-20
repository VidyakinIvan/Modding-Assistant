using System;
using System.Windows.Data;
using System.Windows.Markup;
using Modding_Assistant.MVVM.Services.Interfaces;

namespace Modding_Assistant.MVVM.View.Markup
{
    public class LocalizeExtension : MarkupExtension
    {
        public string Key { get; set; }
        public LocalizeExtension(string key) => Key = key;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding($"[{Key}]")
            {
                Source = ((App)App.Current)._host.Services.GetService(typeof(ILocalizationService)),
                Mode = BindingMode.OneWay
            };
            return binding.ProvideValue(serviceProvider);
        }
    }
}
