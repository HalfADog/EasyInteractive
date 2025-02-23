using HalfDog.EasyInteractive;
using System;
using UnityEngine;

[InteractCase(typeof(UIItem), typeof(Table))]
public class DragToScene : DragSubjectFocusTargetInteractCase
{
	private UIItem uiItem;
	private Table table;
	public DragToScene(Type subject, Type target) : base(subject, target)
	{
	}

	protected override void OnEnter(IDragable subject, IFocusable target)
	{
		Debug.Log("Enter Case");
		uiItem = (subject as UIItem);
		table = (target as Table);
		table.tempItem.gameObject.SetActive(true);
	}

	protected override void OnExecute(IDragable subject, IFocusable target)
	{
		if (Input.GetMouseButtonUp(0)) 
		{
			GameObject.FindObjectOfType<SceneItem>(true).gameObject.SetActive(true);
			Debug.Log("Execute");
		}
	}

	protected override void OnExit()
	{
		Debug.Log("Exit Case");
		table.tempItem.gameObject.SetActive(false);
		uiItem.icon.gameObject.SetActive(false);
	}
}
