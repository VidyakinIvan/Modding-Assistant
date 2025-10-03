using System.Globalization;
using System.Windows.Controls;

namespace Modding_Assistant.MVVM.Validation
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string str && string.IsNullOrWhiteSpace(str))
                return new ValidationResult(false, "Field cannot be empty");
            else if (value is DateOnly date)
                if (date == default)
                    return new ValidationResult(false, "Date cannot be empty");
            return ValidationResult.ValidResult;
        }
    }
}