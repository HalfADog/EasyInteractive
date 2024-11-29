using System;
using UnityEngine;

namespace HalfDog.GameMonoUpdater
{
	public class GameMonoUpdater : MonoBehaviour
	{
	    private static GameMonoUpdater _instance = null;
	    public event Action updateAction;
	    public event Action fixedUpdateAction;
	    public event Action lateUpdateAction;
		private static GameMonoUpdater instance 
		{
			get 
			{
				if (_instance == null) 
				{
					GameObject obj = new GameObject("[GameMonoUpdater]");
					_instance = obj.AddComponent<GameMonoUpdater>();
					DontDestroyOnLoad(obj);
				}
				return _instance;
			}
		}

		public static void AddUpdateAction(Action action) => instance.updateAction += action;
	    public static void RemoveUpdateAction(Action action) => instance.updateAction -= action;
		public static void AddFixedUpdateAction(Action action) => instance.fixedUpdateAction += action;
		public static void RemoveFixedUpdateAction(Action action) => instance.fixedUpdateAction -= action;
		public static void AddLateUpdateAction(Action action) => instance.lateUpdateAction += action;
		public static void RemoveLateUpdateAction(Action action) => instance.lateUpdateAction -= action;

		void Awake()
	    {
	        if (_instance != null) 
	        {
	            Destroy(gameObject);
	            return;
	        }
			_instance = this;
			DontDestroyOnLoad(gameObject);
	    }
	    void Update()
	    {
	        updateAction?.Invoke();
	    }

		private void FixedUpdate()
		{
			fixedUpdateAction?.Invoke();
		}
		private void LateUpdate()
		{
			lateUpdateAction?.Invoke();
		}
	}
}
