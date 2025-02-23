using HalfDog.EasyInteractive;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : InteractableUIElement,IDragable
{
	public Image icon;
	private bool _enableDrag = true;

	public Type interactTag => typeof(UIItem);
	public bool enableFocus => true;
	public bool enableDrag => _enableDrag;

	public void OnFocus()
	{
		_enableDrag = icon.gameObject.activeSelf;
	}
	public void EndFocus()
	{
		_enableDrag = true;
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
