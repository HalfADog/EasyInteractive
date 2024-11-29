using System;

namespace HalfDog.EasyInteractive
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InteractCaseAttribute : Attribute
    {
        public Type interactSubject;
        public Type interactTarget;
        public bool executeOnLoad;

        public InteractCaseAttribute(Type subject, Type target, bool executeOnLoad = true)
        {
            interactSubject = subject;
            interactTarget = target;
            this.executeOnLoad = executeOnLoad;
        }
    }
}
