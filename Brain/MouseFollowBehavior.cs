using Microsoft.Xna.Framework;

namespace Brain
{
    class MouseFollowBehavior : Entity
    {
        public override void Update(GameTime gameTime)
        {
            Point mousePoint = GameSpaceMouse.Point;
            Parent.LocalPosition = new Vector2(mousePoint.X, mousePoint.Y);

            base.Update(gameTime);
        }
    }
}