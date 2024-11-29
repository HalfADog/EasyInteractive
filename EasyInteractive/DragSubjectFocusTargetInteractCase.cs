using System;

namespace HalfDog.EasyInteractive
{
	/// <summary>
	/// 拖拽主体并聚焦目标的交互情景
	/// </summary> 
	public abstract class DragSubjectFocusTargetInteractCase : AbstractInteractCase
	{
		private bool _isEnter = false;
		private bool _isExit = true;

		public DragSubjectFocusTargetInteractCase(Type subject, Type target) : base(subject, target)
		{
		}

		protected abstract void OnExecute(IDragable subject, IFocusable target);

		public override bool Execute(IFocusable focusable, ISelectable selectable, IDragable dragable)
		{
			if (focusable == null || dragable == null || focusable.interactTag != target ||
			    dragable.interactTag != subject)
			{
				if (_isEnter)
				{
					_isEnter = false;
					OnExit(dragable, focusable);
					_isExit = true;
				}

				return false;
			}

			if (_isExit)
			{
				_isExit = false;
				OnEnter(dragable, focusable);
				_isEnter = true;
			}

			OnExecute(dragable, focusable);
			return true;
		}

		/// <summary>
		/// 进入交互情景
		/// </summary>
		protected virtual void OnEnter(IDragable subject, IFocusable target)
		{
		}

		/// <summary>
		/// 退出交互情景
		/// </summary>
		protected virtual void OnExit(IDragable subject, IFocusable target)
		{
		}
	}
}
