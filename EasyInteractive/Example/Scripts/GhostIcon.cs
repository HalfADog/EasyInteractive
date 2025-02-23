using UnityEngine;
using UnityEngine.UI;

public class GhostIcon : MonoBehaviour
{
	public static GhostIcon Instance;
	private Image _icon;
	private bool _isShow = false;

	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		_icon = GetComponent<Image>();
		gameObject.SetActive(false);
	}
	private void Update()
	{
		if (_isShow) 
		{
			transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
		}
	}
	public void ShowGhostIcon(Sprite sprite) 
	{
		if (sprite == null) return;
		transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
		_icon.sprite = sprite;
		gameObject.SetActive(true);
		_isShow = true;
	}

	public void HideGhostIcon() 
	{
		_isShow = false;
		gameObject.SetActive(false);
	}
}
