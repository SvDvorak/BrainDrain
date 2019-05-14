using Microsoft.Xna.Framework;

namespace Brain
{
    internal class Score : DrawableEntity
    {
        private readonly TextEntity scoreText;
        private int currentlyDrawnFails = 0;

        public Score()
        {
            scoreText = new TextEntity("", new Vector2(-15, 0)) { CenteredHorizontally = true };
            Add(scoreText);
            Add(new SpriteEntity("ScoreBack") { Depth = 0.6f });
            Add(new SpriteEntity("ScoreBorder") { Depth = 0.4f });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            scoreText.Text = "$" + GameState.Score;

            if(GameState.Fails > currentlyDrawnFails)
            {
                Add(new SpriteEntity("Fail") { LocalPosition = new Vector2(18 + 9 * currentlyDrawnFails, 0) });
                currentlyDrawnFails += 1;
            }
        }
    }
}