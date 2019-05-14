using Microsoft.Xna.Framework;

namespace Brain
{
    internal interface Level
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }
}