# WPF 3D 中的一些重要概念：

在计算机图形学中，绝大多数3D模型都是通过表面来表示的，因为显示的是表面，内部不需要绘制。这种表示方法叫做“边界表示法”（Boundary Representation，B-rep），**即用一组曲面（通常是三角形面片）**来定义物体的边界。

​	为什么是三角形面片？

**三角形面片是理论和实践的最佳平衡点：**

1. **数学完备性**：三角形是几何学中最简单的多边形
2. **计算确定性**：三点确定唯一的平面，没有歧义
3. **硬件友好性**：GPU架构针对三角形优化了几十年
4. **灵活性**：可以通过细分任意逼近曲面
5. **标准化**：成为整个行业的通用语言

WPF 3D的本质是，几何模型不动，相机在动，所以一个Viewport呈现的一个模型不同视角实际上是，用相机拍出来的不同视角，模型本身是不移动的。这样性能大幅度优化。

## Viewport3D中有什么？

```wiki
Viewport3D (显示区域)
    └─ Visual3DCollection (可视化对象集合)
        └─ ModelVisual3D (可视化容器)
            └─ Content (内容)
                └─ Model3DGroup (模型组)
                    ├─ GeometryModel3D (几何模型)
                    ├─ DirectionalLight (光源)
                    ├─ AmbientLight (光源)
                    └─ Model3DGroup (嵌套子组)
```



| 特性         | Model3DGroup                     | ModelVisual3D              |
| :----------- | :------------------------------- | :------------------------- |
| **继承关系** | Model3D → Model3DGroup           | Visual3D → ModelVisual3D   |
| **本质**     | 3D**内容**（数据）               | 3D**可视化对象**（容器）   |
| **作用**     | 组织和管理3D模型（几何、灯光等） | 将3D内容显示在场景中       |
| **类比**     | 文件夹（包含文件）               | 相框（展示照片）           |
| **坐标系统** | 有自己的变换矩阵                 | 有变换矩阵，影响所有子内容 |

## ProjectionCamera子类

| 特性         | 透视相机 (PerspectiveCamera) | 正交相机 (OrthographicCamera) |
| :----------- | :--------------------------- | :---------------------------- |
| **投影方式** | 中心投影                     | 平行投影                      |
| **视觉感受** | 有近大远小的透视效果         | 无透视，远近物体大小相同      |
| **真实感**   | 高（类似人眼）               | 低（类似工程图）              |
| **适用场景** | 游戏、仿真、真实感渲染       | CAD、工程制图、2D游戏         |

**1. 透视相机 (PerspectiveCamera)**

透视相机模拟人眼或真实相机的观察方式，具有“近大远小”的透视效果。物体距离相机越远，看起来就越小。

```C#
// PerspectiveCamera 透视投影相机类

/// <param name="position">相机在三维空间中的坐标位置</param>
/// <param name="lookDirection">相机观察的方向向量,向量会被自动标准化</param>
/// <param name="upDirection">定义相机的"向上"方向</param>
/// <param name="fieldOfView">相机的视野角度（水平方向,范围1-179）</param>
public PerspectiveCamera(
    Point3D position,
    Vector3D lookDirection,
    Vector3D upDirection,
    double fieldOfView
);
```

```C#
//  HelixToolkit 中，在 CameraHelper 处定义了 PerspectiveCamera 透视投影相机类的默认设置如下：

/// <summary>
/// Resets the specified camera.
/// </summary>
/// <param name="camera">
/// The camera.
/// </param>
public static void Reset(this Camera? camera)
{
    if (camera is null)
    {
        throw new ArgumentNullException(nameof(camera));
    }
    if (camera is not ProjectionCamera projectionCamera)
    {
        throw new NotSupportedException(nameof(camera));
    }
    projectionCamera.Position = new Point3D(2, 16, 20);
    projectionCamera.LookDirection = new Vector3D(-2, -16, -20);
    projectionCamera.UpDirection = new Vector3D(0, 0, 1);
    projectionCamera.NearPlaneDistance = 0.1;
    projectionCamera.FarPlaneDistance = double.PositiveInfinity;
    if (camera is PerspectiveCamera pcamera)
    {
        pcamera.FieldOfView = 45;
    }
    else if (camera is OrthographicCamera ocamera)
    {
        ocamera.Width = 40;
    }
}
```



# HelixToolkit的源码学习

## 设计原则：单一职责原则

**HelixViewport3D 的职责**：

- 作为完整控件的容器
- 管理所有UI元素（标题、状态、坐标系统、ViewCube等）
- 提供整体的3D视图环境

**CameraController 的职责**：（实际上功能也会分类去实现，现在我们先把他们看作一个整体）

- 专门处理相机的交互控制
- 实现旋转、平移、缩放的具体算法
- 处理鼠标/键盘/触摸输入

**属性重合**：HelixViewport3D和CameraController有着重合的依赖属性

```C#
// HelixViewport3D 作为代理，将属性传递给 CameraController
public bool IsRotationEnabled
{
    get { return (bool)GetValue(IsRotationEnabledProperty); }
    set { SetValue(IsRotationEnabledProperty, value); }
}

// 在模板中通过绑定传递
// 在XAML模板中：
<CameraController 
    IsRotationEnabled="{TemplateBinding IsRotationEnabled}"
    IsZoomEnabled="{TemplateBinding IsZoomEnabled}"
    ... />
```

**为什么不直接在 HelixViewport3D 中实现？**

1. **可复用性**：`CameraController` 可以独立使用

   ```csharp
   // CameraController 可以单独使用
   var cameraController = new CameraController();
   cameraController.Viewport = myViewport;
   ```

2. **关注点分离**：交互逻辑和UI逻辑分离

   ```csharp
   // CameraController 专注于交互算法
   public class CameraController : Control
   {
       // 复杂的数学计算和状态管理都在这里
       private void HandleRotation(Point currentPosition);
       private void HandleZoom(double delta);
   }
   ```

3. **可测试性**：可以单独测试 CameraController 的交互逻辑

## 如何处理输入事件响应？

- **InputBinding**和**重写输入事件方法**实现交互

**Input中可能用的到的几个类**

```C#
public static class Cursors
    
// 以下的都是Cursors中的属性,可更改光标样式

// AppStarting: 应用程序启动时显示的光标。
// Arrow: 标准箭头光标。
// ArrowCD: 带有光盘图标的箭头光标。
// Cross: 十字线光标。
// Hand: 手形光标，通常表示可以点击。
// Help: 帮助光标，通常是箭头和问号的组合。
// IBeam: I型光标，用于文本选择。
// No: 禁止光标，表示当前操作无效。
// None: 不可见光标。
// Pen: 笔形光标。
// ScrollAll: 滚动所有方向的光标。
// ScrollE: 向右滚动光标。
// ScrollN: 向上滚动光标。
// ScrollNE: 向右上滚动光标。
// ScrollNS: 垂直滚动光标（上下）。
// ScrollNW: 向左上滚动光标。
// ScrollS: 向下滚动光标。
// ScrollSE: 向右下滚动光标。
// ScrollSW: 向左下滚动光标。
// ScrollW: 向左滚动光标。
// ScrollWE: 水平滚动光标（左右）。
// SizeAll: 四向箭头，用于调整大小。
// SizeNESW: 指向东北和西南的双向箭头，用于调整大小。
// SizeNS: 指向南北的双向箭头，用于调整大小。
// SizeNWSE: 指向西北和东南的双向箭头，用于调整大小。
// SizeWE: 指向东西的双向箭头，用于调整大小。
// UpArrow: 向上箭头光标。
// Wait: 等待（沙漏）光标，表示程序忙。
```

### InputBinding的用法

```C#
public static RoutedCommand RotateCommand { get; private set; }
public static RoutedCommand ZoomCommand { get; private set; }
public static RoutedCommand ResetCameraCommand { get; private set; }

static MyCameraController()
{
	// 1. 设置类级别的默认值
    // 2. 初始化静态成员
    // 3. 注册命令
    
    
    // 举例：设置背景透明
    BackgroundProperty.OverrideMetadata(
        typeof(MyCameraController),
        new FrameworkPropertyMetadata(Brushes.Transparent)
    );
    
    // 举例：注册命令
    RotateCommand = new RoutedCommand();
    ZoomCommand = new RoutedCommand();
    ResetCameraCommand = new RoutedCommand();
}
```

```C#
public MyCameraController()
{
    // 在实例构造函数中绑定命令
    this.CommandBindings.Add(new CommandBinding(RotateCommand, OnRotateCommand));
    this.CommandBindings.Add(new CommandBinding(ZoomCommand, OnZoomCommand));
    this.CommandBindings.Add(new CommandBinding(ResetCameraCommand, OnResetCameraCommand));
    
    // 启用键盘焦点
    this.Focusable = true;
}

private void OnRotateCommand(object sender, ExecutedRoutedEventArgs e)
{
    // 处理旋转命令
    // 可以在这里实现旋转逻辑
    // 或者调用已有的旋转方法
}

private void OnZoomCommand(object sender, ExecutedRoutedEventArgs e)
{
    // 处理缩放命令
}

private void OnResetCameraCommand(object sender, ExecutedRoutedEventArgs e)
{
    // 重置相机
}
```

```XAML
<!-- 方式1：直接使用静态命令 -->
<Window.CommandBindings>
    <CommandBinding 
        Command="local:MyCameraController.ResetCameraCommand"
        Executed="OnResetCamera"/>
</Window.CommandBindings>

<!-- 方式2：按钮绑定 -->
<Button Command="local:MyCameraController.ResetCameraCommand" 
        Content="重置相机"/>

<!-- 方式3：快捷键绑定 -->
<Window.InputBindings>
    <KeyBinding Key="Home" 
                Command="local:MyCameraController.ResetCameraCommand"/>
</Window.InputBindings>
```

### InputBinding的缺陷

如：先按下鼠标左键，再按下鼠标右键，再松开鼠标左键

在松开鼠标左键的时候会同时触发鼠标左右键的MouseUp事件，因为InputBinding无法识别来源，从而引发混乱（有时候也不影响）。

如果全部用事件处理就没这个问题，但是用事件处理又会耦合……

应该有方法解决，暂时不深入了，目前没这个需求…………



# ⭐3D图形基础

3D图形API（OpenGL、DirectX）流程：（基本3D图形开发都是这一套转换流程）

```
相机坐标 → 裁剪坐标：即相机取景器中的画面（近裁切面、远裁切面）
裁剪坐标 → 屏幕坐标：把取景器里的画面拉伸/压缩到银幕尺寸

物体坐标（局部坐标） → 世界坐标  → 相机坐标  →  裁剪坐标  →   屏幕坐标
    ↓            ↓          ↓          ↓          ↓
  模型变换       世界变换   视图变换    投影变换     视口变换
```



























