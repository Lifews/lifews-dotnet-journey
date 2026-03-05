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
/// 高亮装饰器 - 在控件周围绘制红色边框
/// </summary>
public class HighlightAdorner : Adorner
{
    private Brush brush;
    private Pen pen; // 用于绘制轮廓的画笔（颜色+线条样式+粗细）

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

    public HighlightAdorner(UIElement adornedElement)
        : base(adornedElement)
    {
        // 默认值
        brush = new SolidColorBrush(Color.FromArgb(40, 255, 0, 0)); // 半透明红色
        pen = new Pen(Brushes.Red, 2);

        // 允许装饰器接收鼠标事件（可选）
        IsHitTestVisible = false;
    }

    /// <summary>
    /// 渲染逻辑
    /// </summary>
    /// <param name="drawingContext">WPF的2D绘图上下文，相当于画布，提供绘图方法</param>
    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);

        // 获取装饰元素的大小
        Rect adornedElementRect = new Rect(this.AdornedElement.RenderSize);

        // 绘制高亮矩形
        drawingContext.DrawRectangle(brush, pen, adornedElementRect);

        // 添加四个角的三角形标记
        double cornerSize = 10;

        // 左上角
        drawingContext.DrawLine(
            pen,
            new Point(adornedElementRect.Left, adornedElementRect.Top + cornerSize),
            new Point(adornedElementRect.Left + cornerSize, adornedElementRect.Top)
        );

        // 右上角
        drawingContext.DrawLine(
            pen,
            new Point(adornedElementRect.Right - cornerSize, adornedElementRect.Top),
            new Point(adornedElementRect.Right, adornedElementRect.Top + cornerSize)
        );

        // 右下角
        drawingContext.DrawLine(
            pen,
            new Point(adornedElementRect.Right, adornedElementRect.Bottom - cornerSize),
            new Point(adornedElementRect.Right - cornerSize, adornedElementRect.Bottom)
        );

        // 左下角
        drawingContext.DrawLine(
            pen,
            new Point(adornedElementRect.Left + cornerSize, adornedElementRect.Bottom),
            new Point(adornedElementRect.Left, adornedElementRect.Bottom - cornerSize)
        );
    }
}
