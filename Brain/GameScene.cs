using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    internal class GameScene : DrawableEntity, Level
    {
        public static GameScene Instance;
        private readonly GraphicsDevice graphicsDevice;
        private readonly SpriteBatch spriteBatch;

        private readonly Matrix camera2D;
        private bool hasStopped;

        public GameScene(GraphicsDevice graphicsDevice, Matrix camera)
        {
            this.graphicsDevice = graphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);
            Add(new FadeScreen(255, 0, 1));
            Add(new MousePointerEntity());
            Add(new GameFlowBehavior());
            Add(new Score() { LocalPosition = new Vector2(-115, -80) });

            camera2D = camera;
            Instance = this;
        }

        public override void Update(GameTime gameTime)
        {
            if (GameState.GameOver && !hasStopped)
            {
                hasStopped = true;
                Add(new GameOverEntity());
            }

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            Texture2D texture = BrainGame.ContentManager.Load<Texture2D>("Background");
            Rectangle viewportRectangle = new Rectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            spriteBatch.Draw(texture, viewportRectangle, viewportRectangle, Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap,
                DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, camera2D);
            base.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}