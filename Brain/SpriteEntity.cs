using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    internal class SpriteEntity : DrawableEntity
    {
        public Rectangle SourceArea { get; set; }
        public Color Color { get; set; } = Color.White;
        public Vector2 Origin { get; set; }
        public float RotationRadians { get; private set; }
        public int Rotation { get; private set; }
        public float Depth { get; set; } = 0.5f;
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects Effects { get; set; }
        public Rectangle Bounds => new Rectangle((int)(Position.X - (Origin.X * Scale.X)),
            (int)(Position.Y - (Origin.Y * Scale.Y)),
            (int)(SourceArea.Width * Scale.X),
            (int)(SourceArea.Height * Scale.Y));

        private readonly Dictionary<string, Texture2D> loadedContent = new Dictionary<string, Texture2D>();
        private Texture2D currentTexture;
        private string currentPath;

        public SpriteEntity(string path = "")
        {
            if(!string.IsNullOrEmpty(path))
                SetTexture(path);
        }

        public void SetTexture(string path)
        {
            if (currentPath == path)
                return;

            LoadContent(path);
            currentTexture = loadedContent[path];
            currentPath = path;
        }

        private void LoadContent(string path)
        {
            if (loadedContent.ContainsKey(path))
                return;

            var content = BrainGame.ContentManager;
            if (!string.IsNullOrEmpty(path))
            {
                var texture = content.Load<Texture2D>(path);
                loadedContent.Add(path, texture);
                if (SourceArea == Rectangle.Empty)
                    SourceArea = new Rectangle(0, 0, texture.Width, texture.Height);
                if (Origin == Vector2.Zero)
                    Origin = new Vector2(SourceArea.Width / 2, SourceArea.Height / 2);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (currentTexture != null)
            {
                spriteBatch.Draw(currentTexture, new Vector2((int)Position.X, (int)Position.Y), SourceArea, Color, 0, Origin, Scale, Effects, Depth);
            }

            base.Draw(gameTime, spriteBatch);
        }
    }
}