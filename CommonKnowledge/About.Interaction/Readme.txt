第三方库：Microsoft.Xaml.Behaviors.Wpf



Interaction类，包括 Behaviors 和 Triggers

关于Triggers，（注:这可不是WPF的那个原生Triggers
1、需要掌握 ChangePropertyAction、CallMethodAction、InvokeCommandAction
	扩展：ControlStoryboardAction、PlaySoundAction
	思考：通过查看Microsoft.Xaml.Behaviors.Wpf源码，解决如何自定义Action（提示：继承TriggerAction<DependencyObject>...override invoke...


关于Behavior
1、如何自定义Behavior（提示：继承Behavior<T>，重写OnAttached、OnDetaching方法）



