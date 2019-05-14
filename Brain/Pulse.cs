using Microsoft.Xna.Framework;

namespace Brain
{
    internal class Pulse : SpriteEntity
    {
        private SteppingTimer move;

        public Pulse(Entity start, Entity end)
        {
            Depth = 0.78f;
            SetTexture("pulse");
            move = new SteppingTimer(1, x => { LocalPosition = Vector2.Lerp(start.Position, end.Position, x); });
            Add(move);
        }

        public override void Update(GameTime gameTime)
        {
            if(move.Finished)
                Parent.Remove(this);

            base.Update(gameTime);
        }
    }
}