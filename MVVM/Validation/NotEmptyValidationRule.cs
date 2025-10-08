using System.Globalization;
using System.Windows.Controls;

namespace Modding_Assistant.MVVM.Validation
{
    /// <summary>
    /// Validation rule that ensures a value is not empty, null, or default for its type
    /// </summary>
    /// <remarks>
    /// This rule provides validation for various data types (string, DateOnly, nullables) to ensure they contain meaningful values.
    /// It can be used in WPF data binding validation rules to enforce required field constraints.
    /// </remarks>
    public class NotEmptyValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; } = "Field cannot be empty";
        public string DateErrorMessage { get; set; } = "Date cannot be empty";

        /// <summary>
        /// Validates the specified value against the empty value rules
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationResult"/> that indicates whether the value is valid
        /// </returns>
        /// <remarks>
        /// If all checks pass, returns <see cref="ValidationResult.ValidResult"/>
        /// </remarks>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, ErrorMessage);

            if (value is string str && string.IsNullOrWhiteSpace(str))
                return new ValidationResult(false, ErrorMessage);

            if (value is DateOnly date && date == default)
                return new ValidationResult(false, DateErrorMessage);

            return ValidationResult.ValidResult;
        }
    }
}