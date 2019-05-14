using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    internal class HostEntity : SpriteEntity, Interactable
    {
        private readonly Color deathColor = new Color(170, 143, 139);

        public float VisitingTimeLeft => leaveTimer.UnitTimeLeft;
        public float Burnout;
        public float ChainDepth;
        public bool IsBeingDrained;
        public float RetrievedProcess;

        public bool IsInteractable => isAlive && !leaveTimer.Finished;
        private bool isAlive = true;

        private readonly HostBorder hostBorder;
        private readonly ActionTimer leaveTimer;
        private readonly SpriteEntity back;
        private readonly SpriteEntity border;
        private float elapsedTime;
        private readonly int hostIndex;

        public HostEntity()
        {
            hostIndex = BrainGame.Random.Next(1, 7);
            Add(new BoxBehavior() { Width = 50, Height = 50 });
            hostBorder = new HostBorder();
            Add(hostBorder);

            back = new SpriteEntity("HostBack") { Depth = 0.6f };
            border = new SpriteEntity("HostBorder") { Depth = 0.4f };
            Add(back);
            Add(border);
            Add(new TextEntity(Texts.NextName(), new Vector2(0, 22)) { CenteredHorizontally = true });

            leaveTimer = new ActionTimer(GameState.HostTime, Leave);
            Add(leaveTimer);
        }

        public float Drain()
        {
            IsBeingDrained = true;
            var drain = GameState.BrainDrain * (float)Math.Pow(GameState.BrainDrainDepthMultiplier, ChainDepth);
            Burnout += drain * elapsedTime;
            var result = RetrievedProcess + GameState.BrainProcessing * elapsedTime;
            RetrievedProcess = 0;
            return result;
        }

        public void Process(float processing)
        {
            RetrievedProcess += processing;
        }

        private void Leave()
        {
            ConnectionHelper.RemoveConnections(this);
            Delay(1, () => Add(new AnimateMoveBehavior(new Vector2(250 + BrainGame.Random.Next(-20, 20), BrainGame.Random.Next(-50, 50)), 1)));
            Delay(2, () => Parent.Remove(this));
        }

        public override void Update(GameTime gameTime)
        {
            if (GameState.GameOver)
            {
                leaveTimer.Stop();
                return;
            }

            elapsedTime = (float) gameTime.ElapsedGameTime.TotalSeconds;

            UpdatePortrait();

            if (Burnout >= 1 && isAlive)
            {
                Color = deathColor;
                back.Color = deathColor;
                border.Color = deathColor;
                hostBorder.Killed();
                isAlive = false;
                leaveTimer.Stop();
                ConnectionHelper.KillConnections(this);
            }

            if (isAlive)
            {
                hostBorder.SetBurnout(Burnout);

                if (!IsBeingDrained)
                    Burnout = Math.Max(0, Burnout - GameState.BrainRecharge * elapsedTime);
            }

            IsBeingDrained = false;

            base.Update(gameTime);
        }

        private void UpdatePortrait()
        {
            var expectedPortraitIndex = 0;
            if (Burnout < 1/3f)
                expectedPortraitIndex = 1;
            else if (Burnout < 2/3f)
                expectedPortraitIndex = 2;
            else if (Burnout < 1)
                expectedPortraitIndex = 3;
            else
                expectedPortraitIndex = 4;

            SetTexture("host_images/Host_" + hostIndex + "_" + expectedPortraitIndex);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            var timeLeftLength = 15 * leaveTimer.UnitTimeLeft;
            var color = new Color(111, 190, 96);
            var y = 21;
            if(isAlive)
            {
                spriteBatch.DrawLine(Position + new Vector2(0, y), Position + new Vector2(0 - timeLeftLength, y), color, 15, 0.51f);
                spriteBatch.DrawLine(Position + new Vector2(0, y), Position + new Vector2(0 + timeLeftLength, y), color, 15, 0.51f);
            }
        }
    }

    internal class HostBorder : SpriteEntity
    {
        private readonly Color deathColor = new Color(50, 17, 12);
        private readonly Color goodColor = new Color(111, 190, 96);
        private readonly Color badColor = new Color(192, 40, 16);

        public HostBorder()
        {
            SetTexture("HostHealthBorder");
            Depth = 0.4f;
        }

        public void SetBurnout(float burnout)
        {
            Color = Color.Lerp(goodColor, badColor, burnout);
        }

        public void Killed()
        {
            Color = deathColor;
        }
    }
}
