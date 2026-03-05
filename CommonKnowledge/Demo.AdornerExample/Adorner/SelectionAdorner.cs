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
/// 选择装饰器 - 绘制蓝色的选择框
/// </summary>
public class SelectionAdorner : Adorner
{
    private Brush brush;
    private Pen pen;

    public Brush Brush
    {
        get => brush;
        set
        {
            brush = value;
            InvalidateVisual();
        }
    }

    public Pen Pen
    {
        get => pen;
        set
        {
            pen = value;
            InvalidateVisual();
        }
    }

    public SelectionAdorner(UIElement adornedElement)
        : base(adornedElement)
    {
        brush = new SolidColorBrush(Color.FromArgb(32, 0, 0, 255)); // 半透明蓝色
        pen = new Pen(Brushes.Blue, 1);
        pen.DashStyle = DashStyles.Dash;

        IsHitTestVisible = false;
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);

        // 绘制选择框
        drawingContext.DrawRectangle(brush, pen, adornedElementRect);

        // 在四个边中心添加小方块
        double centerSize = 6;
        double halfCenter = centerSize / 2;

        // 上边中心
        drawingContext.DrawRectangle(
            Brushes.Blue,
            null,
            new Rect(
                adornedElementRect.Left + adornedElementRect.Width / 2 - halfCenter,
                adornedElementRect.Top - halfCenter,
                centerSize,
                centerSize
            )
        );

        // 下边中心
        drawingContext.DrawRectangle(
            Brushes.Blue,
            null,
            new Rect(
                adornedElementRect.Left + adornedElementRect.Width / 2 - halfCenter,
                adornedElementRect.Bottom - halfCenter,
                centerSize,
                centerSize
            )
        );

        // 左边中心
        drawingContext.DrawRectangle(
            Brushes.Blue,
            null,
            new Rect(
                adornedElementRect.Left - halfCenter,
                adornedElementRect.Top + adornedElementRect.Height / 2 - halfCenter,
                centerSize,
                centerSize
            )
        );

        // 右边中心
        drawingContext.DrawRectangle(
            Brushes.Blue,
            null,
            new Rect(
                adornedElementRect.Right - halfCenter,
                adornedElementRect.Top + adornedElementRect.Height / 2 - halfCenter,
                centerSize,
                centerSize
            )
        );
    }
}
