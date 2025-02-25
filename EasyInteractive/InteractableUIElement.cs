using UnityEngine;
using UnityEngine.EventSystems;

namespace HalfDog.EasyInteractive
{
	/// <summary>
	/// 可交互UI元素
	/// </summary>
	public abstract class InteractableUIElement : MonoBehaviour, 
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerMoveHandler
    {
        public bool enableInteract = true;

        private IFocusable _focusable = null;
		private ISelectable _selectable = null;
        private IDragable _dragable = null;

		public void OnPointerEnter(PointerEventData eventData)
        {
            if (!enableInteract) return;
			_focusable = (this as IFocusable);
			_selectable = (this as ISelectable);
			_dragable = (this as IDragable);
			if (_focusable != null && _focusable.enableFocus) 
            {
                EasyInteractive.Instance.SetCurrentFocused(_focusable, true);
            }
            if (_selectable != null && _selectable.enableSelect)
            {
                EasyInteractive.Instance.readySelect = _selectable;
            }
            if (_dragable != null && _dragable.enableDrag) 
            {
                EasyInteractive.Instance.readyDrag = _dragable;
            }
        }

		public void OnPointerMove(PointerEventData eventData)
		{
			if (!enableInteract) return;
			if (_selectable != null && _selectable.enableSelect)
			{
				EasyInteractive.Instance.readySelect = _selectable;
			}
			if (_dragable != null && _dragable.enableDrag)
			{
				EasyInteractive.Instance.readyDrag = _dragable;
			}
		}

		public void OnPointerExit(PointerEventData eventData)
        {
            if (!enableInteract) return;
            if (_focusable != null && _focusable.enableFocus)
            {
                if (EasyInteractive.Instance.currentFocused == _focusable) 
                {
                    EasyInteractive.Instance.SetCurrentFocused(null,true);
                }
            }
            if (_selectable != null && _selectable.enableSelect)
            {
                if(EasyInteractive.Instance.readySelect == _selectable)
                    EasyInteractive.Instance.readySelect = null;
            }
            if (_dragable != null && _dragable.enableDrag)
            {
                if (EasyInteractive.Instance.readyDrag == _dragable)
                    EasyInteractive.Instance.readyDrag = null;
            }
        }
	}
}
