using System.Windows;
using System.Windows.Controls;

namespace WPFCustomControls
{
    public class PathBox : Control
    {
        static PathBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathBox), new FrameworkPropertyMetadata(typeof(PathBox)));
        }
    }
}
