namespace HalfDog.GameMonoUpdater
{
	/// <summary>
	/// 可使用游戏MonoUpdater
	/// </summary>
	public interface ICanUseGameMonoUpdater
    {
        public void FixedUpdate();
        public void Update();
        public void LateUpdate();
        public bool InUpdate { get; set; }
    }

	/// <summary>
	/// 游戏MonoUpdater扩展
	/// </summary>
	public static class CanUseMonoUpdaterExtension
    {
		/// <summary>
		/// 启用MonoUpdater
		/// </summary>
		public static void EnableMonoUpdater(this ICanUseGameMonoUpdater self)
        {
            if (self.InUpdate) return;
            GameMonoUpdater.AddUpdateAction(self.Update);
            GameMonoUpdater.AddFixedUpdateAction(self.FixedUpdate);
            GameMonoUpdater.AddLateUpdateAction(self.LateUpdate);
            self.InUpdate = true;
        }
		/// <summary>
		/// 禁用MonoUpdater
		/// </summary>
		public static void DisableMonoUpdater(this ICanUseGameMonoUpdater self)
        {
            GameMonoUpdater.RemoveUpdateAction(self.Update);
            GameMonoUpdater.RemoveFixedUpdateAction(self.FixedUpdate);
            GameMonoUpdater.RemoveLateUpdateAction(self.LateUpdate);
            self.InUpdate = false;
        }
    }
}