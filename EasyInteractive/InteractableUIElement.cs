using UnityEngine;
using UnityEngine.EventSystems;

namespace HalfDog.EasyInteractive
{
    public abstract class InteractableUIElement : MonoBehaviour, 
        IPointerEnterHandler,
        IPointerExitHandler
    {
        public bool enableInteract = true;
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!enableInteract) return;
            if ((this as IFocusable) != null && (this as IFocusable).enableFocus) 
            {
                EasyInteractive.Instance.SetCurrentFocused(this as IFocusable,true);
            }
            if ((this as ISelectable) != null && (this as ISelectable).enableSelect)
            {
                EasyInteractive.Instance.readySelect = (this as ISelectable);
            }
            if ((this as IDragable) != null && (this as IDragable).enableDrag) 
            {
                EasyInteractive.Instance.readyDrag = (this as IDragable);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!enableInteract) return;
            if ((this as IFocusable) != null && (this as IFocusable).enableFocus)
            {
                if (EasyInteractive.Instance.currentFocused == (this as IFocusable)) 
                {
                    EasyInteractive.Instance.SetCurrentFocused(null,true);
                }
            }
            if ((this as ISelectable) != null && (this as ISelectable).enableSelect)
            {
                if((this as ISelectable) == EasyInteractive.Instance.readySelect)
                    EasyInteractive.Instance.readySelect = null;
            }
            if ((this as IDragable) != null && (this as IDragable).enableDrag)
            {
                if ((this as IDragable) == EasyInteractive.Instance.readyDrag)
                    EasyInteractive.Instance.readyDrag = null;
            }
        }
    }
}
