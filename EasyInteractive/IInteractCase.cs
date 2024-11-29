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
