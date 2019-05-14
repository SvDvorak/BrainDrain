using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    class DrawableEntity : Entity
    {
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible)
                return;

            childrenOfType<DrawableEntity>().ForEach(x => x.Draw(gameTime, spriteBatch));
        }
    }
}