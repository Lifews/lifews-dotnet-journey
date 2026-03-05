using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Demo.AdornerExample;

/// <summary>
/// 测量装饰器 - 显示尺寸信息
/// </summary>
public class MeasurementAdorner : Adorner
{
    private string text;
    private Brush backgroundBrush;

    public string Text
    {
        get => text;
        set
        {
            text = value;
            InvalidateVisual();
        }
    }

    public MeasurementAdorner(UIElement adornedElement, string text = "")
        : base(adornedElement)
    {
        this.text = text;
        backgroundBrush = new SolidColorBrush(Color.FromArgb(180, 255, 255, 220)); // 半透明白色
        IsHitTestVisible = false;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);

        // 创建格式化文本
        FormattedText formattedText = new FormattedText(
            $"尺寸: {adornedElementRect.Width:F0} × {adornedElementRect.Height:F0}\n{text}",
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new Typeface("Segoe UI"),
            12,
            Brushes.Black,
            VisualTreeHelper.GetDpi(this).PixelsPerDip
        );

        // 计算文本位置（在元素下方）
        Point textPosition = new Point(adornedElementRect.Left, adornedElementRect.Bottom + 5);

        // 绘制文本背景
        Rect textBackground = new Rect(
            textPosition.X - 2,
            textPosition.Y - 2,
            formattedText.Width + 4,
            formattedText.Height + 4
        );

        drawingContext.DrawRoundedRectangle(
            backgroundBrush,
            new Pen(Brushes.Gray, 1),
            textBackground,
            3,
            3
        );

        // 绘制文本
        drawingContext.DrawText(formattedText, textPosition);

        // 绘制连接线
        drawingContext.DrawLine(
            new Pen(Brushes.Gray, 1) { DashStyle = DashStyles.Dot },
            new Point(
                adornedElementRect.Left + adornedElementRect.Width / 2,
                adornedElementRect.Bottom
            ),
            new Point(adornedElementRect.Left + adornedElementRect.Width / 2, textPosition.Y)
        );
    }
}
