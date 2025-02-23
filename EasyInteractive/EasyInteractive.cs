using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HalfDog.GameMonoUpdater;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HalfDog.EasyInteractive
{
	public class EasyInteractive : ICanUseGameMonoUpdater
	{
		private static EasyInteractive _instance;

		public static EasyInteractive Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new EasyInteractive();
				}
				return _instance;
			}
		}

		private IFocusable _currentFocused;
		private IFocusable _previousFocused;
		private ISelectable _currentSelected;
		private IDragable _currentDraged;
		private ISelectable _readySelect;
		private IDragable _readyDrag;
		private IDragable _possibleDragTarget;
		private Vector3 _mouseDownPosition;
		private bool _isPointerOverUI;
		private Dictionary<Type,IInteractCase> _allInteractCase = new Dictionary<Type,IInteractCase>();
		private List<IInteractCase> _executingInteractCases = new List<IInteractCase>();
		private List<IInteractCase> _activeInteractCase = new List<IInteractCase>();
		private Stack<IFocusable> _currentUIItemFocusOnStack = new();
		//当前激活的交互情景
		private IInteractCase _currentActiveInteractCase;
		private bool _inUpdate;

		public bool InUpdate
		{
			get => _inUpdate;
			set => _inUpdate = value;
		}

		public bool isPointerOverUI => _isPointerOverUI;
		public IFocusable currentFocused => _currentFocused;
		public ISelectable currentSelected => _currentSelected;
		public ISelectable readySelect { get => _readySelect; set => _readySelect = value; }
		public IDragable currentDraged => _currentDraged;
		public IDragable readyDrag { get => _readyDrag; set => _readyDrag = value; }

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void MakesureInstanceExist()
		{
			_instance = new EasyInteractive();
		}

		public EasyInteractive()
		{
			this.EnableMonoUpdater();
			//加载所有的交互情景
			Assembly assembly = typeof(EasyInteractive).Assembly;
			List<Type> types = assembly.GetTypes().Where(type => typeof(IInteractCase).IsAssignableFrom(type) && !type.IsAbstract).ToList();
			for (int i = 0; i < types.Count; i++)
			{
				InteractCaseAttribute attribute = types[i].GetCustomAttribute<InteractCaseAttribute>();
				if (attribute != null) {
					IInteractCase ic = Activator.CreateInstance(types[i], attribute.interactSubject, attribute.interactTarget) as IInteractCase;
					_allInteractCase.Add(types[i],ic);
					ic.enable = attribute.executeOnLoad;
					_executingInteractCases.Add(ic);
				}
			}
		}
		
		public void Update()
		{
			//满足条件的InteractCase会被执行
			//这里的满足条件指的是 交互操作与交互对象类型都要一致
			IInteractCase activeCase = null;
			for (int i = 0; i < _executingInteractCases.Count; i++)
			{
				if (_executingInteractCases[i].enable && _executingInteractCases[i].Execute(currentFocused, currentSelected, currentDraged))
				{
					activeCase = _executingInteractCases[i];
				}
			}
			//如果当前激活的情景更改了，则把激活的情景放到列表最前方第一个进行处理
			//这是为了在情景更改时首先执行激活情景的退出事件(如果有的话)
			if (activeCase != null && activeCase != _currentActiveInteractCase)
			{
				_currentActiveInteractCase = activeCase;
				_executingInteractCases.Remove(_currentActiveInteractCase);
				_executingInteractCases.Insert(0, _currentActiveInteractCase);
			}

			//当指针处于UI上时停止对场景中交互对象的操作
			if (EventSystem.current?.IsPointerOverGameObject() ?? false)
			{
				if (!_isPointerOverUI)
				{
					//ATTENTION 只有当前选择的和准备选择相同时才执行这个操作 否则会出现问题（PointerHandler先于OnFixedUpdate调用）
					if (currentSelected == _readySelect)
						_readySelect = null;
					//ATTENTION 如果从场景转到UI上但是当前聚焦的对象没有RectTransform组件说明当前UI不是一个交互对象
					if (currentFocused != null && !(currentFocused as MonoBehaviour).TryGetComponent(out RectTransform component))
						SetCurrentFocused(null);
					_isPointerOverUI = true;
				}
			}
			else
			{
				if (Camera.main == null) return;
				_isPointerOverUI = false;
				Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity))
				{
					if (hitInfo.transform.TryGetComponent(out IFocusable focus))
					{
						if (focus != _currentFocused && focus.enableFocus)
							SetCurrentFocused(focus);
					}
					else
					{
						SetCurrentFocused(null);
					}

					if (hitInfo.transform.TryGetComponent(out ISelectable selectable) && selectable.enableSelect)
						_readySelect = selectable;
					else
						_readySelect = null;

					if (hitInfo.transform.TryGetComponent(out IDragable dragable) && dragable.enableDrag)
						_readyDrag = dragable;
					else
						_readyDrag = null;
				}
				else
				{
					_readyDrag = null;
					_readySelect = null;
					//射线没有任何碰撞应该把当前聚焦的对象置空
					SetCurrentFocused(null);
				}
			}


			
			//Debug.Log($"【Focus:{currentFocused?.interactTag.Name}】【Select:{currentSelected?.interactTag.Name}]】【Drag:{currentDraged?.interactTag.Name}】");

			if (Input.GetMouseButtonDown(0) && currentFocused!=null)
			{
				_mouseDownPosition = Input.mousePosition;
				_possibleDragTarget = (currentFocused as IDragable);
			}
			if (Input.GetMouseButtonUp(0))
			{
				if (currentDraged == null)
				{
					if (_readySelect != null)//Vector3.Distance(_mouseDownPosition, Input.mousePosition) < 32f && 
					{
						if (currentSelected != _readySelect) SetCurrentSelected(_readySelect);
						//再次点击选择的对象则取消选中
						else if (currentSelected == _readySelect) SetCurrentSelected(null);
					}
				}
				else 
				{
					SetCurrentDraged(null);
				}
			}
			if (Input.GetMouseButton(0)) 
			{
				if(currentDraged == null && Vector3.Distance(_mouseDownPosition, Input.mousePosition) > 16f) 
				{
					if (_possibleDragTarget != null && _readyDrag == _possibleDragTarget)
					{
						//拖拽与被选中的不能是同一个
						if (_readyDrag == currentSelected)
						{
							SetCurrentSelected(null);
						}
						SetCurrentDraged(_readyDrag);
					}
				}
				currentDraged?.ProcessDrag();
			}
			//Debug.Log(currentFocused?.interactTag.Name ?? "Null");

		}
		public void Reset() 
		{
			SetCurrentDraged(null);
			SetCurrentSelected(null);
			SetCurrentFocused(null);
		}
		public void SetCurrentFocused(IFocusable focusable,bool isUIItem = false)
		{
			_previousFocused = _currentFocused;
			_currentFocused?.EndFocus();
			_currentFocused = focusable;
			_currentFocused?.OnFocus();
		}
		public void SetCurrentSelected(ISelectable selectable)
		{
			_currentSelected?.EndSelect();
			_currentSelected = selectable;
			_currentSelected?.OnSelect();
		}
		public void SetCurrentDraged(IDragable dragable)
		{
			_currentDraged?.EndDrag();
			_currentDraged = dragable;
			_currentDraged?.OnDrag();
		}

		public void EnableInteractCase<T>() where T : IInteractCase 
		{
			Type type = typeof(T);
			if (_allInteractCase.ContainsKey(type)) 
			{
				_allInteractCase[type].enable = true;
			}
		}
		public void DisableInteractCase<T>() where T : IInteractCase 
		{
			Type type = typeof(T);
			if (_allInteractCase.ContainsKey(type))
			{
				_allInteractCase[type].enable = false;
			}
		}

		public void FixedUpdate() { }
		public void LateUpdate() { }
	}
}
