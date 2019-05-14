using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    internal class ContractEntity : SpriteEntity, Interactable
    {
        public float ProcessingLeft => 1 - Processed;
        public bool IsCompleted => Math.Abs(ProcessingLeft) < 0.001f;
        public bool IsInteractable => !IsCompleted && !failTimer.Finished;
        private readonly Timer failTimer;

        private float processed;
        public float Processed
        {
            get => processed;
            set => processed = MathHelper.Clamp(value, 0, 1);
        }

        public ContractEntity()
        {
            SetTexture("ContractBack");
            Add(new SpriteEntity("ContractBorder") { Depth = 0.4f });
            Add(new BoxBehavior() { Width = 94, Height = 33 });
            failTimer = new ActionTimer(GameState.ContractTime, Failed);
            Add(failTimer);
            Add(new TextEntity(Texts.NextContract(), new Vector2(-21, 1)));
        }

        public float Drain()
        {
            return 0;
        }

        public void Process(float processing)
        {
            Processed += processing;
        }

        public override void Update(GameTime gameTime)
        {
            if (GameState.GameOver)
            {
                failTimer.Stop();
                return;
            }

            if (IsCompleted && failTimer.IsActive)
                Completed();

            base.Update(gameTime);
        }

        private void Completed()
        {
            Color = Color.Green;
            GameState.Score += GameState.ScorePerContract + (int)(failTimer.UnitTimeLeft * GameState.MaxScoreTimeRemaining);
            failTimer.Stop();
            AnimateExit();
        }

        private void Failed()
        {
            Color = Color.Red;
            GameState.Fails += 1;
            AnimateExit();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            const int x = -30;
            var timeLeftLength = 13 * failTimer.UnitTimeLeft;
            spriteBatch.DrawLine(Position + new Vector2(x, 0), Position + new Vector2(x, 0 - timeLeftLength), GameColors.BadColor, 7, 0.49f);

            var processedLength = 13 * Processed;
            spriteBatch.DrawLine(Position + new Vector2(x, 14), Position + new Vector2(x, 14 - processedLength), GameColors.DataColor, 7, 0.49f);

            base.Draw(gameTime, spriteBatch);
        }

        private void AnimateExit()
        {
            ConnectionHelper.RemoveConnections(this);
            Delay(1, () => Add(new AnimateMoveBehavior(new Vector2(-250, BrainGame.Random.Next(-50, 50)), 1)));
            Delay(2, () => Parent.Remove(this));
        }
    }
}