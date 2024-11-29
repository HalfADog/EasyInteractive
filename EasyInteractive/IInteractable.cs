using System;

namespace HalfDog.EasyInteractive
{
	public interface IInteractable
	{
		/// <summary>
		/// 交互对象标识
		/// </summary>
		public Type interactTag { get; }
	}

	/// <summary>
	/// 交互：可聚焦接口
	/// </summary>
	public interface IFocusable : IInteractable
	{
		public bool enableFocus { get; }

		/// <summary>
		/// 焦点进入
		/// </summary>
		public void OnFocus();

		/// <summary>
		/// 焦点离开
		/// </summary>
		public void EndFocus();
	}

	public interface ISelectable : IFocusable, IInteractable
	{
		public bool enableSelect { get; }

		/// <summary>
		/// 选择
		/// </summary>
		public void OnSelect();

		/// <summary>
		/// 取消选择
		/// </summary>
		public void EndSelect();
	}

	public interface IDragable : IFocusable, IInteractable
	{
		public bool enableDrag { get; }

		/// <summary>
		/// 开始拖拽
		/// </summary>
		public void OnDrag();

		/// <summary>
		/// 拖拽中
		/// </summary>
		public void ProcessDrag();

		/// <summary>
		/// 结束拖拽
		/// </summary>
		public void EndDrag();
	}
}