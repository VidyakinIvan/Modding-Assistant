using System.Windows;

namespace Modding_Assistant.MVVM.AttachedProperties
{
    public static class MoveModsDialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(MoveModsDialogCloser),
                new PropertyMetadata(DialogResultChanged)
            );

        public static bool? GetDialogResult(Window target)
        {
            return (bool?)target.GetValue(DialogResultProperty);
        }

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

        private static void DialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Window window)
            {
                if (e.NewValue is bool dialogResult)
                {
                    window.DialogResult = dialogResult;
                }
            }
        }
    }
}
