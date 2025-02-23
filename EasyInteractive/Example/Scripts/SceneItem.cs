using HalfDog.EasyInteractive;
using System;
using UnityEngine;

public class SceneItem : MonoBehaviour, IDragable
{
	public Sprite iconSprite;
	public Type interactTag => typeof(SceneItem);
	public bool enableDrag => true;
	public bool enableFocus => true;
	private Outline _outline;

	private void Awake()
	{
		_outline = GetComponent<Outline>();
	}

	public void OnFocus()
	{
		_outline.enabled = true;
	}
	public void EndFocus()
	{
		_outline.enabled = false;
	}
	public void OnDrag()
	{
		Debug.Log("Begin Drag");
		GhostIcon.Instance.ShowGhostIcon(iconSprite);
		gameObject.SetActive(false);
	}
	public void ProcessDrag()
	{
	}
	public void EndDrag()
	{
		Debug.Log("End Drag");
		GhostIcon.Instance.HideGhostIcon();
		gameObject.SetActive(true);
	}
}
