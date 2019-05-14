using Microsoft.Xna.Framework;

namespace Brain
{
    internal class AnimateMoveBehavior : Entity
    {
        private readonly Vector2 newPosition;
        private readonly float totalTime;
        private float elapsed;
        private Vector2 startPosition;

        public AnimateMoveBehavior(Vector2 newPosition, float time)
        {
            this.newPosition = newPosition;
            totalTime = time;
        }

        protected override void onParentChanged(Entity parent)
        {
            if (parent != null)
            {
                startPosition = parent.Position;
            }
            base.onParentChanged(parent);
        }

        public override void Update(GameTime gameTime)
        {
            elapsed += (float) gameTime.ElapsedGameTime.TotalSeconds;

            var step = MathHelper.SmoothStep(0, 1, elapsed / totalTime);
            Parent.LocalPosition = Vector2.Lerp(startPosition, newPosition, step);

            if(elapsed > totalTime)
                Parent.Remove(this);

            base.Update(gameTime);
        }
    }
}
