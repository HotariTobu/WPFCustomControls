using System;
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
    ///     xmlns:MyNamespace="clr-namespace:dataset_generator"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:dataset_generator;assembly=dataset_generator"
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
    ///     <MyNamespace:CustomScrollViewer/>
    ///
    /// </summary>
    public class ScalableScrollViewer : ContentControl
    {
        static ScalableScrollViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(typeof(ScalableScrollViewer)));
        }

        #region == HorizontalScrollBarVisibility ==

        public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = DependencyProperty.Register(nameof(HorizontalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));
        public ScrollBarVisibility HorizontalScrollBarVisibility { get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty); set => SetValue(HorizontalScrollBarVisibilityProperty, value); }

        #endregion
        #region == VerticalScrollBarVisibility ==

        public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = DependencyProperty.Register(nameof(VerticalScrollBarVisibility), typeof(ScrollBarVisibility), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));
        public ScrollBarVisibility VerticalScrollBarVisibility { get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty); set => SetValue(VerticalScrollBarVisibilityProperty, value); }

        #endregion

        #region == Scale ==

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register(nameof(Scale), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(1d,
            (d, e) => ((ScalableScrollViewer)d).OnScaleChanged()));
        public double Scale { get => (double)GetValue(ScaleProperty); set => SetValue(ScaleProperty, value); }

        protected virtual void OnScaleChanged()
        {
            UpdateActualContentWidth();
            UpdateActualContentHeight();
        }

        #endregion

        #region == OffsetX ==

        private double MinOffsetX => -ViewportWidth;
        private double MaxOffsetX => ActualContentWidth;

        public static readonly DependencyProperty OffsetXProperty = DependencyProperty.Register(nameof(OffsetX), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(
            (d, e) => ((ScalableScrollViewer)d).OnOffsetXChanged(),
            (d, v) => ((ScalableScrollViewer)d).CoerceOffsetX((double)v)));
        public double OffsetX { get => (double)GetValue(OffsetXProperty); set => SetValue(OffsetXProperty, value); }

        protected virtual void OnOffsetXChanged()
        {
            UpdateX();
        }

        private double CoerceOffsetX(double value)
        {
            if (value < MinOffsetX)
            {
                return MinOffsetX;
            }
            else if (value > MaxOffsetX)
            {
                return MaxOffsetX;
            }

            return value;
        }

        #endregion
        #region == OffsetY ==

        private double MinOffsetY => -ViewportHeight;
        private double MaxOffsetY => ActualContentHeight;

        public static readonly DependencyProperty OffsetYProperty = DependencyProperty.Register(nameof(OffsetY), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(
            (d, e) => ((ScalableScrollViewer)d).OnOffsetYChanged(),
            (d, v) => ((ScalableScrollViewer)d).CoerceOffsetY((double)v)));
        public double OffsetY { get => (double)GetValue(OffsetYProperty); set => SetValue(OffsetYProperty, value); }

        protected virtual void OnOffsetYChanged()
        {
            UpdateY();
        }

        private double CoerceOffsetY(double value)
        {
            if (value < MinOffsetY)
            {
                return MinOffsetY;
            }
            else if (value > MaxOffsetY)
            {
                return MaxOffsetY;
            }

            return value;
        }

        #endregion

        #region == X ==

        private static readonly DependencyPropertyKey XPropertyKey = DependencyProperty.RegisterReadOnly(nameof(X), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty XProperty = XPropertyKey.DependencyProperty;
        public double X { get => (double)GetValue(XProperty); private set => SetValue(XPropertyKey, value); }

        private void UpdateX()
        {
            X = -OffsetX;
        }

        #endregion
        #region == Y ==

        private static readonly DependencyPropertyKey YPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Y), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty YProperty = YPropertyKey.DependencyProperty;
        public double Y { get => (double)GetValue(YProperty); private set => SetValue(YPropertyKey, value); }

        private void UpdateY()
        {
            Y = -OffsetY;
        }

        #endregion

        #region == ViewportWidth ==

        private static readonly DependencyPropertyKey ViewportWidthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewportWidth), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(
            (d, e) => ((ScalableScrollViewer)d).OnViewportWidthChanged()));
        public static readonly DependencyProperty ViewportWidthProperty = ViewportWidthPropertyKey.DependencyProperty;
        public double ViewportWidth { get => (double)GetValue(ViewportWidthProperty); private set => SetValue(ViewportWidthPropertyKey, value); }

        protected virtual void OnViewportWidthChanged()
        {
            UpdateScrollableWidth();
        }

        #endregion
        #region == ViewportHeight ==

        private static readonly DependencyPropertyKey ViewportHeightPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ViewportHeight), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(
            (d, e) => ((ScalableScrollViewer)d).OnViewportHeightChanged()));
        public static readonly DependencyProperty ViewportHeightProperty = ViewportHeightPropertyKey.DependencyProperty;
        public double ViewportHeight { get => (double)GetValue(ViewportHeightProperty); private set => SetValue(ViewportHeightPropertyKey, value); }

        protected virtual void OnViewportHeightChanged()
        {
            UpdateScrollableHeight();
        }

        #endregion

        #region == ContentWidth ==

        private static readonly DependencyPropertyKey ContentWidthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ContentWidth), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(
            (d, e) => ((ScalableScrollViewer)d).OnContentWidthChanged()));
        public static readonly DependencyProperty ContentWidthProperty = ContentWidthPropertyKey.DependencyProperty;
        public double ContentWidth { get => (double)GetValue(ContentWidthProperty); private set => SetValue(ContentWidthPropertyKey, value); }

        protected virtual void OnContentWidthChanged()
        {
            UpdateActualContentWidth();
        }

        #endregion
        #region == ContentHeight ==

        private static readonly DependencyPropertyKey ContentHeightPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ContentHeight), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(
            (d, e) => ((ScalableScrollViewer)d).OnContentHeightChanged()));
        public static readonly DependencyProperty ContentHeightProperty = ContentHeightPropertyKey.DependencyProperty;
        public double ContentHeight { get => (double)GetValue(ContentHeightProperty); private set => SetValue(ContentHeightPropertyKey, value); }

        protected virtual void OnContentHeightChanged()
        {
            UpdateActualContentHeight();
        }

        #endregion

        #region == ActualContentWidth ==

        private static readonly DependencyPropertyKey ActualContentWidthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ActualContentWidth), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(
            (d, e) => ((ScalableScrollViewer)d).OnActualContentWidthChanged()));
        public static readonly DependencyProperty ActualContentWidthProperty = ActualContentWidthPropertyKey.DependencyProperty;
        public double ActualContentWidth { get => (double)GetValue(ActualContentWidthProperty); private set => SetValue(ActualContentWidthPropertyKey, value); }

        protected virtual void OnActualContentWidthChanged()
        {
            UpdateScrollableWidth();
        }

        private void UpdateActualContentWidth()
        {
            ActualContentWidth = ContentWidth * Scale;
        }

        #endregion
        #region == ActualContentHeight ==

        private static readonly DependencyPropertyKey ActualContentHeightPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ActualContentHeight), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(
            (d, e) => ((ScalableScrollViewer)d).OnActualContentHeightChanged()));
        public static readonly DependencyProperty ActualContentHeightProperty = ActualContentHeightPropertyKey.DependencyProperty;
        public double ActualContentHeight { get => (double)GetValue(ActualContentHeightProperty); private set => SetValue(ActualContentHeightPropertyKey, value); }

        protected virtual void OnActualContentHeightChanged()
        {
            UpdateScrollableHeight();
        }

        private void UpdateActualContentHeight()
        {
            ActualContentHeight = ContentHeight * Scale;
        }

        #endregion

        #region == ScrollableWidth ==

        private static readonly DependencyPropertyKey ScrollableWidthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ScrollableWidth), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(
            (d, e) => ((ScalableScrollViewer)d).OnScrollableWidthChanged()));
        public static readonly DependencyProperty ScrollableWidthProperty = ScrollableWidthPropertyKey.DependencyProperty;
        public double ScrollableWidth { get => (double)GetValue(ScrollableWidthProperty); private set => SetValue(ScrollableWidthPropertyKey, value); }

        protected virtual void OnScrollableWidthChanged()
        {
            UpdateComputedHorizontalScrollBarVisibility();
        }

        private void UpdateScrollableWidth()
        {
            ScrollableWidth = ActualContentWidth - ViewportWidth;
        }

        #endregion
        #region == ScrollableHeight ==

        private static readonly DependencyPropertyKey ScrollableHeightPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ScrollableHeight), typeof(double), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata(
            (d, e) => ((ScalableScrollViewer)d).OnScrollableHeightChanged()));
        public static readonly DependencyProperty ScrollableHeightProperty = ScrollableHeightPropertyKey.DependencyProperty;
        public double ScrollableHeight { get => (double)GetValue(ScrollableHeightProperty); private set => SetValue(ScrollableHeightPropertyKey, value); }

        protected virtual void OnScrollableHeightChanged()
        {
            UpdateComputedVerticalScrollBarVisibility();
        }

        private void UpdateScrollableHeight()
        {
            ScrollableHeight = ActualContentHeight - ViewportHeight;
        }

        #endregion

        #region == ComputedHorizontalScrollBarVisibility ==

        private static readonly DependencyPropertyKey ComputedHorizontalScrollBarVisibilityPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ComputedHorizontalScrollBarVisibility), typeof(Visibility), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ComputedHorizontalScrollBarVisibilityProperty = ComputedHorizontalScrollBarVisibilityPropertyKey.DependencyProperty;
        public Visibility ComputedHorizontalScrollBarVisibility { get => (Visibility)GetValue(ComputedHorizontalScrollBarVisibilityProperty); private set => SetValue(ComputedHorizontalScrollBarVisibilityPropertyKey, value); }

        private void UpdateComputedHorizontalScrollBarVisibility()
        {
            Visibility autoVisibility;

            if (ScrollableWidth > 0)
            {
                autoVisibility = Visibility.Visible;
            }
            else
            {
                autoVisibility = Visibility.Collapsed;
            }

            ComputedHorizontalScrollBarVisibility = HorizontalScrollBarVisibility switch
            {
                ScrollBarVisibility.Auto => autoVisibility,
                ScrollBarVisibility.Disabled => Visibility.Collapsed,
                ScrollBarVisibility.Hidden => Visibility.Hidden,
                ScrollBarVisibility.Visible => Visibility.Visible,
                _ => autoVisibility,
            };
        }

        #endregion
        #region == ComputedVerticalScrollBarVisibility ==

        private static readonly DependencyPropertyKey ComputedVerticalScrollBarVisibilityPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ComputedVerticalScrollBarVisibility), typeof(Visibility), typeof(ScalableScrollViewer), new FrameworkPropertyMetadata());
        public static readonly DependencyProperty ComputedVerticalScrollBarVisibilityProperty = ComputedVerticalScrollBarVisibilityPropertyKey.DependencyProperty;
        public Visibility ComputedVerticalScrollBarVisibility { get => (Visibility)GetValue(ComputedVerticalScrollBarVisibilityProperty); private set => SetValue(ComputedVerticalScrollBarVisibilityPropertyKey, value); }

        private void UpdateComputedVerticalScrollBarVisibility()
        {
            Visibility autoVisibility;

            if (ScrollableHeight > 0)
            {
                autoVisibility = Visibility.Visible;
            }
            else
            {
                autoVisibility = Visibility.Collapsed;
            }

            ComputedVerticalScrollBarVisibility = VerticalScrollBarVisibility switch
            {
                ScrollBarVisibility.Auto => autoVisibility,
                ScrollBarVisibility.Disabled => Visibility.Collapsed,
                ScrollBarVisibility.Hidden => Visibility.Hidden,
                ScrollBarVisibility.Visible => Visibility.Visible,
                _ => autoVisibility,
            };
        }

        #endregion

        private ContentControl? Viewport;
        private ContentPresenter? ContentPresenter;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Viewport = GetTemplateChild($"PART_{nameof(Viewport)}") as ContentControl;
            if (Viewport != null)
            {
                Viewport.SizeChanged += Viewport_SizeChanged;
            }

            ContentPresenter = GetTemplateChild($"PART_{nameof(ContentPresenter)}") as ContentPresenter;
            if (ContentPresenter != null)
            {
                ContentPresenter.SizeChanged += ContentPresenter_SizeChanged;
            }
        }

        #region == SizeChanged ==

        private void Viewport_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ViewportWidth = e.NewSize.Width;
            ViewportHeight = e.NewSize.Height;
        }

        private void ContentPresenter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentWidth = e.NewSize.Width;
            ContentHeight = e.NewSize.Height;
        }

        #endregion

        #region == Mouse Events ==

        private Point MouseStartPoint;
        private Point OffsetStartPoint;

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                MouseStartPoint = e.GetPosition(this);
                OffsetStartPoint = new Point(OffsetX, OffsetY);

                Mouse.Capture(this);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition(this);
                OffsetX = MouseStartPoint.X - point.X + OffsetStartPoint.X;
                OffsetY = MouseStartPoint.Y - point.Y + OffsetStartPoint.Y;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            Mouse.Capture(null);
        }

        private static double ScaleFactor = 1.001;
        private static double ScrollFactor = 0.2;

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                double scale = Scale * Math.Pow(ScaleFactor, e.Delta);

                Point offset = e.GetPosition(this);
                Point point = e.GetPosition(ContentPresenter);

                Scale = scale;

                OffsetX = point.X * scale - offset.X;
                OffsetY = point.Y * scale - offset.Y;
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                OffsetX -= e.Delta * ScrollFactor;
            }
            else
            {
                OffsetY -= e.Delta * ScrollFactor;
            }
        }

        #endregion
    }
}
