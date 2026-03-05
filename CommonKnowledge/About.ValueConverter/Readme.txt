总结：

0、掌握基础知识
	了解IValueConverter和IMultiValueConverter接口的定义和作用
	了解ValueConverter在数据绑定中的使用场景

1、掌握FuncValueConverter的用法，很实用。
	对于单次使用（在单个View中使用）的转换器，可以不单独创建类，而是把转换逻辑定义在ViewModel中，简化代码结构。

2、对于需要频繁定制的Converter，请使用MarkupExtension声明，便于传参

3、理解这两个的区别
	DependencyProperty.UnsetValue // 把这条 DP 的值重置为未赋值状态
	Binding.DoNothing // 这次更新就当什么都没发生”（既不改目标，也不改源，停止本次数据流）
	 
4、有意思的第三方库
	CalcBinding		    允许在Binding中进行计算
	CompiledBindings	允许使用编译的绑定