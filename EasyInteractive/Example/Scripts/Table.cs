using System;
using UnityEngine;
using HalfDog.EasyInteractive;

public class Table : MonoBehaviour, IFocusable
{
	public GameObject tempItem;
	public Type interactTag => typeof(Table);
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
}
