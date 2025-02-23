using HalfDog.EasyInteractive;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[InteractCase(typeof(SceneItem), typeof(UIItem))]
public class DragToUI : DragSubjectFocusTargetInteractCase
{
	private SceneItem sceneItem;
	public DragToUI(Type subject, Type target) : base(subject, target)
	{
	}
	protected override void OnEnter(IDragable subject, IFocusable target)
	{
		Debug.Log("Enter Case");
		sceneItem = (subject as SceneItem);
	}
	protected override void OnExecute(IDragable subject, IFocusable target)
	{
		if (EndDrag)
		{
			Debug.Log("Execute");
			(target as UIItem).icon.gameObject.SetActive(true);
			(target as UIItem).icon.sprite = sceneItem.iconSprite;
		}
	}
	protected override void OnExit()
	{
		Debug.Log("Exit Case");
		sceneItem.gameObject.SetActive(false);
	}
}
