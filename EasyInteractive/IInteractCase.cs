using System;

namespace HalfDog.EasyInteractive
{
	/// <summary>
	/// 交互情景接口
	/// </summary>
	public interface IInteractCase
	{
		public Type subject { get; }
		public Type target { get; }
		public bool enable { get; set; }
		/// <summary>
		/// 执行交互情景
		/// </summary>
		/// <param name="focusable">当前聚焦对象</param>
		/// <param name="selectable">当前选择对象</param>
		/// <param name="dragable">当前拖拽对象</param>
		/// <returns></returns>
		public bool Execute(IFocusable focusable, ISelectable selectable, IDragable dragable);
	}

	/// <summary>
	/// 交互情景抽象类
	/// </summary>
	public abstract class AbstractInteractCase : IInteractCase
	{
		private Type _subject;
		private Type _target;
		private bool _enable;
		public Type subject => _subject;
		public Type target => _target;

		/// <summary>
		/// 是否开启
		/// </summary>
		public bool enable
		{
			get => _enable;
			set => _enable = value;
		}

		public AbstractInteractCase(Type subject, Type target)
		{
			_subject = subject;
			_target = target;
		}

		public abstract bool Execute(IFocusable focusable, ISelectable selectable, IDragable dragable);
	}
}
