using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Brain
{
    public static class ImprovedMouse
    {
        private static MouseState previousState;

        public static Point Position => GameSpaceMouse.Point;

        public static bool DidJustLeftClick { get; private set; }
        public static bool DidJustLeftRelease { get; private set; }
        public static bool DidJustRightClick { get; private set; }
        public static bool DidJustRightRelease { get; private set; }

        public static void Update()
        {
            var currentState = Mouse.GetState();

            DidJustLeftClick = previousState.LeftButton == ButtonState.Released
                               && currentState.LeftButton == ButtonState.Pressed;
            DidJustLeftRelease = previousState.LeftButton == ButtonState.Pressed
                                 && currentState.LeftButton == ButtonState.Released;
            DidJustRightClick = previousState.RightButton == ButtonState.Released
                                && currentState.RightButton == ButtonState.Pressed;
            DidJustRightRelease = previousState.RightButton == ButtonState.Pressed
                                  && currentState.RightButton == ButtonState.Released;

            previousState = currentState;
        }
    }
}