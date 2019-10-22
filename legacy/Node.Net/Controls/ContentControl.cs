using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Threading;

namespace Node.Net.Controls
{
    public class ContentControl : System.Windows.Controls.ContentControl
    {
        [Browsable(false)]
        public new Brush Background
        {
            get { return base.Background; }
            set { base.Background = value; }
        }
        [Browsable(false)]
        public new Brush BorderBrush
        {
            get { return base.BorderBrush; }
            set { base.BorderBrush = value; }
        }
        [Browsable(false)]
        public new Thickness BorderThickness
        {
            get { return base.BorderThickness; }
            set { base.BorderThickness = value; }
        }
        [Browsable(false)]
        public new FontFamily FontFamily
        {
            get { return base.FontFamily; }
            set { base.FontFamily = value; }
        }
        [Browsable(false)]
        public new double FontSize
        {
            get { return base.FontSize; }
            set { base.FontSize = value; }
        }
        [Browsable(false)]
        public new FontStretch FontStretch
        {
            get { return base.FontStretch; }
            set { base.FontStretch = value; }
        }
        [Browsable(false)]
        public new FontStyle FontStyle
        {
            get { return base.FontStyle; }
            set { base.FontStyle = value; }
        }
        [Browsable(false)]
        public new FontWeight FontWeight
        {
            get { return base.FontWeight; }
            set { base.FontWeight = value; }
        }
        [Browsable(false)]
        public new Brush Foreground
        {
            get { return base.Foreground; }
            set { base.Foreground = value; }
        }
        [Browsable(false)]
        public new object ToolTip
        {
            get { return base.ToolTip; }
            set { base.ToolTip = value; }
        }
        [Browsable(false)]
        public new bool IsTabStop
        {
            get { return base.IsTabStop; }
            set { base.IsTabStop = value; }
        }
        [Browsable(false)]
        public new int TabIndex
        {
            get { return base.TabIndex; }
            set { base.TabIndex = value; }
        }
        [Browsable(false)]
        public new object Content
        {
            get { return base.Content; }
            set { base.Content = value; }
        }
        [Browsable(false)]
        public new string ContentStringFormat
        {
            get { return base.ContentStringFormat; }
            set { base.ContentStringFormat = value; }
        }
        [Browsable(false)]
        public new DataTemplate ContentTemplate
        {
            get { return base.ContentTemplate; }
            set { base.ContentTemplate = value; }
        }
        [Browsable(false)]
        public new DataTemplateSelector ContentTemplateSelector
        {
            get { return base.ContentTemplateSelector; }
            set { base.ContentTemplateSelector = value; }
        }
        [Browsable(false)]
        public new HorizontalAlignment HorizontalContentAlignment
        {
            get { return base.HorizontalContentAlignment; }
            set { base.HorizontalContentAlignment = value; }
        }
        [Browsable(false)]
        public new Thickness Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }
        [Browsable(false)]
        public new VerticalAlignment VerticalContentAlignment
        {
            get { return base.VerticalContentAlignment; }
            set { base.VerticalContentAlignment = value; }
        }
        [Browsable(false)]
        public new double ActualHeight
        {
            get { return base.ActualHeight; }
        }
        [Browsable(false)]
        public new double ActualWidth
        {
            get { return base.ActualWidth; }
        }
        [Browsable(false)]
        public new bool AllowDrop
        {
            get { return base.AllowDrop; }
            set { base.AllowDrop = value; }
        }
        [Browsable(false)]
        public new bool AreAnyTouchesCaptured
        {
            get { return base.AreAnyTouchesCaptured; }
        }
        [Browsable(false)]
        public new bool AreAnyTouchesCapturedWithin
        {
            get { return base.AreAnyTouchesCapturedWithin; }
        }
        [Browsable(false)]
        public new bool AreAnyTouchesDirectlyOver
        {
            get { return base.AreAnyTouchesDirectlyOver; }
        }
        [Browsable(false)]
        public new bool AreAnyTouchesOver
        {
            get { return base.AreAnyTouchesOver; }
        }
        [Browsable(false)]
        public new BindingGroup BindingGroup
        {
            get { return base.BindingGroup; }
            set { base.BindingGroup = value; }
        }
        [Browsable(false)]
        public new BitmapEffect BitmapEffect
        {
#pragma warning disable CS0618 // Type or member is obsolete
            get { return base.BitmapEffect; }
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
            set { base.BitmapEffect = value; }
#pragma warning restore CS0618 // Type or member is obsolete
        }
        [Browsable(false)]
        public new BitmapEffectInput BitmapEffectInput
        {
#pragma warning disable CS0618 // Type or member is obsolete
            get { return base.BitmapEffectInput; }
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning disable CS0618 // Type or member is obsolete
            set { base.BitmapEffectInput = value; }
#pragma warning restore CS0618 // Type or member is obsolete
        }
        [Browsable(false)]
        public new CacheMode CacheMode
        {
            get { return base.CacheMode; }
            set { base.CacheMode = value; }
        }
        [Browsable(false)]
        public new Geometry Clip
        {
            get { return base.Clip; }
            set { base.Clip = value; }
        }
        [Browsable(false)]
        public new bool ClipToBounds
        {
            get { return base.ClipToBounds; }
            set { base.ClipToBounds = value; }
        }
        [Browsable(false)]
        public new CommandBindingCollection CommandBindings
        {
            get { return base.CommandBindings; }
        }
        [Browsable(false)]
        public new ContextMenu ContextMenu
        {
            get { return base.ContextMenu; }
            set { base.ContextMenu = value; }
        }
        [Browsable(false)]
        public new Cursor Cursor
        {
            get { return base.Cursor; }
            set { base.Cursor = value; }
        }
        [Browsable(false)]
        public new object DataContext
        {
            get { return base.DataContext; }
            set { base.DataContext = value; }
        }
        [Browsable(false)]
        public new DependencyObjectType DependencyObjectType
        {
            get { return base.DependencyObjectType; }
        }
        [Browsable(false)]
        public new Size DesiredSize
        {
            get { return base.DesiredSize; }
        }
        [Browsable(false)]
        public new Dispatcher Dispatcher
        {
            get { return base.Dispatcher; }
        }
        [Browsable(false)]
        public new Effect Effect
        {
            get { return base.Effect; }
            set { base.Effect = value; }
        }
        [Browsable(false)]
        public new FlowDirection FlowDirection
        {
            get { return base.FlowDirection; }
            set { base.FlowDirection = value; }
        }
        [Browsable(false)]
        public new bool Focusable
        {
            get { return base.Focusable; }
            set { base.Focusable = value; }
        }
        [Browsable(false)]
        public new Style FocusVisualStyle
        {
            get { return base.FocusVisualStyle; }
            set { base.FocusVisualStyle = value; }
        }
        [Browsable(false)]
        public new bool ForceCursor
        {
            get { return base.ForceCursor; }
            set { base.ForceCursor = value; }
        }
        [Browsable(false)]
        public new bool HasAnimatedProperties
        {
            get { return base.HasAnimatedProperties; }
        }
        [Browsable(false)]
        public new double Height
        {
            get { return base.Height; }
            set { base.Height = value; }
        }
        [Browsable(false)]
        public new HorizontalAlignment HorizontalAlignment
        {
            get { return base.HorizontalAlignment; }
            set { base.HorizontalAlignment = value; }
        }
        [Browsable(false)]
        public new InputBindingCollection InputBindings
        {
            get { return base.InputBindings; }
        }
        [Browsable(false)]
        public new InputScope InputScope
        {
            get { return base.InputScope; }
            set { base.InputScope = value; }
        }
        [Browsable(false)]
        public new bool IsArrangeValid
        {
            get { return base.IsArrangeValid; }
        }
        [Browsable(false)]
        public new bool IsEnabled
        {
            get { return base.IsEnabled; }
            set { base.IsEnabled = value; }
        }
        [Browsable(false)]
        public new bool IsFocused
        {
            get { return base.IsFocused; }
        }
        [Browsable(false)]
        public new bool IsHitTestVisible
        {
            get { return base.IsHitTestVisible; }
            set { base.IsHitTestVisible = value; }
        }
        [Browsable(false)]
        public new bool IsInitialized
        {
            get { return base.IsInitialized; }
        }
        [Browsable(false)]
        public new bool IsInputMethodEnabled
        {
            get { return base.IsInputMethodEnabled; }
        }
        [Browsable(false)]
        public new bool IsKeyboardFocused
        {
            get { return base.IsKeyboardFocused; }
        }
        [Browsable(false)]
        public new bool IsKeyboardFocusWithin
        {
            get { return base.IsKeyboardFocusWithin; }
        }
        [Browsable(false)]
        public new bool IsLoaded
        {
            get { return base.IsLoaded; }
        }
        [Browsable(false)]
        public new bool IsMeasureValid
        {
            get { return base.IsMeasureValid; }
        }
        [Browsable(false)]
        public new bool IsMouseCaptured
        {
            get { return base.IsMouseCaptured; }
        }
        [Browsable(false)]
        public new bool IsMouseCaptureWithin
        {
            get { return base.IsMouseCaptureWithin; }
        }
        [Browsable(false)]
        public new bool IsMouseDirectlyOver
        {
            get { return base.IsMouseDirectlyOver; }
        }
        [Browsable(false)]
        public new bool IsMouseOver
        {
            get { return base.IsMouseOver; }
        }
        [Browsable(false)]
        public new bool IsSealed
        {
            get { return base.IsSealed; }
        }
        [Browsable(false)]
        public new bool IsStylusCaptured
        {
            get { return base.IsStylusCaptured; }
        }
        [Browsable(false)]
        public new bool IsStylusCaptureWithin
        {
            get { return base.IsStylusCaptureWithin; }
        }
        [Browsable(false)]
        public new bool IsStylusDirectlyOver
        {
            get { return base.IsStylusDirectlyOver; }
        }
        [Browsable(false)]
        public new bool IsStylusOver
        {
            get { return base.IsStylusOver; }
        }
        [Browsable(false)]
        public new bool IsVisible
        {
            get { return base.IsVisible; }
        }
        [Browsable(false)]
        public new XmlLanguage Language
        {
            get { return base.Language; }
            set { base.Language = value; }
        }
        [Browsable(false)]
        public new Transform LayoutTransform
        {
            get { return base.LayoutTransform; }
            set { base.LayoutTransform = value; }
        }
        [Browsable(false)]
        public new Thickness Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; }
        }
        [Browsable(false)]
        public new double MaxHeight
        {
            get { return base.MaxHeight; }
            set { base.MaxHeight = value; }
        }
        [Browsable(false)]
        public new double MaxWidth
        {
            get { return base.MaxWidth; }
            set { base.MaxWidth = value; }
        }
        [Browsable(false)]
        public new double MinHeight
        {
            get { return base.MinHeight; }
            set { base.MinHeight = value; }
        }
        [Browsable(false)]
        public new double MinWidth
        {
            get { return base.MinWidth; }
            set { base.MinWidth = value; }
        }
        [Browsable(false)]
        public new double Opacity
        {
            get { return base.Opacity; }
            set { base.Opacity = value; }
        }
        [Browsable(false)]
        public new Brush OpacityMask
        {
            get { return base.OpacityMask; }
            set { base.OpacityMask = value; }
        }
        [Browsable(false)]
        public new bool OverridesDefaultStyle
        {
            get { return base.OverridesDefaultStyle; }
            set { base.OverridesDefaultStyle = value; }
        }
        [Browsable(false)]
        public new DependencyObject Parent
        {
            get { return base.Parent; }
        }
        [Browsable(false)]
        public new int PersistId
        {
#pragma warning disable CS0618 // Type or member is obsolete
            get { return base.PersistId; }
#pragma warning restore CS0618 // Type or member is obsolete
        }
        [Browsable(false)]
        public new Size RenderSize
        {
            get { return base.RenderSize; }
            set { base.RenderSize = value; }
        }
        [Browsable(false)]
        public new Transform RenderTransform
        {
            get { return base.RenderTransform; }
            set { base.RenderTransform = value; }
        }
        [Browsable(false)]
        public new Point RenderTransformOrigin
        {
            get { return base.RenderTransformOrigin; }
            set { base.RenderTransformOrigin = value; }
        }
        [Browsable(false)]
        public new ResourceDictionary Resources
        {
            get { return base.Resources; }
            set { base.Resources = value; }
        }
        [Browsable(false)]
        public new bool SnapsToDevicePixels
        {
            get { return base.SnapsToDevicePixels; }
            set { base.SnapsToDevicePixels = value; }
        }
        [Browsable(false)]
        public new Style Style
        {
            get { return base.Style; }
            set { base.Style = value; }
        }
        [Browsable(false)]
        public new object Tag
        {
            get { return base.Tag; }
            set { base.Tag = value; }
        }
        [Browsable(false)]
        public new ControlTemplate Template
        {
            get { return base.Template; }
            set { base.Template = value; }
        }
        [Browsable(false)]
        public new DependencyObject TemplatedParent
        {
            get { return base.TemplatedParent; }
        }
        [Browsable(false)]
        public new IEnumerable<TouchDevice> TouchesCaptured
        {
            get { return base.TouchesCaptured; }
        }
        [Browsable(false)]
        public new IEnumerable<TouchDevice> TouchesCapturedWithin
        {
            get { return base.TouchesCapturedWithin; }
        }
        [Browsable(false)]
        public new IEnumerable<TouchDevice> TouchesDirectlyOver
        {
            get { return base.TouchesDirectlyOver; }
        }
        [Browsable(false)]
        public new IEnumerable<TouchDevice> TouchesOver
        {
            get { return base.TouchesOver; }
        }
        [Browsable(false)]
        public new TriggerCollection Triggers
        {
            get { return base.Triggers; }
        }
        [Browsable(false)]
        public new string Uid
        {
            get { return base.Uid; }
            set { base.Uid = value; }
        }
        [Browsable(false)]
        public new bool UseLayoutRounding
        {
            get { return base.UseLayoutRounding; }
            set { base.UseLayoutRounding = value; }
        }
        [Browsable(false)]
        public new VerticalAlignment VerticalAlignment
        {
            get { return base.VerticalAlignment; }
            set { base.VerticalAlignment = value; }
        }
        [Browsable(false)]
        public new Visibility Visibility
        {
            get { return base.Visibility; }
            set { base.Visibility = value; }
        }
        [Browsable(false)]
        public new double Width
        {
            get { return base.Width; }
            set { base.Width = value; }
        }
        [Browsable(false)]
        public new bool IsManipulationEnabled
        {
            get { return base.IsManipulationEnabled; }
            set { base.IsManipulationEnabled = value; }
        }
        [Browsable(false)]
        public new string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }
}
