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
					ic.enable = attribute.enableExecuteOnLoad;
					_executingInteractCases.Add(ic);
				}
			}
		}
		
		public void Update()
		{
			//执行所有的交互情景
			IInteractCase activeCase = null;
			for (int i = 0; i < _executingInteractCases.Count; i++)
			{
				//Execute方法返回true则表示当前情景被激活
				if (_executingInteractCases[i].enable && _executingInteractCases[i].Execute(currentFocused, currentSelected, currentDraged))
				{
					activeCase = _executingInteractCases[i];
				}
			}
			//为了在情景更改时首先执行激活情景的退出事件(如果有的话)
			if (activeCase != null && activeCase != _currentActiveInteractCase)
			{
				//如果当前激活的情景更改了，则把激活的情景放到列表最前方第一个进行处理
				_currentActiveInteractCase = activeCase;
				_executingInteractCases.Remove(_currentActiveInteractCase);
				_executingInteractCases.Insert(0, _currentActiveInteractCase);
			}

			//当指针处于UI上时停止对场景中交互对象的操作
			if (EventSystem.current?.IsPointerOverGameObject() ?? false)
			{
				//如果当前指针不在UI上但是之前是在UI上则重置当前聚焦对象
				if (!_isPointerOverUI)
				{
					if (currentSelected == _readySelect)
						_readySelect = null;
					//如果从场景转到UI上但是当前聚焦的对象没有RectTransform组件说明当前UI不是一个交互对象
					if (currentFocused != null && !(currentFocused as MonoBehaviour).TryGetComponent(out RectTransform component))
						SetCurrentFocused(null);
					_isPointerOverUI = true;
				}
			}
			else
			{
				if (Camera.main == null) return;
				_isPointerOverUI = false;
				//射线检测
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

			bool mouseBtnPressed = Input.GetMouseButton(0);
			if (currentFocused != null)
			{
				if (Input.GetMouseButtonDown(0))
				{
					_mouseDownPosition = Input.mousePosition;
					_possibleDragTarget = (currentFocused as IDragable);
				}
				//如果鼠标按下并且移动了一定距离则开始拖拽
				if (mouseBtnPressed)
				{
					if (currentDraged == null && Vector3.Distance(_mouseDownPosition, Input.mousePosition) > 16f)
					{
						if (_possibleDragTarget != null && _readyDrag == _possibleDragTarget)
						{
							//拖拽与被选中的不能是同一个
							if (_readyDrag == currentSelected)
							{
								//把选择的对象置空
								SetCurrentSelected(null);
							}
							//再设置拖拽
							SetCurrentDraged(_readyDrag);
						}
					}
				}
			}

			//拖拽处理
			if (mouseBtnPressed && currentDraged != null)
				currentDraged.ProcessDrag();

			if (Input.GetMouseButtonUp(0))
			{
				if (currentDraged == null)
				{
					if (_readySelect != null)
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
			//一些调试信息
			//Debug.Log(currentFocused?.interactTag.Name ?? "Null");
			//Debug.Log($"【Focus:{currentFocused?.interactTag.Name}】【Select:{currentSelected?.interactTag.Name}]】【Drag:{currentDraged?.interactTag.Name}】");
		}

		/// <summary>
		/// 重置所有的交互状态
		/// </summary>
		public void Reset() 
		{
			SetCurrentDraged(null);
			SetCurrentSelected(null);
			SetCurrentFocused(null);
		}
		/// <summary>
		/// 设置当前聚焦对象
		/// </summary>
		public void SetCurrentFocused(IFocusable focusable,bool isUIItem = false)
		{
			_previousFocused = _currentFocused;
			_currentFocused?.EndFocus();
			_currentFocused = focusable;
			_currentFocused?.OnFocus();
		}
		/// <summary>
		/// 设置当前选择对象
		/// </summary>
		public void SetCurrentSelected(ISelectable selectable)
		{
			_currentSelected?.EndSelect();
			_currentSelected = selectable;
			_currentSelected?.OnSelect();
		}
		/// <summary>
		/// 设置当前拖拽对象
		/// </summary>
		public void SetCurrentDraged(IDragable dragable)
		{
			_currentDraged?.EndDrag(currentFocused);
			_currentDraged = dragable;
			_currentDraged?.OnDrag();
		}
		/// <summary>
		/// 启用指定的交互情景
		/// </summary>
		public void EnableInteractCase<T>() where T : IInteractCase 
		{
			Type type = typeof(T);
			if (_allInteractCase.ContainsKey(type)) 
			{
				_allInteractCase[type].enable = true;
			}
		}
		/// <summary>
		/// 禁用指定的交互情景
		/// </summary>
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
