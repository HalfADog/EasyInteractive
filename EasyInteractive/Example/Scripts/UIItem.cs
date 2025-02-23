using HalfDog.EasyInteractive;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : InteractableUIElement,IDragable
{
	public Image icon;
	public Type interactTag => typeof(UIItem);
	public bool enableFocus => true;
	public bool enableDrag => true;

	public void OnFocus()
	{
	}
	public void EndFocus()
	{
	}
	public void OnDrag()
	{
		Debug.Log("Begin Drag");
		if (!icon.gameObject.activeSelf) return;
		GhostIcon.Instance.ShowGhostIcon(icon.sprite);
		icon.gameObject.SetActive(false);
	}
	public void ProcessDrag()
	{
	}
	public void EndDrag()
	{
		Debug.Log("End Drag");
		GhostIcon.Instance.HideGhostIcon();
		icon.gameObject.SetActive(true);
	}
}
