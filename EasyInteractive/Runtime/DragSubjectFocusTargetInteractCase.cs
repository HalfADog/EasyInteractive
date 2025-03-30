using System;
using UnityEngine;

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
			//拖拽对象和聚焦对象都不能为空且对象类型匹配才能执行交互情景
			if (focusable == null || dragable == null || focusable.interactTag != target ||
			    dragable.interactTag != subject)
			{
				if (_isEnter)
				{
					_isEnter = false;
					OnExit();
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
		protected virtual void OnExit()
		{
		}

		/// <summary>
		/// 是否结束拖拽
		/// </summary>
		protected virtual bool EndDrag=>Input.GetMouseButtonUp(0);
	}
}
