using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    internal class TextEntity : DrawableEntity
    {
        public string Text;
        public Color Color = new Color(40, 36, 36);
        public float Depth = 0.4f;
        private SpriteFont font;

        public TextEntity(string text, Vector2 position)
        {
            this.Text = text;
            LocalPosition = position;

            LoadFont("GameFont");
        }

        public bool CenteredHorizontally { get; set; }

        private void LoadFont(string path)
        {
            font = BrainGame.ContentManager.Load<SpriteFont>(path);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var textSize = font.MeasureString(Text);
            var splitText = Text.Split('\n');
            var horizontalOrigin = CenteredHorizontally ? textSize.X / 2 : 0;
            for (var index = 0; index < splitText.Length; index++)
            {
                var t = splitText[index];
                spriteBatch.DrawString(font, t, Position + new Vector2(0, index * 12), Color,
                    0, new Vector2(horizontalOrigin, textSize.Y / 2), Vector2.One, SpriteEffects.None, Depth);
            }

            base.Draw(gameTime, spriteBatch);
        }
    }
}