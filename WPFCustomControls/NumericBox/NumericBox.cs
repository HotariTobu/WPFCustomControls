using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFCustomControls
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPFCustomControls.NumericBox"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WPFCustomControls;assembly=WPFCustomControls"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:NumericBox/>
    ///
    /// </summary>
    public class NumericBox : TextBox
    {
        static NumericBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericBox), new FrameworkPropertyMetadata(typeof(NumericBox)));

            TextProperty.OverrideMetadata(typeof(NumericBox), new FrameworkPropertyMetadata(
                (d, e) => ((NumericBox)d).OnTextChanged(),
                (d, v) => ((NumericBox)d).CoerceText((string)v)));
        }

        #region == DoubleValue ==

        public static readonly DependencyProperty DoubleValueProperty = DependencyProperty.Register(nameof(DoubleValue), typeof(double), typeof(NumericBox), new FrameworkPropertyMetadata(0d,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            (d, e) => ((NumericBox)d).OnDoubleValueChanged()));
        public double DoubleValue { get => (double)GetValue(DoubleValueProperty); set => SetValue(DoubleValueProperty, value); }

        protected virtual void OnDoubleValueChanged()
        {
            UpdateValues(() =>
            {
                double value = DoubleValue;
                UpdateText(value);
                IntValue = (int)value;
            });
        }

        #endregion
        #region == IntValue ==

        public static readonly DependencyProperty IntValueProperty = DependencyProperty.Register(nameof(IntValue), typeof(int), typeof(NumericBox), new FrameworkPropertyMetadata(0,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            (d, e) => ((NumericBox)d).OnIntValueChanged()));
        public int IntValue { get => (int)GetValue(IntValueProperty); set => SetValue(IntValueProperty, value); }

        protected virtual void OnIntValueChanged()
        {
            UpdateValues(() =>
            {
                int value = IntValue;
                UpdateText(value);
                DoubleValue = value;
            });
        }

        #endregion

        #region == Text ==

        private string LastText = "";

        protected virtual void OnTextChanged()
        {
            if (double.TryParse(Text, out double doubleValue))
            {
                DoubleValue = doubleValue;
            }

            if (int.TryParse(Text, out int intValue))
            {
                IntValue = intValue;
            }
            else
            {
                IntValue = (int)doubleValue;
            }

            LastText = Text;
        }

        private void UpdateText<T>(T value)
        {
            string text = value.ToString();
            if (!Text.Equals(text))
            {
                Text = text;
                Select(text.Length, 0);
            }
        }

        #endregion

        #region == IsUpdatingValues ==

        private bool IsUpdatingValues = false;
        private void UpdateValues(Action action)
        {
            if (!IsUpdatingValues)
            {
                IsUpdatingValues = true;
                action();
                IsUpdatingValues = false;
            }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            CommandManager.AddPreviewExecutedHandler(this, OnPreviewExecuted);
        }

        #region == Validation ==

        private static Regex ValidationPattern = new Regex(@"-?\d*\.?\d*", RegexOptions.Compiled);

        private string CoerceText(string value)
        {
            Match match = ValidationPattern.Match(value);
            if (match.Success && match.Value.Equals(value))
            {
                return value;
            }
            else
            {
                return LastText;
            }
        }

        private void OnPreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        #endregion
    }
}
