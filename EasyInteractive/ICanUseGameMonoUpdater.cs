namespace HalfDog.GameMonoUpdater
{
    public interface ICanUseGameMonoUpdater
    {
        public void FixedUpdate();
        public void Update();
        public void LateUpdate();
        public bool InUpdate { get; set; }
    }

    public static class CanUseMonoUpdaterExtension
    {
        public static void EnableMonoUpdater(this ICanUseGameMonoUpdater self)
        {
            if (self.InUpdate) return;
            GameMonoUpdater.AddUpdateAction(self.Update);
            GameMonoUpdater.AddFixedUpdateAction(self.FixedUpdate);
            GameMonoUpdater.AddLateUpdateAction(self.LateUpdate);
            self.InUpdate = true;
        }

        public static void DisableMonoUpdater(this ICanUseGameMonoUpdater self)
        {
            GameMonoUpdater.RemoveUpdateAction(self.Update);
            GameMonoUpdater.RemoveFixedUpdateAction(self.FixedUpdate);
            GameMonoUpdater.RemoveLateUpdateAction(self.LateUpdate);
            self.InUpdate = false;
        }
    }
}