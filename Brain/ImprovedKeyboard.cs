using Microsoft.Xna.Framework.Input;

namespace Brain
{
    public static class ImprovedKeyboard
    {
        private static KeyboardState currentState;
        private static KeyboardState previousState;

        public static bool DidJustPress(Keys key) => currentState.IsKeyDown(key) && previousState.IsKeyUp(key);
        public static bool DidJustRelease(Keys key) => currentState.IsKeyUp(key) && previousState.IsKeyDown(key);
        public static bool Pressed(Keys key) => currentState.IsKeyDown(key);
        public static bool Released(Keys key) => currentState.IsKeyUp(key);

        public static void Update()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
        }
    }
}