using Microsoft.Xna.Framework;

namespace Brain
{
    internal class GameOverEntity : DrawableEntity
    {
        private readonly FadeScreen fade;
        private DrawableEntity textRoot;

        public GameOverEntity()
        {
            fade = new FadeScreen(200, 1);
            Add(fade);
            Delay(1, ShowText);
        }

        private void ShowText()
        {
            textRoot = new DrawableEntity() { LocalPosition = -Vector2.UnitY * BrainGame.gameHeight };
            textRoot.Add(new AnimateMoveBehavior(Vector2.Zero, 1));
            var depth = 0.11f;
            var color = new Color(236, 236, 236);
            textRoot.Add(new TextEntity("You're fired!", new Vector2(0, -16)) { CenteredHorizontally = true, Color = color, Depth = depth });
            textRoot.Add(new TextEntity("$" + GameState.Score, new Vector2(0, 0)) { CenteredHorizontally = true, Color = color, Depth = depth });
            textRoot.Add(new TextEntity("Click to restart", new Vector2(0, 16)) { CenteredHorizontally = true, Color = color, Depth = depth });
            Add(textRoot);
        }

        public override void Update(GameTime gameTime)
        {
            if (ImprovedMouse.DidJustLeftRelease && !HasQueuedActions)
            {
                fade.ChangeAlphaTo(255);
                textRoot.Add(new AnimateMoveBehavior(Vector2.UnitY * BrainGame.gameHeight, 1));
                Delay(1, () => BrainGame.Instance.Restart());
            }
            base.Update(gameTime);
        }
    }
}