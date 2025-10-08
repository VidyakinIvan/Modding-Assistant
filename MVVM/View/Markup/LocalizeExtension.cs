using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Modding_Assistant.MVVM.Services.Interfaces;

namespace Modding_Assistant.MVVM.View.Markup
{
    public class LocalizeExtension(string key) : MarkupExtension
    {
        public string Key { get; set; } = key;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Application.Current.TryFindResource("LocalizationService") is ILocalizationService localizationService)
            {
                var binding = new Binding($"[{Key}]")
                {
                    Source = localizationService,
                    Mode = BindingMode.OneWay
                };
                return binding.ProvideValue(serviceProvider);
            }
            return $"[{Key}]";
        }
    }
}
