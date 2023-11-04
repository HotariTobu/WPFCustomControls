using System.Windows;
using System.Windows.Controls;

namespace WPFCustomControls
{
    public class Separator : Control
    {
        static Separator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Separator), new FrameworkPropertyMetadata(typeof(Separator)));
        }
    }
}
