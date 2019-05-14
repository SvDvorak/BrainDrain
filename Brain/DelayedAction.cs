using System;

namespace Brain
{
    class DelayedAction
    {
        public float Time;
        public readonly Action Action;

        public DelayedAction(float time, Action action)
        {
            Action = action;
            Time = time;
        }
    }
}