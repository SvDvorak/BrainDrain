using System;
using Microsoft.Xna.Framework;

namespace Brain
{
    internal class Timer : Entity
    {
        public float Time { get; private set; }
        public float Elapsed { get; private set; }
        public float TimeLeft => MathHelper.Clamp(Time - Elapsed, 0, Time);
        public float UnitTimeLeft => TimeLeft / Time;

        public Timer(float time)
        {
            Start(time);
        }

        public void Start(float time)
        {
            Elapsed = 0;
            Time = time;
            IsActive = true;
        }

        public void Restart()
        {
            Start(Time);
        }

        public void Stop()
        {
            IsActive = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(IsActive)
                Elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public bool Finished => Elapsed > Time;
    }

    internal class ActionTimer : Timer
    {
        private readonly Action finished;

        public ActionTimer(float time, Action finished) : base(time)
        {
            this.finished = finished;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsActive && Finished)
            {
                Stop();
                finished();
            }
        }
    }

    internal class IntervalTimer : ActionTimer
    {
        private readonly float time;

        public IntervalTimer(float time, Action lap) : base(time, lap)
        {
            this.time = time;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Finished)
                Start(time);
        }
    }

    internal class SteppingTimer : Timer
    {
        private readonly Action<float> step;

        public SteppingTimer(float time, Action<float> step) : base(time)
        {
            this.step = step;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var ratio = MathHelper.Clamp(Elapsed / Time, 0, 1);
            step(ratio);
        }
    }
}