using HalfDog.EasyInteractive;
using System;

/// <summary>
/// 拖拽物体到UI上的交互情景
/// </summary>
[InteractCase(typeof(SceneItem), typeof(UIItem))]
public class DragToUI : DragSubjectFocusTargetInteractCase
{
	private SceneItem sceneItem;
	private UIItem uiItem;
	public DragToUI(Type subject, Type target) : base(subject, target)
	{
	}
	protected override void OnEnter(IDragable subject, IFocusable target)
	{
		sceneItem = (subject as SceneItem);
		uiItem = (target as UIItem);
	}
	protected override void OnExecute(IDragable subject, IFocusable target)
	{
		if (EndDrag)
		{
			uiItem.icon.gameObject.SetActive(true);
			uiItem.icon.sprite = sceneItem.iconSprite;
		}
	}
	protected override void OnExit()
	{
		sceneItem.gameObject.SetActive(false);
	}
}
