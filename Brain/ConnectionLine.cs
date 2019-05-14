using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    internal interface Interactable
    {
        bool IsInteractable { get; }
        float Drain();
        void Process(float processing);
    }

    internal class LineColors
    {
        public Color Thick;
        public Color Thin;

        public LineColors(Color thick, Color thin)
        {
            Thick = thick;
            Thin = thin;
        }
    }

    internal class ConnectionLine : SpriteEntity
    {
        public Entity Start;
        public Entity End;
        private readonly float collisionThickness = 12;

        public bool IsHoveringOver { get; set; }
        public bool IsDead { get; set; }

        private readonly LineColors ActiveColors = new LineColors(new Color(163, 182, 236), GameColors.DataColor);
        private readonly LineColors DeadColors = new LineColors(new Color(156, 131, 124), new Color(35, 17, 12));
        private readonly LineColors IsHoveredOverColors = new LineColors(new Color(251, 124, 112), new Color(216, 71, 42));

        private IntervalTimer pulseTimer;
        private bool isFullyConnected;

        public ConnectionLine(Entity c1, Entity c2)
        {
            Set(c1, c2);
        }

        public void Connected(Entity c1, Entity c2)
        {
            Set(c1, c2);
            pulseTimer = new IntervalTimer(0.5f, SpawnPulse);
            Add(pulseTimer);
            isFullyConnected = true;
        }

        private void Set(Entity c1, Entity c2)
        {
            Start = c1;
            End = c2;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colors = ActiveColors;
            if (IsDead)
            {
                colors = DeadColors;
            }

            if (IsHoveringOver)
            {
                colors = IsHoveredOverColors;
            }

            spriteBatch.DrawLine(Start.Position, End.Position, colors.Thick, 4, 0.81f);
            spriteBatch.DrawLine(Start.Position, End.Position, colors.Thin, 2, 0.8f);

            IsHoveringOver = false;

            Visit<Pulse>(x =>
            {
                x.Color = colors.Thin;
            });

            base.Draw(gameTime, spriteBatch);
        }

        private void SpawnPulse()
        {
            Add(new Pulse(Start, End));
        }

        public override void Update(GameTime gameTime)
        {
            if (GameState.GameOver)
            {
                pulseTimer.Stop();
                return;
            }

            var processing = 0f;
            if (!IsDead && isFullyConnected)
            {
                if (Start is Interactable startInteractable)
                {
                    processing = startInteractable.Drain();
                }

                if (End is Interactable endInteractable)
                {
                    endInteractable.Process(processing);
                }
            }
            base.Update(gameTime);
        }

        public bool AlreadyHasConnection(Entity c1, Entity c2)
        {
            var alreadyHas = false;
            GameScene.Instance.Visit(x =>
            {
                if (x is ConnectionLine connection &&
                    connection != this &&
                    connection.Has(c1, c2))
                {
                    alreadyHas = true;
                }
            });
            return alreadyHas;
        }

        private bool Has(Entity c1, Entity c2)
        {
            return c1 == Start && c2 == End || c2 == Start && c1 == End;
        }

        public bool Has(Entity e)
        {
            return e == Start || e == End;
        }

        public bool HasPointOnLine(Point currentMousePoint)
        {
            var pos1 = Start.Position;
            var pos2 = End.Position;
            return Extensions.IsPointOnLine(pos1, pos2, collisionThickness, currentMousePoint.ToV2());
        }

        public void Wake()
        {
            if(Start is HostEntity start && start.IsInteractable && End is HostEntity end && end.IsInteractable)
            {
                IsDead = false;
                pulseTimer?.Restart();
            }
        }

        public void Kill()
        {
            IsDead = true;
            pulseTimer?.Stop();
        }
    }
}