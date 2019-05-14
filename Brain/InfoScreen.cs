using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    internal class InfoScreen : DrawableEntity, Level
    {
        public bool Continue;
        private readonly GraphicsDevice graphicsDevice;
        private readonly SpriteBatch spriteBatch;
        private readonly Matrix camera2D;
        private readonly string image;

        public InfoScreen(GraphicsDevice graphicsDevice, Matrix camera, string image)
        {
            this.graphicsDevice = graphicsDevice;
            camera2D = camera;
            this.image = image;
            spriteBatch = new SpriteBatch(graphicsDevice);

            var cursor = new SpriteEntity() { Depth = 0.2f, LocalPosition = new Vector2(-80,0)};
            cursor.SetTexture("hand");
            cursor.Add(new MouseFollowBehavior());
            Add(cursor);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ImprovedMouse.DidJustLeftRelease)
                Continue = true;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            Texture2D texture = BrainGame.ContentManager.Load<Texture2D>(image);
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