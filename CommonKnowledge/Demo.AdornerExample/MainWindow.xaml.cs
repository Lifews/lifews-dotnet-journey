using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Demo.AdornerExample;

public partial class MainWindow : Window
{
    // 存储当前活动的装饰器
    private Dictionary<UIElement, List<Adorner>> activeAdorners =
        new Dictionary<UIElement, List<Adorner>>();
    private AdornerLayer currentAdornerLayer;

    public MainWindow()
    {
        InitializeComponent();
        currentAdornerLayer = AdornerLayer.GetAdornerLayer(mainAdornerDecorator);

        // 更新状态
        UpdateStatus();

        // 订阅复选框变化
        chkHitTest.Checked += (s, e) => UpdateHitTestVisibility(true);
        chkHitTest.Unchecked += (s, e) => UpdateHitTestVisibility(false);
    }

    // 获取或创建AdornerLayer
    private AdornerLayer GetAdornerLayerForElement(UIElement element)
    {
        // 尝试获取元素的AdornerLayer
        AdornerLayer layer = AdornerLayer.GetAdornerLayer(element);

        // 如果没有找到，使用主AdornerLayer
        return layer ?? currentAdornerLayer;
    }

    // 添加高亮装饰器
    private void BtnAddHighlight_Click(object sender, RoutedEventArgs e)
    {
        AddAdornerToElement(decoratedButton, new HighlightAdorner(decoratedButton));
    }

    // 移除高亮装饰器
    private void BtnRemoveHighlight_Click(object sender, RoutedEventArgs e)
    {
        RemoveAdornersFromElement(decoratedButton, typeof(HighlightAdorner));
    }

    // 切换选择框
    private void BtnToggleSelection_Click(object sender, RoutedEventArgs e)
    {
        UIElement element = decoratedTextBox;

        if (HasAdorner(element, typeof(SelectionAdorner)))
        {
            RemoveAdornersFromElement(element, typeof(SelectionAdorner));
        }
        else
        {
            AddAdornerToElement(element, new SelectionAdorner(element));
        }
    }

    // 清除所有装饰器
    private void BtnClearAll_Click(object sender, RoutedEventArgs e)
    {
        ClearAllAdorners();
    }

    // 点击装饰按钮
    private void DecoratedButton_Click(object sender, RoutedEventArgs e)
    {
        UIElement element = (UIElement)sender;

        // 切换测量装饰器
        if (HasAdorner(element, typeof(MeasurementAdorner)))
        {
            RemoveAdornersFromElement(element, typeof(MeasurementAdorner));
        }
        else
        {
            var measurementAdorner = new MeasurementAdorner(element, "已点击");
            AddAdornerToElement(element, measurementAdorner);
        }

        infoText.Text = $"已点击 {((Button)element).Content}";
    }

    // 添加装饰器到元素
    private void AddAdornerToElement(UIElement element, Adorner adorner)
    {
        try
        {
            AdornerLayer layer = GetAdornerLayerForElement(element);
            if (layer != null)
            {
                layer.Add(adorner);

                // 记录装饰器
                if (!activeAdorners.ContainsKey(element))
                {
                    activeAdorners[element] = new List<Adorner>();
                }
                activeAdorners[element].Add(adorner);

                UpdateStatus();
                statusText.Text = $"已添加装饰器: {adorner.GetType().Name}";
            }
        }
        catch (Exception ex)
        {
            statusText.Text = $"错误: {ex.Message}";
        }
    }

    // 从元素移除装饰器
    private void RemoveAdornersFromElement(UIElement element, Type adornerType)
    {
        if (activeAdorners.ContainsKey(element))
        {
            var toRemove = activeAdorners[element].Where(a => a.GetType() == adornerType).ToList();

            foreach (var adorner in toRemove)
            {
                AdornerLayer layer = GetAdornerLayerForElement(element);
                if (layer != null)
                {
                    layer.Remove(adorner);
                    activeAdorners[element].Remove(adorner);
                }
            }

            if (activeAdorners[element].Count == 0)
            {
                activeAdorners.Remove(element);
            }

            UpdateStatus();
            statusText.Text = $"已移除 {toRemove.Count} 个装饰器";
        }
    }

    // 检查元素是否有特定类型的装饰器
    private bool HasAdorner(UIElement element, Type adornerType)
    {
        return activeAdorners.ContainsKey(element)
            && activeAdorners[element].Any(a => a.GetType() == adornerType);
    }

    // 清除所有装饰器
    private void ClearAllAdorners()
    {
        foreach (var kvp in activeAdorners)
        {
            AdornerLayer layer = GetAdornerLayerForElement(kvp.Key);
            if (layer != null)
            {
                foreach (var adorner in kvp.Value)
                {
                    layer.Remove(adorner);
                }
            }
        }

        activeAdorners.Clear();
        UpdateStatus();
        statusText.Text = "已清除所有装饰器";
    }

    // 更新装饰器的HitTestVisible属性
    private void UpdateHitTestVisibility(bool isVisible)
    {
        foreach (var kvp in activeAdorners)
        {
            foreach (var adorner in kvp.Value)
            {
                adorner.IsHitTestVisible = isVisible;
            }
        }

        statusText.Text = isVisible ? "装饰器现在会拦截鼠标点击" : "装饰器现在不会拦截鼠标点击";
    }

    // 更新状态显示
    private void UpdateStatus()
    {
        int totalAdorners = activeAdorners.Values.Sum(list => list.Count);
        adornerCountText.Text = totalAdorners.ToString();
    }

    // 鼠标进入装饰元素时显示提示
    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        // 找到鼠标下的元素
        Point position = e.GetPosition(mainContentPanel);
        UIElement element = GetElementAtPosition(position);

        if (element != null && HasAdorner(element, typeof(HighlightAdorner)))
        {
            infoText.Text = $"鼠标在 {element.GetType().Name} 上（有高亮装饰）";
        }
    }

    // 获取指定位置的元素
    private UIElement GetElementAtPosition(Point position)
    {
        var hitTestResult = VisualTreeHelper.HitTest(mainContentPanel, position);
        return hitTestResult?.VisualHit as UIElement;
    }
}
