using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Brain
{
    class GameSpaceMouse
    {
        public static Point Point
        {
            get
            {
                MouseState mouseState = Mouse.GetState();
                Point screenSpacePoint = new Point(mouseState.X, mouseState.Y);
                return convertScreenSpaceToGameSpace(screenSpacePoint);
            }
        }

        public static int X => Point.X;

        public static int Y => Point.Y;

        //public Vector2 FromScreenspace(Vector2 pos)
        //{
        //    //pos.X -= Frame.Viewport.X;
        //    //pos.Y -= Frame.Viewport.Y;
        //    return Vector2.Transform(Frame != null ? Frame.TransformScreenToViewport(pos) : pos, GetViewInverse());
        //}
        //public Vector2 TransformScreenToViewport(Vector2 input) => new Vector2((input.X - Collection.Location.X) / Collection.Scale - Viewport.X, (input.Y - Collection.Location.Y) / Collection.Scale - Viewport.Y);

        static int gameWidth;
        static int gameHeight;
        static Matrix cameraTranslationMatrix;
        static GraphicsDevice graphicsDevice;

        public static void Setup(int gameWidth, int gameHeight, Matrix cameraTranslationMatrix, GraphicsDevice graphicsDevice)
        {
            GameSpaceMouse.gameWidth = gameWidth;
            GameSpaceMouse.gameHeight = gameHeight;
            GameSpaceMouse.cameraTranslationMatrix = cameraTranslationMatrix;
            GameSpaceMouse.graphicsDevice = graphicsDevice;
        }

        static Point convertScreenSpaceToGameSpace(Point screenSpacePoint)
        {
            float xScale = gameWidth / (float)graphicsDevice.PresentationParameters.BackBufferWidth;
            float yScale = gameHeight / (float)graphicsDevice.PresentationParameters.BackBufferHeight;

            Vector2 scaledPosition = new Vector2(screenSpacePoint.X, screenSpacePoint.Y) * new Vector2(xScale, yScale);
            Vector2 gameSpacePosition = Vector2.Transform(scaledPosition, Matrix.Invert(cameraTranslationMatrix));

            return new Point((int)gameSpacePosition.X, (int)gameSpacePosition.Y);
        }
    }
}
