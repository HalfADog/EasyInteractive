using System;

namespace HalfDog.EasyInteractive
{
	/// <summary>
	/// 交互情景标识
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
    public class InteractCaseAttribute : Attribute
    {
        public Type interactSubject;
        public Type interactTarget;
        public bool enableExecuteOnLoad;

		/// <summary>
		/// 交互情景标识
		/// </summary>
		/// <param name="subject">交互主体类型</param>
		/// <param name="target">交互目标类型</param>
		/// <param name="enableExecuteOnLoad">默认开启执行</param>
		public InteractCaseAttribute(Type subject, Type target, bool enableExecuteOnLoad = true)
        {
            interactSubject = subject;
            interactTarget = target;
            this.enableExecuteOnLoad = enableExecuteOnLoad;
        }
    }
}
