using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    internal class FadeScreen : SpriteEntity
    {
        private readonly SteppingTimer timer;
        private int startAlpha;
        private int endAlpha;
        private int currentAlpha;

        public FadeScreen(int startAlpha, int endAlpha, float time)
        {
            this.startAlpha = startAlpha;
            this.endAlpha = endAlpha;
            timer = new SteppingTimer(time, UpdateAlpha);
            Add(timer);
        }

        public FadeScreen(int endAlpha, float time) : this(0, endAlpha, time) { }

        private void UpdateAlpha(float x)
        {
            currentAlpha = (int)(startAlpha * (1 - x) + endAlpha * x);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            var screenResolution = new Vector2(BrainGame.gameWidth, BrainGame.gameHeight);
            spriteBatch.Draw(BrainGame.Pixel, new Vector2(), new Rectangle(0, 0, 1, 1), new Color(0, 0, 0, currentAlpha),
                0, new Vector2(0.5f), screenResolution, SpriteEffects.None, 0.3f);
        }

        public void ChangeAlphaTo(int newAlpha)
        {
            startAlpha = endAlpha;
            endAlpha = newAlpha;
            timer.Restart();
        }
    }
}