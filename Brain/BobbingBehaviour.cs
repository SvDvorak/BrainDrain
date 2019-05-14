using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Brain
{
    class BobbingBehaviour : Entity
    {
        Vector2 startPosition;
        Vector2 bobDelta;

        protected override void onParentChanged(Entity parent)
        {
            startPosition = parent.Position;

            base.onParentChanged(parent);
        }

        public override void Update(GameTime gameTime)
        {
            bobDelta.Y = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 6) * 5;
            Parent.LocalPosition = startPosition + bobDelta;
            base.Update(gameTime);
        }
    }
}
